<?php
require 'scripts/databaseconnect.php';

$query="SELECT * FROM tourn, tourn_circuit, event, round where tourn_circuit.circuit=43 and tourn.id=tourn_circuit.tourn and event.tourn=tourn.id and event.type='policy' and round.event=event.id";
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
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"tourn.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"event.id")."-".mysql_result($tourn,$i,"event.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"round.id")."-".mysql_result($tourn,$i,"round.label")."</td>";
echo "<td>".mysql_result($tourn,$i,"round.name")."</td>";
echo "</tr>";
}

?>

    </tbody></table>

<br>
<hr>

<?php

mysql_close();

?>