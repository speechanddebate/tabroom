<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';
?>

<body>

<?php
$time_start = microtime(true);

$eresult = array();  //array to hold round and result; $eresult[0][0] is $eresult[round][panel]

$event=$_GET['event'];
$query="SELECT *, round.label as round_label, round.name as round_name, panel.id as panel_id FROM tourn, panel, round, event WHERE tourn.id=event.tourn and round.event=event.id and event.id=".$event." and panel.round=round.id and round.name>9 and round.post_results > 0";
$panel=mysql_query($query);
$entryNum = mysql_num_rows($panel);

$i=0; $Nround=0; $ctr=0; $lowround=99;
while ($i < $entryNum)
 {
  if (mysql_result($panel,$i,"round_name")<$lowround) {$lowround=mysql_result($panel,$i,"round_name");}
  //pull all ballots for the panel and construction a decision string
  $query="SELECT *, ballot.judge as ballot_judge, ballot.panel as ballot_panel, ballot.entry as ballot_entry, ballot_value.value as ballot_decision from ballot, ballot_value Where ballot.panel=".mysql_result($panel,$i,"panel_id")." and ballot.id=ballot_value.ballot and ballot_value.tag='ballot'";
  $ballot=mysql_query($query);

   $team1=""; $balfor1=""; $team2=""; $balfor2=""; $decision=""; $judge=array(); $votedfor=array();
   while ($row = mysql_fetch_array($ballot, MYSQL_BOTH)) 
    {
     //echo $row['side']." ".$row['ballot_entry']."<br>";
     if ($team1=="") {$team1=$row['ballot_entry']; $side1=$row['side'];}
     if ($team2=="" and $row['ballot_entry']<>$team1) {$team2=$row['ballot_entry']; $side2=$row['side'];}
     if ($team1==$row['ballot_entry']) {$balfor1 += $row['ballot_decision'];}
     if ($team2==$row['ballot_entry']) {$balfor2 += $row['ballot_decision'];}
     if ($row['ballot_decision']==1) {$votedfor[$row['ballot_judge']]=$row['ballot_entry'];}
     if (in_array($row['ballot_judge'], $judge)==FALSE) {array_push($judge,$row['ballot_judge']);}
    }
   if ($balfor1>$balfor2 and $team2>0) {$decision="<strong>".getfullteamname($team1)."</strong> (".getsidestring($side1).") defeat ".getfullteamname($team2)." (".$balfor1."-".$balfor2.") ";}
   if ($balfor1>$balfor2 and $team2==0) {$decision="<strong>".getfullteamname($team1)."</strong> advance without debating";}
   if ($balfor2>$balfor1 and $team1>0) {$decision="<strong>".getfullteamname($team2)."</strong> (".getsidestring($side2).") defeat ".getfullteamname($team1)." (".$balfor2."-".$balfor1.") ";}
   if ($balfor1>$balfor2 and $team1==0) {$decision="<strong>".getfullteamname($team2)."</strong> advance without debating";}
   $x=0;
   foreach ($judge as $item) 
    {
     $decision.=getjudgename($item, $x); 
     if ($balfor1>$balfor2 and $team1<>$votedfor[$item]) {$decision.="*";}
     if ($balfor2>$balfor1 and $team2<>$votedfor[$item]) {$decision.="*";}
     $x++;
    }

  if (mysql_result($panel,$i,"round_name") <> $Nround)
   {
    $ctr=0;
   }
  $ctr++; 
  $eresult[mysql_result($panel,$i,"round_name")][$ctr]=$decision;
  $Nround=mysql_result($panel,$i,"round_name");
  $i++;
 }
?>
<h2>Elimination rounds for <?php echo mysql_result($panel,0,"event.name")." at ".mysql_result($panel,0,"tourn.name"); ?></h2>
<a href='https://www.tabroom.com/jbruschke/TeamResults.php'>Back to Main Results Page</a></br></br>

       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
       <?php    
         if ($lowround<=10) {echo "<th class='smallish'>Quads</th>";}
         if ($lowround<=11) {echo "<th class='smallish'>Triples</th>";}
	  if ($lowround<=12) {echo "<th class='smallish'>Doubles</th>";}
         if ($lowround<=13) {echo "<th class='smallish'>Octos</th>";}
         if ($lowround<=14) {echo "<th class='smallish'>Quarters</th>";}
         if ($lowround<=15) {echo "<th class='smallish'>Semis</th>";}
         if ($lowround<=16) {echo "<th class='smallish'>Finals</th>";}
       ?>
	</tr>
	</thead>

     <tbody id="myTbodytotals">

<?php
if ($lowround==16) {$nrows=1;}
if ($lowround==15) {$nrows=2;}
if ($lowround==14) {$nrows=4;}
if ($lowround==13) {$nrows=8;}
if ($lowround==12) {$nrows=16;}
if ($lowround==11) {$nrows=32;}
if ($lowround==10) {$nrows=64;}
$i=1;
while ($i <= $nrows)
 {
  echo "<tr>";
  $x=$lowround;
  while ($x<=16)
   {
    if (empty($eresult[$x][$i])) {$eresult[$x][$i]="";}
    echo "<td>".$eresult[$x][$i]."</td>";
    $x++;
   }
  $i++;
  echo "</tr>";
 }

?>
    </tbody>
    </table>
<hr>

<?php                                                              //            Closing snippet and functions below

$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n"."</br>";

mysql_close();
require 'scripts/tabroomfooter.html';

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

function getfullteamname($entry)
{
   $schoolname="";$fullname=""; $x=0; $school1=0; $school2=0;
   if ($entry==0) {$schoolname="Bye"; return $schoolname;}
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
    $schoolname_temp=$schoolname;
    return $schoolname." -- ".$fullname;
}

function getschoolname($school)
{
   $schoolname="";
   $query="SELECT * FROM chapter WHERE id=".$school;
   $school=mysql_query($query);
   while ($row = mysql_fetch_array($school, MYSQL_BOTH)) 
     {$schoolname=$row['name'];}
    return $schoolname;
}

function getjudgename($judge, $njudge)
{
   $judgename="";
   $query="SELECT * FROM judge WHERE id=".$judge;
   $dude=mysql_query($query);
   while ($row = mysql_fetch_array($dude, MYSQL_BOTH)) 
     {$judgename=$row['first']." ".$row['last'];}
   if ($njudge>0) {$judgename=", ".$judgename;}
   return $judgename;
}

function getsidestring($side)
{
 if ($side==1) {return "Aff";}
 if ($side==2) {return "Neg";}
}
?>
