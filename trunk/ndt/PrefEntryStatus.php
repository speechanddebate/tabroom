<?php                    //This makes cume sheets
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

?>

<h2>Pref Entry Status</h2>
       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Team</th>
              <th class="smallish">Rated Judges</th>
              <th class="smallish">Total Judges</th>
	</tr>
	</thead>
    <tbody id="myTbodytotals">

<?php
$tourn=$_GET['tourn'];
$query="SELECT * from entry where tourn=".$tourn." order by school asc";
$entry=mysql_query($query);
$entrynum = mysql_num_rows($entry);
while ($row = mysql_fetch_array($entry, MYSQL_BOTH)) 
 { 
  echo "<tr>";
  echo "<td>".getfullteamname($row['id'])."</td>";
  echo "<td>".counter($row['id'])."</td>";
  echo "<td>".judgesbyevent($row['event'],$row['id'])."</td>";
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

function counter($entry)
{
 $query="SELECT * from rating where entry=".$entry;
 $rating=mysql_query($query);
 $num = mysql_num_rows($rating);
 return $num;
}

function judgesbyevent($event, $entry)
{
 $query="SELECT * from judge, event, entry where event.id=".$event." and judge.judge_group=event.judge_group and judge.chapter<>entry.chapter and entry.id=".$entry;
 $query="SELECT * from judge, event where event.id=".$event." and judge.judge_group=event.judge_group";
 //echo $query."</br>";
 $judge=mysql_query($query);
 $num = mysql_num_rows($judge);
 return $num;
}

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

?>
