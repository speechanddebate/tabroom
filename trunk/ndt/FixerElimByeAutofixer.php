<?php
// 1. Pull all elim panels for circuit 43
// 2. Find all panels with fewer than 3 judges, (a) make 3 judges (b) create a decision for all new ballots in favor of the team

require 'scripts/databaseconnect.php';

$query="SELECT * FROM tourn, tourn_circuit, event, round, panel where tourn_circuit.circuit=43 and tourn.id=tourn_circuit.tourn and event.tourn=tourn.id and event.type='policy' and round.event=event.id and round.name>=10 and panel.round=round.id";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

?>

       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">tourn</th>
		<th class="smallish">Event</th>
		<th class="smallish">round.label</th>
		<th class="smallish">round.name</th>
		<th class="smallish">panel</th>
		<th class="smallish">ballots</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"tourn.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"event.id")."-".mysql_result($tourn,$i,"event.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"round.label")."</td>";
echo "<td>".mysql_result($tourn,$i,"round.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"panel.id")."</td>";
echo "<td>".ballotsbypanel(mysql_result($tourn,$i,"panel.id"))."</td>";
echo "</tr>";
}

?>

    </tbody></table>

<br>
<hr>
Run a custom query
<form action="scripts/DoSQLUpdate.php" method="post">
 Enter the SQL query you wish to run: <input type="text" size="50" name="querystring" />
 <input type="submit" value="Run query"/>
</form>
<hr>
For panels with ZERO ballots:
<form action="scripts/AddTeamToPanel.php" method="post">
 Panel: <input type="text" size="30" name="panel" />
 Team: <input type="text" size="30" name="entry" />
 <input type="submit" value="Add Team to Blank Panel and give them a bye"/>
</form>
<hr>
Audit the panel to make sure there are 3 ballots and all have ballot_values associated with them
<form action="scripts/Make3.php" method="post">
  Panel: <input type="text" size="30" name="panel" />
  <input type="submit" value="Audit a panel with a bye"/>
</form>
<hr>
Take a bye that credits only 1 ballot and credit the team with all ballots; must already have 3 ballots for the entry.  Only works if ballot_values don't exist
<form action="scripts/FillOutWinner.php" method="post">
  Panel: <input type="text" size="30" name="panel" />
  Entry: <input type="text" size="30" name="entry" />
  <input type="submit" value="Set all ballots to win for this team"/>
<hr>
Takes a panel and a team, deletes all existing info for that panel and gives the team a 3-0 decision<br>
<form action="scripts/Give30Win.php" method="post">
  Panel: <input type="text" size="30" name="panel" /><br>
  Entry/winner: <input type="text" size="30" name="entry" /><br>
  Is a bye (0 for judge unknown, -1 for bye): <input type="text" size="30" name="isbye" /><br>
  <input type="submit" value="Give a 3-0 win"/>
</form>



<?php

mysql_close();

function ballotsbypanel($panel)
{
$query="SELECT * FROM ballot WHERE ballot.panel=".$panel;
$tourn=mysql_query($query);
$ballotNum = mysql_num_rows($tourn);
return $ballotNum;
}
?>