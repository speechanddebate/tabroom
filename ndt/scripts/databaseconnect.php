<?php
	$username="tabroom";
	$password=":UGMQJcD{u=3[CiM$@JNjy|N";
	$database="tabroom";
	$host="localhost";
	mysql_connect($host,$username,$password);
	@mysql_select_db($database) or die( "Unable to select database<br>");
?>
