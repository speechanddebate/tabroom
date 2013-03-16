
<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';
?>

<body>

<?php                    
$time_start = microtime(true);

//Load judges at tourney
$query="SELECT * FROM judge, school, judge_group WHERE school.id=judge.school and judge_group.id=judge.judge_group and judge_group.tourn=".$_GET['tourn'];
$judgelist=mysql_query($query);
$entryNum = mysql_num_rows($judgelist);

echo "<h2>JUDGING RECORDS</h2><br>";
echo "Clicking the link the philosophy column will launch the full judge record.<br>";
?>

<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
<thead>
<tr class="yellowrow">
 <th class="smallish">Judge</th>
 <th class="smallish">School</th>
 <th class="smallish">Open Rounds</th>
 <th class="smallish">Total Rounds</th>
 <th class="smallish">Philosophy</th>
</tr>
</htead>
<tbody>

<?php
for ($i=0; $i <= $entryNum-1; $i++)
{
 echo "<tr><td>".mysql_result($judgelist,$i,"first")." ".mysql_result($judgelist,$i,"last")."</td>";
 echo "<td>".mysql_result($judgelist,$i,"school.name")."</td>";
 $openrds=0; $totrds=0; $paradigm=""; RoundCount(mysql_result($judgelist,$i,"judge.account"), $openrds, $totrds, $paradigm);
 echo "<td>".$openrds."</td>";
 echo "<td>".$totrds."</td>";
 getparadigm(mysql_result($judgelist,$i,"judge.account"), $paradigm);
 if ($paradigm<>"") {echo "<td><a href='https://www.tabroom.com/jbruschke/JudgeRecord.php?judgeid=".mysql_result($judgelist,$i,"judge.account")."'>Entered</a></td>";}
 if ($paradigm=="") {echo "<td><a href='https://www.tabroom.com/jbruschke/JudgeRecord.php?judgeid=".mysql_result($judgelist,$i,"judge.account")."'>None</a></td>";}
 echo "</tr>";
}

?>
</table>

<?php
function getparadigm($account, &$paradigm)
{
$paradigm="";
$query="SELECT * FROM account WHERE id=".$account;
$judge=mysql_query($query);
$entryNum = mysql_num_rows($judge);
if ($entryNum==0) {return;}
$paradigm=mysql_result($judge,0,"paradigm");
}

function RoundCount ($account, &$openrds, &$totrds)
{
if ($account==0) {$openrds="No account"; return;}
$query="SELECT * FROM judge WHERE judge.account=".$account;
$judge=mysql_query($query);
$entryNum = mysql_num_rows($judge);
$openrds=0; $totrds=0;

$i=0; 
while ($i < $entryNum) 
 {
 $query="SELECT Distinct Panel FROM ballot where judge=".mysql_result($judge,$i,"judge.id");
 $rounds=mysql_query($query);
 $totrds = $totrds + mysql_num_rows($rounds);

 $query="SELECT distinct panel FROM ballot, panel, round, event, event_setting where event_setting.event=event.id and event_setting.tag='level' and event_setting.value='open' and panel.id=ballot.panel and round.id=panel.round and event.id=round.event and ballot.judge=".mysql_result($judge,$i,"judge.id");
 $rounds=mysql_query($query);
 $openrds = $openrds + mysql_num_rows($rounds);

 $i++;
 }
}

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n";

require 'scripts/tabroomfooter.html';
mysql_close();
?>