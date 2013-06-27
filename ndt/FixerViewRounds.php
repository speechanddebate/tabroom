<?php
die;
require 'scripts/databaseconnect.php';
$event=$_GET['event'];
$query="SELECT * FROM round where round.event=".$event;
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
?>

       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">id</th>
		<th class="smallish">round.name</th>
		<th class="smallish">round.label</th>
		<th class="smallish">round.type</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"id")."</td>";
echo "<td>".mysql_result($tourn,$i,"name")."</td>";
echo "<td>".mysql_result($tourn,$i,"label")."</td>";
echo "<td>".mysql_result($tourn,$i,"type")."</td>";
echo "</tr>";
}

mysql_close();

?>

<a href="FixMaster.php">Fixer Main Page</a></br>