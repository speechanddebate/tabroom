<?php                    //This makes cume sheets
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

?>
<h2>Participation by Student</h2>
       <table id="sortmealso" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Measure</th>
		<th class="smallish">count</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php
echo "<tr><td>Total accounts</td><td>".counter("Select * from account")."</td></tr>";
echo "<tr><td>Different countries</td><td>".counter("Select distinct country from account")."</td></tr>";
echo "<tr><td>Different states</td><td>".counter("Select distinct state from account")."</td></tr>";
echo "<tr><td>Total chapters</td><td>".counter("Select * from chapter")."</td></tr>";
echo "<tr><td>Total non-retired student records</td><td>".counter("Select * from student WHERE retired=false")."</td></tr>";
echo "<tr><td>Students attending 1+ tournaments</td><td>".counter("Select distinct student from entry, entry_student, student where retired=false and entry_student.student=student.id and entry.id=entry_student.entry and entry.dropped=false")."</td></tr>";
echo "<tr><td>Total judge entries marked active</td><td>".counter("Select distinct id from judge where active=true")."</td></tr>";
echo "<tr><td>Total active judge entries with accounts</td><td>".counter("Select distinct account from judge where active=true")."</td></tr>";
echo "<tr><td>Total hosted tournaments</td><td>".counter("Select * from tourn")."</td></tr>";
echo "<tr><td>Total hosted tournaments since 9/1/2012</td><td>".counter("Select * from tourn where start>'2012-09-01'")."</td></tr>";

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

function counter($query)
{
$student=mysql_query($query);
$studentnum = mysql_num_rows($student);
return $studentnum;

}

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

?>
