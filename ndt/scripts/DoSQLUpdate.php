<?php
require 'databaseconnect.php';

$query=$_POST['querystring'];
echo $query;
mysql_query($query);
echo "done.";

mysql_close();
?>