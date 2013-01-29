<?php
require 'databaseconnect.php';

$query=$_POST['querystring'];
echo $query;
mysql_query($query);
echo "<br><br>done.";

mysql_close();
?>