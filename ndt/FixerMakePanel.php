<?php
die;
require 'scripts/databaseconnect.php';

$round=$_POST['round'];
$panel=$_POST['panel'];
$entry1=$_POST['entry1'];
$entry2=$_POST['entry2'];

if ($round>0)
{
$query="INSERT INTO panel (round) VALUES (".$round.")";
echo $query."<br>";
mysql_query($query);

echo "CREATE PANEL QUERIES PROCESSED.<BR>";
}

if ($panel>0)
{
$query="INSERT INTO ballot (panel, entry) VALUES (".$panel.",".$entry1.")";
echo $query."<br>";
mysql_query($query);

$query="INSERT INTO ballot (panel, entry) VALUES (".$panel.",".$entry2.")";
echo $query."<br>";
mysql_query($query);

echo "CREATE PANEL QUERIES PROCESSED.<BR>";
}

?>

<br><hr>Complete the form below to create a blank panel for the round.  Use the bye fixer page to complete the bye.

<form action="FixerMakePanel.php" method="post">
 <p>Round: <input type="text" size="50" name="round"></input></p>
 <p><input type="submit" /></p>
</form>
<hr></br>
Complete the form below to add teams to the panel.  IF THE ROUND FIELD IS COMPLETED, A NEW PANEL WILL BE CREATED.
<form action="FixerMakePanel.php" method="post">
 <p>panel: <input type="text" size="50" name="panel"></input></p>
 <p>Entry 1: <input type="text" size="50" name="entry1"></input></p>
 <p>Entry 2: <input type="text" size="50" name="entry2"></input></p>
 <p><input type="submit" /></p>
</form>

<?php

mysql_close();

?>