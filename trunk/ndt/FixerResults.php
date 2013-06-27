<?php
die;
require 'scripts/databaseconnect.php';

$ballotid=$_POST['ballotid'];
$decision=$_POST['decision'];
$query="INSERT INTO ballot_value (tag, ballot, value) VALUES ('ballot', ".$ballotid.", ".$decision.")";
mysql_query($query);
echo $query;
echo "<br><br>done."
?>

CREATE A DECISION AND LINK IT TO A BALLOT -- for elim ballots that don't have any ballot_value linked to them.

<form action="FixerResults.php" method="post">
 <p>Ballot: <input type="text" size="50" name="ballotid"></input></p>
 <p>decision: <input type="text" size="50" name="decision"></input></p>
 <p><input type="submit" /></p>
</form>

<?php

mysql_close();

?>