<?php
require 'scripts/tabroomtemplate.html';
?>
<body>

<?php                    // this does seasonal totals
$time_start = microtime(true);

require 'scripts/databaseconnect.php';

$tournid=$_GET['tourn'];
$panel_id = array();
$panel_entry = array();

//pull all entries
$query="SELECT * from entry where entry.tourn=".$tournid;
$entries=mysql_query($query);
$entryNum = mysql_num_rows($entries);

//scroll entries to make local panels table

   while ($row = mysql_fetch_array($entries, MYSQL_BOTH)) 
    {panelmaker($row['id'], $panel_id, $panel_entry);}

//set up stuff at the top
?>
<h2>Head To Head
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/SeasonalTotals.php?division=open'>Waitlist vs. waitlist</a>
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/SeasonalTotals.php?division=jv'> Entered vs. Entered</a>
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/SeasonalTotals.php?division=novice'>Entered vs. Waitlist</a>
</span></h2>
<?php
//scroll through each entry and create a table for them
   mysql_data_seek($entries,0); //move back to the top of the array
   while ($row = mysql_fetch_array($entries, MYSQL_BOTH)) 
    {
     echo "<table id='sortme' class='hovertable sortable' border='2' cellspacing='2' cellpadding='2'>";
     echo "<thead>";
     echo "<tr><th style='text-align:center' colspan=4><B>".$row['name']."</B></th></tr>";
     echo "<tr class='yellowrow'>";
     echo " <th class='smallish'>Tourney</th>";
     echo " <th class='smallish'>Round</th>";
     echo " <th class='smallish'>Opponent</th>";
     echo " <th class='smallish'>Judge</th>";
     echo " <th class='smallish'>Win/Loss</th>";
     echo "</tr></thead><tbody>";
     printdebates($row['id'], $panel_id, $panel_entry);
     //echo "<td>".panelcounter($row['id'], $panel_id, $panel_entry)."</td></tr>";
     die;
    }
?>

	<tbody>
</table>

<?php

mysql_close();

function printdebates($entry, $panel_id, $panel_entry)
{
 $x=0;
 while ($x<count($panel_id))
  {
   $x++;
   if ($entry==$panel_entry[$x] and nduplicates($panel_id[$x], $panel_id)>1)
    {
    echo "<tr>";
    echo "<td>".getvalue("Select tourn.name as tourn_name from tourn, round, panel, event where tourn.id=event.tourn and event.id=round.event and round.id=panel.round and panel.id=".$panel_id[$x], "tourn_name")."</td>";
    echo "<td>".getvalue("Select label from round, panel where round.id=panel.round and panel.id=".$panel_id[$x], "label")."</td>";
    $oppn=""; $judge=""; $decision="";
    getopponent($panel_id[$x], $panel_entry[$x], $oppn, $judge, $decision);
    echo "<td>".$oppn."</td>";
    echo "<td>".$judge."</td>";
    echo "<td>".$decision."</td>";
    echo "</tr>";
    }
  }
}

function getopponent($panel_id, $panel_entry, &$oppn, &$judge, &$decision)  //returns opponent, judge, and decision
{
  $query="SELECT * from ballot where ballot.panel=".$panel_id." order by judge asc";
  $ballots=mysql_query($query);
  $judgenum=-99;
  while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
    {
     if ($row['entry']<>$panel_entry) {$opponent=$row['entry'];}
     if ($row['judge']<>$judgenum) {addjudgename($judge, $row['judge']); decisionmaker($row['id'], $panel_entry, $decision);}
     $judgenum=$row['judge'];
    }
  $oppn=getfullteamname($opponent);
}

function decisionmaker($ballot_id, $panel_entry, &$decision)
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
     $strjudgename=$row['first']." ".$row['last'];
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

function panelmaker($entry, &$panel_id, &$panel_entry)
{
  $entrylist=array();                                 //list of all entries that have the same entry_student records as the current entry
  findcommonentries($entry, $entrylist);              //populate it
  if (count($entrylist)==0) {return;}
  $query="SELECT distinct panel.id as panel_id from panel, ballot where panel.id=ballot.panel and ".entryquerymaker($entrylist);
  $panels=mysql_query($query);
  while ($row = mysql_fetch_array($panels, MYSQL_BOTH)) 
    {
     $i=count($panel_id)+1;
     $panel_id[$i]=$row['panel_id'];
     $panel_entry[$i]=$entry;
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
 $query="SELECT *, entry.id as entry_id from entry, entry_student where entry.id=entry_student.entry and (entry_student.student=".$student1." or entry_student.student=".$student2.") and entry.dropped=false ORDER BY entry.id ASC";
 $entries=mysql_query($query);
 //add the entries only if BOTH students appear on it
   $i=0; $priorlist=0;
   while ($row = mysql_fetch_array($entries, MYSQL_BOTH)) 
    { if ($row['entry_id'] == $priorlist) {$entrylist[$i]=$row['entry_id']; $i++;}
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
echo "Total load time is $time seconds\n";

require 'scripts/tabroomfooter.html';
?>
