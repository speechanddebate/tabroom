<?php
require 'scripts/databaseconnect.php';

$cbx=$_POST['sports'];
echo $cbx;

$tournnum=$_GET['tourn'];
$query="SELECT * FROM tourn, round, event, panel, ballot, ballot_value, entry WHERE tourn.id=".$tournnum." and event.tourn=tourn.id and round.event=event.id and panel.round=round.id and round.name>=10 and ballot.panel=panel.id and ballot_value.ballot=ballot.id and entry.id=ballot.entry ORDER BY panel.id asc";

$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
?>
ELIM BYE FIXER</br>
scrolls through all elims at a tournament and adds ballot_value scores to byes</br>
       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
              <th class="smallish">Round</th>
              <th class="smallish">Event</th>
		<th class="smallish">panel</th>
		<th class="smallish">ballot</th>
		<th class="smallish">entry</th>
		<th class="smallish">judge</th>
		<th class="smallish">ballot_value ID</th>
		<th class="smallish">ballot_value value</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"round.name")."-".mysql_result($tourn,$i,"round.label")."</td>";
echo "<td>".mysql_result($tourn,$i,"event.id")."-".mysql_result($tourn,$i,"event.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"panel.id")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot.id")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot.entry")."-".mysql_result($tourn,$i,"entry.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot.judge")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot_value.id")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot_value.value")."</td>";
//echo "<td>".mysql_result($tourn,$i,"value")."</td>";
//echo "<td>".mysql_result($tourn,$i,"content")."</td>";
echo "</tr>";
}

?>

    </tbody></table>

<form action="ElimByeFixer.php<?php echo "?tourn=".$tournnum; ?>" method="post">
 Blue squid? <input type="checkbox" name="sports" value="soccer"  /><br />
 <p><input type="submit" /></p>
</form>

<?php

mysql_close();

?>