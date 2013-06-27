<?php
die;
require 'scripts/databaseconnect.php';

$event=$_GET['event'];
$query="SELECT *, round.id as round_id, judge.first as judge_first, judge.last as judge_last, entry.code as entry_code, entry.name as entry_name, ballot.entry as ballot_entry, ballot.judge as ballot_judge, ballot.id as ballot_id, round.label as round_label, round.name as round_name, round.type as round_type, round.published as round_published, panel.id as panel_id FROM ballot, panel, round, entry, judge WHERE round.event=".$event." and panel.round=round.id and ballot.panel=panel.id and entry.id=ballot.entry and judge.id=ballot.judge";
$query="SELECT *, round.id as round_id, entry.code as entry_code, entry.name as entry_name, ballot.entry as ballot_entry, ballot.judge as ballot_judge, ballot.id as ballot_id, round.label as round_label, round.name as round_name, round.type as round_type, round.published as round_published, panel.id as panel_id FROM ballot, panel, round, entry WHERE round.event=".$event." and panel.round=round.id and ballot.panel=panel.id and entry.id=ballot.entry";

$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
?>

       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Round label</th>
		<th class="smallish">Round name</th>
		<th class="smallish">Round type</th>
		<th class="smallish">Round published</th>
		<th class="smallish">Panel</th>
		<th class="smallish">Ballot</th>
		<th class="smallish">Judge</th>
		<th class="smallish">Team</th>
		<th class="smallish">Result</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"round_id")."-".mysql_result($tourn,$i,"round_label")."</td>";
echo "<td>".mysql_result($tourn,$i,"round_name")."</td>";
echo "<td>".mysql_result($tourn,$i,"round_type")."</td>";
echo "<td>".mysql_result($tourn,$i,"round_published")."</td>";
echo "<td>".mysql_result($tourn,$i,"panel_id")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot_id")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot_judge")."-".getjudgename(mysql_result($tourn,$i,"ballot_judge"))."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot_entry")."-".mysql_result($tourn,$i,"entry_code")."-".mysql_result($tourn,$i,"entry_name")."</td>";
echo "<td>Win</td>";
echo "</tr>";
}

?>

    </tbody></table>
<br>
<form action="scripts/DoSQLUpdate.php" method="post">
 Enter the SQL query you wish to run: <input type="text" size="50" name="querystring" />
 <input type="submit" value="Run query"/>
</form>
<hr>
<form action="scripts/DecisionFlipper.php" method="post">
 Panel: <input type="text" size="30" name="panel" />
 Judge: <input type="text" size="30" name="judge" />
 Event: <input type="text" size="30" name="event" value="<?php echo $event; ?>" />
 <input type="submit" value="Flip Decision"/>
</form>
<hr>
<form action="scripts/SideFlipper.php" method="post">
  Panel: <input type="text" size="30" name="panel" />
  Event: <input type="text" size="30" name="event" value="<?php echo $event; ?>" />
  <input type="submit" value="Flip Side"/>
</form>



<?php

mysql_close();

function getjudgename($judgeID)
{
 $judgename="N/A";
 $query="SELECT * FROM judge where judge.id=".$judgeID;
 $judges=mysql_query($query); 
   while ($row = mysql_fetch_array($judges, MYSQL_BOTH)) 
   {$judgename=$row['first']." ".$row['last'];}
 return $judgename;
}

?>