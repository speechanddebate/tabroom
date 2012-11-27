
<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';
?>

<body>
<?php                    // this does CEDA Checker pages
$time_start = microtime(true);

$studentid=$_GET['studentid'];
$tourney = array(); 
$event = array(); 
$outcome = array(); 
$teamname = array(); 
$points = array(); 
$x=0; $balfor=0; $balvs=0; $panel=0; $totpts=0;

//Print name
$query="SELECT * FROM student WHERE id=".$studentid;
$student=mysql_query($query);
$entryNum = mysql_num_rows($student);
$i=0;
while ($i < $entryNum) 
 {
 echo "<h2>idebate point totals FOR ".mysql_result($student,$i,"first")." ".mysql_result($student,$i,"last")."</h2><br>";
 $i++;
 }

$query="SELECT *, ballot_value.tag as ballot_value_tag, ballot_value.value as ballot_decision, panel.id as panel_id, round.label as round_name, tourn.name as tourn_name, event.name as event_name FROM entry_student, entry, tourn, event, ballot, panel, round, ballot_value WHERE event.type='policy' and ballot_value.tag='ballot' and ballot_value.ballot=ballot.id and round.id=panel.round and panel.id=ballot.panel and ballot.entry=entry.id and event.id=entry.event and tourn.id=entry.tourn and entry.id=entry_student.entry and entry_student.student=".$studentid." ORDER BY tourn.id, panel.id ASC";
$ballots=mysql_query($query); 
$entryNum = mysql_num_rows($ballots);
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
   {
      if ($row['ballot_decision'] == 1) {$balfor +=1;}
      if ($row['ballot_decision'] == 0) {$balvs +=1;}
      if ($row['panel_id'] <> $panel) 
        {
         $x++;
         $tourney[$x]=$row['tourn_name'];
         $event[$x]=$row['event_name'];
         $round[$x]=$row['round_name'];
         if($balfor > $balvs) {$outcome[$x] = "Win"; $points[$x]=6; $totpts+=6;}
         if($balfor < $balvs) {$outcome[$x] = "Loss"; $points[$x]=3; $totpts+=3;}
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
     }
   }

$query="SELECT *, event.type as event_type, ballot_value.tag as ballot_value_tag, ballot_value.value as ballot_decision, panel.id as panel_id, round.label as round_name, tourn.name as tourn_name, event.name as event_name FROM entry_student, entry, tourn, event, ballot, panel, round, ballot_value WHERE event.type='wudc' and ballot_value.tag='rank' and ballot_value.ballot=ballot.id and round.id=panel.round and panel.id=ballot.panel and ballot.entry=entry.id and event.id=entry.event and tourn.id=entry.tourn and entry.id=entry_student.entry and entry_student.student=".$studentid." ORDER BY tourn.id, panel.id ASC";
$ballots=mysql_query($query); 
$entryNum = mysql_num_rows($ballots);
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
   {
         $x++;
         $tourney[$x]=$row['tourn_name'];
         $event[$x]=$row['event_name'];
         $round[$x]=$row['round_name'];
         $outcome[$x]=$row['ballot_decision'];
         if ($row['ballot_decision']==1) {$points[$x]=6; $totpts+=6;}
         if ($row['ballot_decision']==2) {$points[$x]=5; $totpts+=5;}
         if ($row['ballot_decision']==3) {$points[$x]=4; $totpts+=4;}
         if ($row['ballot_decision']==4) {$points[$x]=3; $totpts+=3;}
   }

?>

<h4>CATEGORY #1: COMPETITIVE DEBATE (<?php echo $totpts ?> total points)
<a onclick="document.getElementById('table').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide</a>
<a onclick="document.getElementById('table').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show</a>
</h4>
       <table id="table" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Tourney</th>
		<th class="smallish">Event</th>
		<th class="smallish">Round</th>
		<th class="smallish">Outcome</th>
		<th class="smallish">Points</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php

$i=1;
while ($i < $x) {
 echo "<tr>";
 echo "<td>".$tourney[$i]."</td>";
 echo "<td>".$event[$i]."</td>";
 echo "<td>".$round[$i]."</td>";
 echo "<td>".$outcome[$i]."</td>";
 echo "<td>".$points[$i]."</td>";
 echo "</tr>";
$i++;
}
?>

	</tbody>
       </table>

<?php

//INDIVIDUAL EVENTS

$x=0; $totpts=0;
$query="SELECT *, event.name as event_name, tourn.name as tourn_name, round.label as round_name FROM student, entry_student, entry, event, tourn, ballot, panel, round, ballot_value WHERE student.id=".$studentid." and entry_student.student=student.id and entry.id=entry_student.entry and event.id=entry.event and tourn.id=event.tourn and ballot.entry=entry.id and panel.id=ballot.panel and round.id=panel.round and ballot_value.ballot=ballot.id and ballot_value.tag='rank' and event.type='Speech'";
$ballots=mysql_query($query); 
$entryNum = mysql_num_rows($ballots);
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
   {
         $x++;
         $tourney[$x]=$row['tourn_name'];
         $event[$x]=$row['event_name'];
         $round[$x]=$row['round_name'];
         $outcome[$x]=$row['value'];
         $points[$x]=2;
         if ($row['value']==1) {$points[$x]=6; $totpts+=6;}
         if ($row['value']==2) {$points[$x]=5; $totpts+=5;}
         if ($row['value']==3) {$points[$x]=4; $totpts+=4;}
         if ($row['value']==4) {$points[$x]=3; $totpts+=3;}
   }
?>

<h4>CATEGORY #2: COMPETITIVE SPEAKING (<?php echo $totpts ?> total points)
<a onclick="document.getElementById('IEtable').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide</a>
<a onclick="document.getElementById('IEtable').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show</a>
</h4>
       <table id="IEtable" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Tourney</th>
		<th class="smallish">Event</th>
		<th class="smallish">Round</th>
		<th class="smallish">Outcome</th>
		<th class="smallish">Points</th>
	</tr>
	</thead>

    <tbody id="myIETable">

<?php

$i=1;
while ($i < $x) {
 echo "<tr>";
 echo "<td>".$tourney[$i]."</td>";
 echo "<td>".$event[$i]."</td>";
 echo "<td>".$round[$i]."</td>";
 echo "<td>".$outcome[$i]."</td>";
 echo "<td>".$points[$i]."</td>";
 echo "</tr>";
$i++;
}
?>
	</tbody>
       </table>

<?php
$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n";

mysql_close();
require 'scripts/tabroomfooter.html';

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

?>