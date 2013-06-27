<?php
die;
require 'scripts/databaseconnect.php';

$ballot=$_GET['ballot'];
$query="SELECT * FROM ballot_value WHERE ballot=".$ballot;

$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
?>
UGLY BALLOT EDITOR</br>
Receives a ballotID and shows all values associated with it</br>
       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">ID</th>
		<th class="smallish">ballot</th>
		<th class="smallish">tag</th>
		<th class="smallish">student</th>
		<th class="smallish">tiebreak</th>
		<th class="smallish">value</th>
		<th class="smallish">content</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">


<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"id")."</td>";
echo "<td>".mysql_result($tourn,$i,"ballot")."</td>";
echo "<td>".mysql_result($tourn,$i,"tag")."</td>";
echo "<td>".mysql_result($tourn,$i,"student")."</td>";
echo "<td>".mysql_result($tourn,$i,"tiebreak")."</td>";
echo "<td>".mysql_result($tourn,$i,"value")."</td>";
echo "<td>".mysql_result($tourn,$i,"content")."</td>";
echo "</tr>";
}

?>

    </tbody></table>

<form action="DoSQLUpdate.php" method="post">
 <p>Enter the SQL query you wish to run: <input type="text" size="250" name="querystring" value="Update ballot_value set value=1 where ballot_value.id=1156828"></input></p>
 <p><input type="submit" /></p>
</form>

<?php

mysql_close();

?>