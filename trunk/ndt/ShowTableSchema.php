<?php
$username="jbruschke";
$password="Oof9iyeeGh9jeeg";
$database="itab";
echo $_SESSION['account']."<br>";

mysql_connect("localhost",$username,$password);
@mysql_select_db($database) or die( "Unable to select database<br>");

$result = mysql_query("SHOW COLUMNS FROM ballot_value");
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

$query="SELECT distinct tag from ballot_value order by tag";
//$query="SELECT * from chapter, chapter_circuit where chapter_circuit.chapter=chapter.id and chapter_circuit.circuit=43";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

for ($i=0; $i <= $entryNum-1; $i++)
{
echo "tag=".mysql_result($tourn,$i,"tag")."<br>";
//echo mysql_result($tourn,$i,"id")." ".mysql_result($tourn,$i,"tag")." ".mysql_result($tourn,$i,"name")."<br>";

}

$query="SELECT * from INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

for ($i=0; $i <= $entryNum-1; $i++)
{
echo mysql_result($tourn,$i,"table_name")."<br>";
}

echo "<br>";

$sql = mysql_query("SELECT * FROM entry where id=394384");
$assoc = mysql_fetch_assoc($sql);
var_dump($assoc);

mysql_close();
?>
