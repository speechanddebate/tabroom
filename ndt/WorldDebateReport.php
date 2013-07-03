<?php
$username="jbruschke";
$password="Oof9iyeeGh9jeeg";
$database="itab";

mysql_connect("localhost",$username,$password);
@mysql_select_db($database) or die( "Unable to select database<br>");

$query="SELECT *, count(entry.id) as nEntries from tourn, event, entry where entry.dropped=0 and event.tourn=tourn.id and entry.event=event.id and start BETWEEN '2012-08-01' AND '2013-08-01' group by event.id order by start asc, tourn.id asc";
$tourn=mysql_query($query);

$entryNum = mysql_num_rows($tourn);
echo "<?xml version ='1.0'?>";
echo "<tabroom>";
for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<event>";
echo " <tourn>".mysql_result($tourn,$i,"id")."</tourn>"; 
$tname=str_replace("/", "", mysql_result($tourn,$i,"name"));
$tname=str_replace("&", " ", mysql_result($tourn,$i,"name"));
echo " <name>".$tname."</name>";
echo " <type>".mysql_result($tourn,$i,"event.type")."</type>";
echo " <entries>".mysql_result($tourn,$i,"nEntries")."</entries>";
echo " <start>".mysql_result($tourn,$i,"start")."</start>";
echo " <state>".mysql_result($tourn,$i,"state")."</state>";
echo " <country>".mysql_result($tourn,$i,"country")."</country>";

$query2="SELECT entry_student.id from entry_student, entry where entry.dropped=0 and entry_student.entry=entry.id and entry.event=".mysql_result($tourn,$i,"event.id");
$tourn2=mysql_query($query2);
echo " <ncompetitors>".mysql_num_rows($tourn2)."</ncompetitors>";

echo "</event>";
}

echo "</tabroom>";

mysql_close();
?>
