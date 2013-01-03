<?php                    //This makes cume sheets
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

//Load all students
$query="SELECT *, student.id as student_id, student.chapter as student_chapter FROM student, chapter_circuit where student.chapter=chapter_circuit.chapter and chapter_circuit.circuit=43 and retired=FALSE ORDER BY student.id";
$student=mysql_query($query);
$studentnum = mysql_num_rows($student);
//echo "LOAD<BR>";
//while ($row = mysql_fetch_array($student, MYSQL_BOTH)) {if ($row['student_chapter']==6439) {echo $row['student_id']." ".$row['first']." ".$row['last']." ".$row['student_chapter']."<br>";}}
//echo "END LOAD<BR>";


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
echo "<tr><td>Total non-retired student records</td><td>".$studentnum."</td></tr>";
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

//CLOSE UP AND SWITCH TO SCHOOLS	
?>
	</tbody>
       </table>
<br/>
<h2>Participation by School</h2>
       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">School</th>
              <th class="smallish">Competitors</th>
              <th class="smallish">Comp. at 1+ tourneys</th>
              <th class="smallish">Comp. at 3+ tourneys</th>
              <th class="smallish">Comp. at 5+ tourneys</th>
              <th class="smallish">Comp. at 10+ tourneys</th>
	</tr>
	</thead>
    <tbody id="myTbodytotals">

<?php
//Load all SCHOOLS
$query="SELECT *, chapter.name as chapter_name, chapter.id as chapter_id FROM chapter_circuit, chapter where chapter_circuit.circuit=43 and chapter.id=chapter_circuit.chapter order by chapter.name";
$school=mysql_query($query);
while ($row = mysql_fetch_array($school, MYSQL_BOTH)) 
 { 
  echo "<tr>";
  echo "<td>".$row['chapter_name']."</td>";
  echo "<td>".countcompbyschool($row['chapter_id'], $student)."</td>";
  echo "<td>".counter(1, $ntourn, $row['chapter_id'], $chapter)."</td>";
  echo "<td>".counter(3, $ntourn, $row['chapter_id'], $chapter)."</td>";
  echo "<td>".counter(5, $ntourn, $row['chapter_id'], $chapter)."</td>";
  echo "<td>".counter(10, $ntourn, $row['chapter_id'], $chapter)."</td>";
  echo "</tr>";
 }
?>
	</tbody>
       </table>

<br/>
<h2>Participation by Tournament</h2>
       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Tournament</th>
              <th class="smallish">Open Entries</th>
              <th class="smallish">JV Entries</th>
              <th class="smallish">Novice Entries</th>
              <th class="smallish">Total Entries</th>
	</tr>
	</thead>
    <tbody id="myTourney">

<?php
//Load all TOURNEYS
$query="SELECT *, tourn.name as tourn_name, tourn.id as tourn_id FROM tourn, tourn_circuit where tourn_circuit.circuit=43 and tourn.id=tourn_circuit.tourn order by tourn.name";
$tourns=mysql_query($query);
while ($row = mysql_fetch_array($tourns, MYSQL_BOTH)) 
 { 
  echo "<tr>";
  echo "<td>".$row['tourn_name']."</td>";
  $open=getnentries($row['tourn_id'],'open'); echo "<td>".$open."</td>";
  $jv=getnentries($row['tourn_id'],'jv'); echo "<td>".$jv."</td>";
  $novice=getnentries($row['tourn_id'],'novice'); echo "<td>".$novice."</td>";
  $total=$open+$jv+$novice;
  echo "<td>".$total."</td>";
  echo "</tr>";

 }

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

function getnentries($tourn_id, $division)
{
$query="SELECT * FROM entry, event, event_setting WHERE event.tourn=".$tourn_id." and entry.event=event.id and event.type='policy' and event_setting.event=event.id and event_setting.tag='Level' and event_setting.value='".$division."'";
$results=mysql_query($query);
$nentries = mysql_num_rows($results);
return $nentries;
}

function counter($target, $array, $targetchapter, $chapter)
{
 $ctr=0;
 foreach ($array as $index_name => $value) 
 {
  if ($value>=$target and $targetchapter==-99) {$ctr++;}
  if ($value>=$target and $targetchapter==$chapter[$index_name]) {$ctr++;}
 }
 return $ctr;
}

function countcompbyschool($chapter, $student)
{
 $ctr=0;
 mysql_data_seek($student,0);
 while ($row = mysql_fetch_array($student, MYSQL_BOTH))
  {if ($row['student_chapter']==$chapter) {$ctr++;}
   //if ($row['student_chapter']==$chapter and $chapter==6306) {echo $row['first']." ".$row['last']." ".$row['student_chapter']."<br>";}
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