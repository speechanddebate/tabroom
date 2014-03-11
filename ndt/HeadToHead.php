<?php
require 'scripts/tabroomtemplate.html';
?>

<body>

<?php                    // this does seasonal totals
$time_start = microtime(true);

require 'scripts/databaseconnect.php';

$tournid=$_GET['tourn'];
$panel_id = array();
$panel_entry_sync = array();  //new number for all entries with those 2 debaters
$panel_entry_orig = array();  //original entry number on panel
$panel_entry_waitlist = array();  //whether entry is on the waitlist
$sum_team=array(); $sum_win=array(); $sum_loss=array();

//pull all entries
$query="SELECT * from entry where entry.tourn=".$tournid;
$entries=mysql_query($query);
$entryNum = mysql_num_rows($entries);

//scroll entries to make local panels table

   while ($row = mysql_fetch_array($entries, MYSQL_BOTH)) 
    {panelmaker($row['id'], $panel_id, $panel_entry_sync, $panel_entry_orig, $panel_entry_waitlist, $row['waitlist']);}
 //array_multisort($panel_entry_sync, $panel_entry_orig, $panel_entry_waitlist, $panel_id);
 
//set up stuff at the top
?>
<a name="Top"></a>
<h2>Head To Head
<?php
echo " for ".gettournname($tournid)." ";
if (empty($_GET['mode'])) {$mode="EE";} else {$mode=$_GET['mode'];}
$modestr="(Waitlist vs. Waitlst)";
if ($mode=="EE") {$modestr="(Entered vs. Entered)";}
if ($mode=="EW") {$modestr="(Waitlist vs. Entered)";}
echo $modestr;
?>
</h2>
INSTRUCTIONS: This page will let you view waitlisted teams against each other, entered teams against each other, or waitlisted teams against entered teams.  
Click a link on the line below to generate your desired comparison.  
The top of the page will list teams one at a time, and clicking the hide/show detail links will display their entire history round-by-round.  
A summary table of all the records will appear at the bottom of the page.  
NDT RANKING: the waitlist vs. waitlist comparison will allow you to view the head-to-head records of the first- and second-round applicants.
The waitlist vs. entered comparison will show the records of second-round applicants against teams entered in the NDT.  
Note that the first-round displays will not be acurate until all teams have applied and are on the waitlist, and the second-round comparison will not 
be accurate until all the district qualifiers have entered and all second-round applicants have been added to the waitlist.<br><r>
<a href='https://www.tabroom.com/jbruschke/HeadToHead.php?tourn=<?php echo $tournid; ?>&mode=WW'>Waitlist vs. waitlist</a>
<a href='https://www.tabroom.com/jbruschke/HeadToHead.php?tourn=<?php echo $tournid; ?>&mode=EE'> Entered vs. Entered</a>
<a href='https://www.tabroom.com/jbruschke/HeadToHead.php?tourn=<?php echo $tournid; ?>&mode=EW'> Waitlist vs. Entered</a>
<a href="#ResultTable"> Jump to summary table</a></br></br>

<?php
//scroll through each entry and create a table for them
   $x=0; $win=0; $loss=0; 
   mysql_data_seek($entries,0); //move back to the top of the array
   while ($row = mysql_fetch_array($entries, MYSQL_BOTH)) 
    {
     $displayOK=TRUE;
     if ($mode=="EE" and $row['waitlist']==1) {$displayOK=FALSE;}
     if ($mode=="WW" and $row['waitlist']==0) {$displayOK=FALSE;}
     if ($mode=="EW" and $row['waitlist']==0) {$displayOK=FALSE;}
     if ($displayOK==TRUE) 
      {
       echo "<strong>".$row['code']." - ".$row['name']."</strong>";
       ?>
         <a onclick="document.getElementById('<?php echo "tbl".$x; ?>').style.display='';">show detail</a>
         <a onclick="document.getElementById('<?php echo "tbl".$x; ?>').style.display='none';">hide detail</a>
       <?php
      echo "<table id='tbl".$x."' class='hovertable sortable' border='2' cellspacing='2' cellpadding='2' style='display:none'>";
      echo "<thead>";
      echo "<tr><th style='text-align:center' colspan=4><B>".$row['name']."</B>";
      echo "<tr class='yellowrow'>";
      echo " <th class='smallish'>Tourney</th>";
      echo " <th class='smallish'>Round</th>";
      echo " <th class='smallish'>Opponent</th>";
      echo " <th class='smallish'>Judge</th>";
      echo " <th class='smallish'>Win/Loss</th>";
      echo "</tr></thead><tbody>";
      printdebates($row['id'], $panel_id, $panel_entry_sync, $panel_entry_orig, $win, $loss, $mode, $panel_entry_waitlist);
      echo "</table>";
      if ($displayOK==TRUE) {echo " (".$win."-".$loss.")</br>";}
      $sum_team[$x]=$row['code']." - ".$row['name']; $sum_win[$x]=$win; $sum_loss[$x]=$loss;
      } 
      $x++; 
    }
?>

<?php

mysql_close();

function printdebates($entry, $panel_id, $panel_entry_sync, $panel_entry_orig, &$win, &$loss, $mode, $panel_entry_waitlist)
{
 $x=0; $win=0; $loss=0; $opponent=0; //$opponent=opponent number as entry for current tournament
 while ($x<count($panel_id))
  {
   $x++;
   if ($entry==$panel_entry_sync[$x] and nduplicates($panel_id[$x], $panel_id)>1)
    {
    $oppn=""; $judge=""; $decision="";
    getopponent($panel_id[$x], $panel_entry_orig[$x], $oppn, $judge, $decision, $opponent);
    $oppwaitlist=getoppwaitlist($panel_entry_orig, $panel_entry_waitlist, $opponent);
    //echo "Oppon on waitlist=".$oppwaitlist;
    $printok=FALSE;
    if ($mode=="EE" and $oppwaitlist==0) {$printok=TRUE;}
    if ($mode=="WW" and $oppwaitlist==1) {$printok=TRUE;}
    if ($mode=="EW" and $oppwaitlist==0) {$printok=TRUE;}
    //echo " printok=".$printok;
    if ($printok==TRUE)
     {
     //echo " printing<br>";
     echo "<tr>";
     echo "<td>".getvalue("Select tourn.name as tourn_name from tourn, round, panel, event where tourn.id=event.tourn and event.id=round.event and round.id=panel.round and panel.id=".$panel_id[$x], "tourn_name")."</td>";
     echo "<td>".getvalue("Select label from round, panel where round.id=panel.round and panel.id=".$panel_id[$x], "label")."</td>";
     echo "<td>".$oppn."</td>";
     echo "<td>".$judge."</td>";
     echo "<td>".$decision."</td>";
     if (iswin($decision)=="W") {$win++;} else {$loss++;}
     echo "</tr>";
     }
    }
  }
}

?>

<br><br>
<a name="ResultTable"></a>
<h2>Summary Table<span style="font-size:24px"><a href="#Top"> Jump to top</a></span></h2>
       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Team</th>
		<th class="smallish">Win</th>
		<th class="smallish">Loss</th>
		<th class="smallish">Pct</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php

$i=0;
while ($i < $x) {
if (isset($sum_team[$i]))
 {
 echo "<tr>";
 echo "<td>".$sum_team[$i]."</td>";
 echo "<td>".$sum_win[$i]."</td>";
 echo "<td>".$sum_loss[$i]."</td>";
 if ($sum_win[$i]+$sum_loss[$i]>0)
 {echo "<td>".round($sum_win[$i]/($sum_win[$i]+$sum_loss[$i]) * 100) . "%</td>";}
 else {echo "<td>-</td>";}
 echo "</tr>";
 }
$i++;
}
?>

	</tbody>
       </table>

<?php

function getoppwaitlist($panel_entry_orig, $panel_entry_waitlist, $opponent)
{
 for ($i = 1; $i < count($panel_entry_orig); ++$i) 
    {
     if ($panel_entry_orig[$i]==$opponent) 
      {
       //echo "Opponent=".$opponent." oppon orig=".$panel_entry_orig[$i]." waitlist=".$panel_entry_waitlist[$i]."<br>";
       return $panel_entry_waitlist[$i];
      }
    }
}

function iswin($decision)
{
 $balfor=substr_count($decision, "W");
 $balvs=substr_count($decision, "L");
 //echo $decision." ".$balfor." ".$balvs."<br>";
 if ($balfor>$balvs) {return "W";}
 return "L";
}

function getopponent($panel_id, $panel_entry_orig, &$oppn, &$judge, &$decision, &$opponent)  //returns opponent, judge, and decision
{
  $query="SELECT ballot.* from ballot, panel, round where ballot.panel=".$panel_id." and panel.id=ballot.panel and round.id=panel.round and round.published>0 order by judge asc";
  $ballots=mysql_query($query);
  $judgenum=-99;
  while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
    {
     if ($row['entry']<>$panel_entry_orig) {$opponent=$row['entry'];}
     if ($row['judge']<>$judgenum) {addjudgename($judge, $row['judge']);}
     if ($row['entry']==$panel_entry_orig) {decisionmaker($row['id'], $decision);}
     $judgenum=$row['judge'];
    }
  $oppn=getfullteamname($opponent);
}

function decisionmaker($ballot_id, &$decision)
{
  $query="SELECT ballot_value.value as ballot_decision from ballot_value where ballot_value.ballot=".$ballot_id." and ballot_value.tag='ballot'";
  //echo $query."<br>";
  $ballots=mysql_query($query);
  while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
    {
     if ($row['ballot_decision']==1) {$decision.="W";}
     if ($row['ballot_decision']==0) {$decision.="L";}
    }
}

function addjudgename(&$strjudgename, $judge)
{
 if ($strjudgename<>"") {$strjudgename.=", ";}
  $query="SELECT * from judge where id=".$judge;
  $judge=mysql_query($query);
  while ($row = mysql_fetch_array($judge, MYSQL_BOTH)) 
    {
     $strjudgename.=$row['first']." ".$row['last'];
    }
}

function gettournname($tournid)
{
  $query="SELECT * from tourn where id=".$tournid;
  $tourn=mysql_query($query);
  while ($row = mysql_fetch_array($tourn, MYSQL_BOTH)) 
    {
     return $row['name'];
    }
}


function getfullteamname($entry)
{
   $schoolname="";$fullname=""; $x=0; $school1=0; $school2=0;
   $query="SELECT *, chapter.name as chapter_name, chapter.id as chapter_id FROM student, entry_student, chapter WHERE entry_student.entry=".$entry." and student.id=entry_student.student and chapter.id=student.chapter";
   $student=mysql_query($query);
   while ($row = mysql_fetch_array($student, MYSQL_BOTH)) 
    {
    $x++;
    if ($fullname<>"") {$fullname.=" and ";}
    $fullname.=$row['first']." ".$row['last'];
    if ($x==1) {$school1=$row['chapter_id'];}
    if ($x==1) {$schoolname=$row['chapter_name'];}
    if ($x==2) {$school2=$row['chapter_id'];}
    if ($x==2 and $school1<>$school2) {$schoolname.="/".$row['chapter_name'];}
    }
    return $schoolname." -- ".$fullname;
}

function getvalue($query, $fieldtoreturn)
{
  $outcome=mysql_query($query);
  while ($row = mysql_fetch_array($outcome, MYSQL_BOTH)) 
    {return $row[$fieldtoreturn];}
}

function nduplicates($panel, $panel_id)
{
 $keys = array_keys($panel_id, $panel);
 return count($keys);
}

function panelcounter($entry, &$panel_id, &$panel_entry)
{
 $i=count($panel_entry);
 $x=1; $npanels=0;
 while ($x<=$i)
  {if ($panel_entry[$x]==$entry) {$npanels++;} 
   $x++;
  }
 return $npanels;
}

function panelmaker($entry, &$panel_id, &$panel_entry_sync, &$panel_entry_orig, &$panel_entry_waitlist, $waitlist)
{
  $entrylist=array();                                 //list of all entries that have the same entry_student records as the current entry
  findcommonentries($entry, $entrylist);              //populate it
  if (count($entrylist)==0) {return;}
  $year=date("Y"); $month=date("M"); if ($month<8) {$year=$year-1;}
  $query="SELECT distinct panel.id as panel_id, ballot.entry as entry from panel, ballot, round, event, tourn WHERE round.id=panel.round and event.id=round.event and tourn.id=event.tourn and tourn.start>'".$year."-09-01' and panel.id=ballot.panel and round.published>0 and ".entryquerymaker($entrylist);
  $panels=mysql_query($query);
  while ($row = mysql_fetch_array($panels, MYSQL_BOTH)) 
    {
     $i=count($panel_id)+1;
     $panel_id[$i]=$row['panel_id'];
     $panel_entry_sync[$i]=$entry;
     $panel_entry_orig[$i]=$row['entry'];
     $panel_entry_waitlist[$i]=$waitlist;
    }
}

function entryquerymaker($entrylist)
{
 $dummy="";
 $i=0;
 while ($i<count($entrylist))
 {
  if ($dummy=="") {$dummy=$dummy."(ballot.entry=".$entrylist[$i];}
  if ($dummy<>"") {$dummy=$dummy." or ballot.entry=".$entrylist[$i];}
  $i++;
 }
 $dummy.=")";
 return $dummy;
}

function findcommonentries($entry, &$entrylist)
{
 $student1=0;$student2=0;

 //pull the students from the current entry
 $query="SELECT * from entry_student where entry_student.entry=".$entry;
 $students=mysql_query($query);
   while ($row = mysql_fetch_array($students, MYSQL_BOTH)) 
    {
     if ($student1>0 and $student2==0) {$student2=$row['student'];}
     if ($student1==0) {$student1=$row['student'];}
    }

 //pull all entries that have either student on it
 $query="SELECT *, entry.name as entry_name, entry.id as entry_id from entry, entry_student where entry.id=entry_student.entry and (entry_student.student=".$student1." or entry_student.student=".$student2.") and entry.dropped=false ORDER BY entry.id ASC";
 $entries=mysql_query($query);

 //add the entries only if BOTH students appear on it
   $i=0; $priorlist=0;
   while ($row = mysql_fetch_array($entries, MYSQL_BOTH)) 
    { 
      //if ($row['entry_id'] == $entry) {echo "Target:".$row['entry_name']."</br>";}
      if ($row['entry_id'] == $priorlist) {$entrylist[$i]=$row['entry_id']; $i++;}
      $priorlist=$row['entry_id'];
    }
}

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

$time_end = microtime(true);
$time = $time_end - $time_start;
echo "<hr>Total load time is $time seconds\n";
require 'scripts/tabroomfooter.html';

?>
