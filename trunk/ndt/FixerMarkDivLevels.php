<?php
require 'scripts/databaseconnect.php';

//INSERT INTO event_setting (tag, event, value) VALUES ('level', 18947,"novice")
$event=$_GET['event'];
$query="SELECT * FROM tourn, tourn_circuit, event where tourn_circuit.circuit=43 and tourn.id=tourn_circuit.tourn and event.tourn=tourn.id";

$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
?>

       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">tourn</th>
		<th class="smallish">Event</th>
		<th class="smallish">Level</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"tourn.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"event.id")."-".mysql_result($tourn,$i,"event.name")."</td>";
echo "<td>".getlevel(mysql_result($tourn,$i,"event.id"))."</td>";
echo "</tr>";
}

?>

    </tbody></table>
<br>
<form action="scripts/DoSQLUpdate.php" method="post">
 Enter the SQL query you wish to run: <input type="text" size="50" name="querystring"/>
 <input type="submit" value="Run query"/>
</form>
<hr>

<?php

mysql_close();

function getlevel($event)
{
   $schoolname="";
   $query="SELECT * FROM event_setting WHERE event_setting.event=".$event." and event_setting.tag='level'";
   $school=mysql_query($query);
   while ($row = mysql_fetch_array($school, MYSQL_BOTH)) 
     {$schoolname=$row['value'];}
    return $schoolname;
}


?>