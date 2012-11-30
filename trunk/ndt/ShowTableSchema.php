<?php
$username="jbruschke";
$password="Oof9iyeeGh9jeeg";
$database="itab";

mysql_connect("localhost",$username,$password);
@mysql_select_db($database) or die( "Unable to select database<br>");

$result = mysql_query("SHOW COLUMNS FROM student");
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
echo "Field name stuff:";
//$query="SELECT *, ballot.id AS ballot_id FROM ballot, panel, round, judge where ballot.entry=341286 and panel.id=ballot.panel and round.id=panel.round and judge.id=ballot.judge";
$query="SELECT *, student.first as student_first from student, chapter where student.id=49928 and chapter.id=student.chapter";
$ballot=mysql_query($query);
$NumFields=mysql_num_fields($ballot);
echo "Total fields:".$NumFields."<br>";
for ($i=0; $i <= $NumFields-1; $i++)
{
echo "Field $i name:".mysql_field_name($ballot, $i)."<br>";
}

$females=0;
$query="SELECT * from student where last='pappas'";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

for ($i=0; $i <= $entryNum-1; $i++)
{
echo mysql_result($tourn,$i,"id")." ".mysql_result($tourn,$i,"first")." ".mysql_result($tourn,$i,"last")." chapter:".mysql_result($tourn,$i,"chapter")."<br>";
if (mysql_result($tourn,$i,"gender")=="F") {$females++;}
}
echo "Total judges:".$entryNum." females:".$females."<br>";

$query="SELECT * from INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);

for ($i=0; $i <= $entryNum-1; $i++)
{
echo mysql_result($tourn,$i,"table_name")."<br>";
}

mysql_close();
?>
