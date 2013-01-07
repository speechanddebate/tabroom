<?php
$username="jbruschke";
$password="Oof9iyeeGh9jeeg";
$database="itab";

mysql_connect("localhost",$username,$password);
@mysql_select_db($database) or die( "Unable to select database<br>");

$result = mysql_query("SHOW COLUMNS FROM result");
if (!$result) {
    echo 'Could not run query: ' . mysql_error();
    exit;
}
if (mysql_num_rows($result) > 0) {
    while ($row = mysql_fetch_assoc($result)) {
        print_r($row)."<br><br>";
    }
}

echo "<br>";

$query="SELECT distinct tag from event_setting order by tag";
$query="SELECT * from student where last='Carter'";
$query="SELECT distinct type from result";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

for ($i=0; $i <= $entryNum-1; $i++)
{
echo mysql_result($tourn,$i,"type")."<br>";
//echo mysql_result($tourn,$i,"id")." ".mysql_result($tourn,$i,"first")." ".mysql_result($tourn,$i,"last")."<br>";

}

$query="SELECT * from INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

for ($i=0; $i <= $entryNum-1; $i++)
{
echo mysql_result($tourn,$i,"table_name")."<br>";
}

echo "<br>";

$sql = mysql_query("SELECT * FROM student WHERE last = 'Lurie-Spicer'");
$assoc = mysql_fetch_assoc($sql);
var_dump($assoc);

mysql_close();
?>
