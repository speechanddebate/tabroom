<?php
require 'scripts/databaseconnect.php';
$panel=$_GET['panel'];
$query="SELECT * FROM ballot where ballot.panel=".$panel;
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
?>

       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">ballot</th>
		<th class="smallish">entry</th>
		<th class="smallish">judge</th>
		<th class="smallish">side</th>
		<th class="smallish">ballot_value ID/decision</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"id")."</td>";
echo "<td>".mysql_result($tourn,$i,"entry")."</td>";
echo "<td>".mysql_result($tourn,$i,"judge")."</td>";
echo "<td>".mysql_result($tourn,$i,"side")."</td>";
echo "<td>".getdecision(mysql_result($tourn,$i,"id"))."</td>";
echo "</tr>";
}

mysql_close();

function getdecision($ballot)
{
 $judgename="N/A";
 $query="SELECT * FROM ballot_value WHERE ballot_value.ballot=".$ballot." and tag='ballot'";
 $judges=mysql_query($query); 
   while ($row = mysql_fetch_array($judges, MYSQL_BOTH)) 
   {$judgename=$row['id']."-".$row['value'];}
 return $judgename;
}

?>

<a href="FixerElimByeAutofixer.php">Elim Bye Main Page</a></br>

<a href="FixMaster.php">Fixer Main Page</a></br>