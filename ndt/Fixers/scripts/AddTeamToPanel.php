<?php

require 'databaseconnect.php';

$panel=$_POST['panel'];
$entry=$_POST['entry'];
$query="INSERT INTO ballot (panel, entry, side) VALUES (".$panel.",".$entry.",1)";
//echo $query."<br>";
mysql_query($query);

mysql_close();

header('Location: https://www.tabroom.com/jbruschke/FixerElimByeAutofixer.php?event='.$event);

?>