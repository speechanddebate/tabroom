<?php                    //This makes cume sheets
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

//Load all students
$query="SELECT *, student.id as student_id FROM student, chapter_circuit where student.chapter=chapter_circuit.chapter and chapter_circuit.circuit=43 ORDER BY student.id";
$student=mysql_query($query);
$studentnum = mysql_num_rows($student);

//Mark participation
$nrounds=array(); $ntourn=array(); $chapter=array();
while ($row = mysql_fetch_array($student, MYSQL_BOTH)) {countstuff($row['student_id'], $nrounds, $ntourn); $chapter[$row['student_id']]=$row['chapter'];}

?>
<h2>Participation by Student</h2>
       <table id="thing" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Measure</th>
		<th class="smallish">count</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php
echo "<tr><td>Total student accounts</td><td>".$studentnum."</td></tr>";
echo "<tr><td>Students with 1+ round</td><td>".counter(1, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students with 5+ round</td><td>".counter(5, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students with 10+ round</td><td>".counter(10, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students with 25+ round</td><td>".counter(25, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students with 50+ round</td><td>".counter(50, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students with 75+ round</td><td>".counter(75, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students with 100+ round</td><td>".counter(100, $nrounds, -99, $chapter)."</td></tr>";
echo "<tr><td>Students attending 1+ tournaments</td><td>".counter(1, $ntourn, -99, $chapter)."</td></tr>";
echo "<tr><td>Students attending 3+ tournaments</td><td>".counter(3, $ntourn, -99, $chapter)."</td></tr>";
echo "<tr><td>Students attending 5+ tournaments</td><td>".counter(5, $ntourn, -99, $chapter)."</td></tr>";
echo "<tr><td>Students attending 8+ tournaments</td><td>".counter(8, $ntourn, -99, $chapter)."</td></tr>";
echo "<tr><td>Students attending 10+ tournaments</td><td>".counter(10, $ntourn, -99, $chapter)."</td></tr>";
echo "<tr><td>Students attending 12+ tournaments</td><td>".counter(12, $ntourn, -99, $chapter)."</td></tr>";


?>
	</tbody>
       </table>

<?php
//// CLOSE UP AND FUNCTIONS

mysql_close();
$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n";
require 'scripts/tabroomfooter.html';

function counter($target, $array, $targetchapter, $chapter)
{
 $ctr=0;
 foreach ($array as $index_name => $value) 
 {
  if ($value>=$target and $targetchapter==-99) {$ctr++;}
 }
 return $ctr;
}

function countstuff($student, &$nrounds, &$ntourn)
{
$query="SELECT *, event.tourn as event_tourn, panel.round as panel_round, panel.id as panel_id, entry_student.student as entry_student_id FROM ballot, panel, entry, entry_student, round, event WHERE entry_student.student=".$student." and entry.id=entry_student.entry and ballot.entry=entry.id and panel.id=ballot.panel and round.id=panel.round and event.id=round.event ORDER BY event.tourn, round.id";
$results=mysql_query($query);
$tourn=-99;$round=-99;$ntourneys=0; $nrd=0;
while ($row = mysql_fetch_array($results, MYSQL_BOTH)) 
 {
 //echo "student=".$row['entry_student_id']." panel=".$row['panel_id']." round=".$row['panel_round']." tourn=".$row['event_tourn']."<br>";
 if ($row['event_tourn']<>$tourn) {$ntourneys++;}
 if ($row['panel_round']<>$round) {$nrd++;}
 $tourn=$row['event_tourn'];
 $round=$row['panel_round'];
 }
 $nrounds[$student]=$nrd;
 $ntourn[$student]=$ntourneys;
}

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

?>