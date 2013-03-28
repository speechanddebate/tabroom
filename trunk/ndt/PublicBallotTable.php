<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$round=$_GET['RoundID'];

?>

<!--
<form action="PublicBallotTable.php" method="get">
  Round ID: <input type="text" size="30" name="RoundID" />
  <input type="submit" value="Show Round"/>
</form>
-->

       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Room</th>
		<th class="smallish">Started</th>
		<th class="smallish">Ballots Out</th>
		<th class="smallish">Ballots In</th>
	</tr>
	</thead>

<?php
$query="SELECT * FROM panel, room, round WHERE round.id=panel.round and room.id=panel.room and panel.round=".$round." ORDER BY Started ASC";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
$stillout=0; $alreadyin=0;

echo "<b><center>ROUND:".mysql_result($tourn,0,"round.label")."</b></center>";

for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"room.name")."</td>";
ballotsout(mysql_result($tourn,$i,"panel.id"), $stillout, $alreadyin);
echo "<td>".$stillout."</td>";
echo "<td>".$alreadyin."</td>";
echo "<td>".mysql_result($tourn,$i,"panel.started")."</td>";
echo "</tr>";
}

?>

    </tbody></table>

<? 
function ballotsout($panel, &$stillout, &$alreadyin)
{
$query="SELECT distinct judge FROM ballot where ballot.panel=".$panel;
$tourn=mysql_query($query);
$Nballots = mysql_num_rows($tourn);

$query="SELECT * FROM ballot, ballot_value WHERE ballot_value.ballot=ballot.id and ballot_value.tag='ballot' and ballot_value.value=1 and ballot.panel=".$panel;
//echo $query;
$tourn=mysql_query($query);
$alreadyin = mysql_num_rows($tourn);

$stillout=$Nballots-$alreadyin; 
}

mysql_close(); 
?>