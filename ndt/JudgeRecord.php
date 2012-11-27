
<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';
?>

<body>

<?php                    // this does judge records
$time_start = microtime(true);

//Load judge record; 6413 is murillo, 6681 is albiniak
$query="SELECT * FROM account WHERE id=".$_GET['judgeid'];
$judge=mysql_query($query);
$entryNum = mysql_num_rows($judge);

//loop, although the length is only one, and print the name and philosophy
$i=0;
while ($i < $entryNum) 
 {
//echo mysql_result($judge,$i,"first")." ".mysql_result($judge,$i,"last")."<br>PHILOSOPHY/PARADIGM<br> ".mysql_result($judge,$i,"paradigm");
$paradigm=mysql_result($judge,$i,"paradigm");

 echo "<h2>JUDGING RECORD FOR ".mysql_result($judge,$i,"first")." ".mysql_result($judge,$i,"last")."</h2><br>";

 $i++;
 }

?>

<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
<thead>
<tr class="yellowrow">
 <th class="smallish">Tournament</th>
 <th class="smallish">Division</th>
 <th class="smallish">Round</th>
 <th class="smallish">Aff</th>
 <th class="smallish">Aff Decision</th>
 <th class="smallish">Neg</th>
 <th class="smallish">Neg Decision</th>
 <th class="smallish">Panel Decision</th>
</tr>
</htead>
<tbody>

<?php
//Load all judge records using this account
$query="SELECT * FROM judge WHERE judge.account=".$_GET['judgeid'];
$judge=mysql_query($query);
$entryNum = mysql_num_rows($judge);

//loop, although the length is only one, and print the name and philosophy
$i=0; $panel=0;
while ($i < $entryNum) 
{
   $query="SELECT *, event.name as event_name, tourn.name as tourn_name, panel.id as panel_id, ballot_value.value as ballot_decision FROM ballot, panel, round, tourn, event, entry, ballot_value WHERE judge=".mysql_result($judge,$i,"id")." and published=1 and panel.id=ballot.panel and round.id=panel.round and event.id=round.event and tourn.id=event.tourn and entry.id=ballot.entry and ballot.id=ballot_value.ballot and ballot_value.tag='ballot' ORDER BY panel.id, ballot.id asc";
   $ballot=mysql_query($query);
   while ($row = mysql_fetch_array($ballot, MYSQL_BOTH)) 
    {
    if ($panel>0 and $panel<>$row['panel_id']) {printrow($tourn, $event, $round, $aff, $affdec, $neg, $negdec, $panel);}
    $tourn=$row['tourn_name'];
    $event=$row['event_name'];
    $round=$row['label'];
    if ($row['side']==1) {$aff=$row['name']; $affdec=$row['ballot_decision'];}
    if ($row['side']==2) {$neg=$row['name']; $negdec=$row['ballot_decision'];}
    $panel=$row['panel_id'];
    }
    if ($panel>0) {printrow($tourn, $event, $round, $aff, $affdec, $neg, $negdec, $panel);} //print the last row since it hasn't been printed yet
    $panel=0;
$i++;
}

mysql_close();
?>

</tbody>
</table>

<br><h2>paradigm</h2>
<div class="paradigm">
<?php echo $paradigm; ?>
</div>
</body>
</html>

<?php
function printrow ($tourn, $event, $round, $aff, $affdec, $neg, $negdec, $panel)
{
 if ($affdec==1) {$affdec="Win";} else {$affdec="Loss";}
 if ($negdec==1) {$negdec="Win";} else {$negdec="Loss";}
 echo "<tr><td>".$tourn."</td>";
 echo "<td>".$event."</td>";
 echo "<td>".$round."</td>";
 echo "<td>".$aff."</td>";
 echo "<td>".$affdec."</td>";
 echo "<td>".$neg."</td>";
 echo "<td>".$negdec."</td>";
 echo "<td>".getpaneldecision($panel)."</td>";
 echo "</tr>";
}

function getpaneldecision($panel)
{
   $decision="";
   $query="SELECT *, ballot_value.value as ballot_decision FROM ballot, panel, ballot_value WHERE panel.id=".$panel." and ballot.panel=panel.id and ballot.id=ballot_value.ballot and ballot_value.tag='ballot'";
   $ballots=mysql_query($query);
   $entryNum = mysql_num_rows($ballots);
   if ($entryNum>2)
   {
    while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
    {
    if ($row['side']==1 and $row['ballot_decision']==1) {$decision.="Aff ";}
    if ($row['side']==2 and $row['ballot_decision']==1) {$decision.="Neg ";}
    }
   }
   
 return $decision;
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

?>