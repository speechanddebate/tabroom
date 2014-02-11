<?php
#$username="jbruschke";
#$password="Oof9iyeeGh9jeeg";
#$database="itab";
#$host="localhost";
	$username="tabroom";
	$password="Ev34yGYLMPGYP3Y4";
	$database="itab";
	$host="tabroom.coha3i31zrbw.us-east-1.rds.amazonaws.com"

	mysql_connect($host,$username,$password);
	@mysql_select_db($database) or die( "Unable to select database<br>");
?>

