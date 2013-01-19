<?php                    //This makes cume sheets
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

//Ultimately, need to assign values to these variables
$Round = ""; $Judge = ""; $Oppn = ""; $Side = "N/A"; $Outcome = "";

//Load all entries; 16935 and 16986 are UNI
$query="SELECT * FROM entry where dropped=false and entry.event=".$_GET['event'];
$entry=mysql_query($query);
$entryNum = mysql_num_rows($entry);

//loop entries
$i=0;
while ($i < $entryNum) {

//Pull speakers for entries
$query="SELECT * FROM student, entry_student where entry=".mysql_result($entry,$i,"id")." and student.id=entry_student.student";
$entry_student=mysql_query($query);

?>

<!-- New table for each teams-->
<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
<tr><td colspan=7>
<!-- Put team name on the first row of the table-->
<?php 
echo "<strong>".mysql_result($entry,$i,"name")."</strong>"; 
?>
</td></tr>

<!-- Header rows for the cume sheet-->
<tr>
<th>Round</th>
<th>Judge</th>
<th>Opponent</th>
<th>Side</th>
<th>Outcome</th>

<!-- List student names in header -->
<?php
$studentNum=0;
while ($row = mysql_fetch_array($entry_student, MYSQL_BOTH)) {
echo "<th>".$row['first']." ".$row['last']."</th>";
$SpkrNum[$studentNum]=$row['student'];
$Spkr[$studentNum]="";
$studentNum++;
}
?>

</tr>

<?php
//load all ballots with team
$query="SELECT *, ballot.id as ballot_id FROM ballot, panel, round where ballot.entry=".mysql_result($entry,$i,"id")." and panel.id=ballot.panel and round.id=panel.round and round.post_results>0 ORDER BY round.name ASC";

$ballot=mysql_query($query);

while ($row = mysql_fetch_array($ballot, MYSQL_BOTH)) {

//if it's a new round, print current row and blank out the variables to start a new one
if ($Round <> $row['label'] AND $Round <> "")
 {
   WriteRow($Round, $Judge, $Oppn, $Side, $Outcome, $Spkr, $studentNum);
   $Round=""; $Judge=""; $Oppn=""; $Outcome="";
   $i2=0; while ($i2 < $studentNum) {$Spkr[$i2]="";$i2++;}
 }

//assign round and judge for current ballots
$Round=$row['label'];
if ($Judge<>"") {$Judge.=", ";}
$Judge.=getjudgename($row['judge']);
if ($row['side']=='1') {$Side="Aff";}
if ($row['side']=='2') {$Side="Neg";}
if ($Oppn=="No opponent/Bye" or $Judge=="") {$Side="N/A";}

//load opponents,
$query="SELECT * FROM ballot, entry where panel=".$row['panel']." and entry<>".mysql_result($entry,$i,"id")." and entry.id=ballot.entry";
$OppName=mysql_query($query);
while ($row2 = mysql_fetch_array($OppName, MYSQL_BOTH)) 
 {$Oppn=$row2['name'];}
if ($Oppn=="") {$Oppn="No opponent/Bye";}

//load ballot_value scores
$query="SELECT * FROM ballot_value where ballot=".$row['ballot_id'];
$BallotScore=mysql_query($query);

while ($row2 = mysql_fetch_array($BallotScore, MYSQL_BOTH)) 
 {
   if ($row2['tag']=='ballot' AND $row2['value']=="1") {$Outcome.=" Win";}
   if ($row2['tag']=='ballot' AND $row2['value']=="0") {$Outcome.=" Loss";}
    $i2=0;
    while ($i2 < $studentNum and $row['post_results']==2) {
     if ($row2['student']==$SpkrNum[$i2]) {
      if ($Spkr[$i2]<>"") {$Spkr[$i2].="/";}
      $Spkr[$i2].=$row2['value'];
      }
     if ($row['type']=="elim") {$Spkr[$i2]="";}
     $i2++;
    }
 }

} //This ends the while that's looping through the ballots

WriteRow($Round, $Judge, $Oppn, $Side, $Outcome, $Spkr, $studentNum);
$Round=""; $Judge=""; $Oppn=""; $Outcome="";
$i2=0; while ($i2 < $studentNum) {$Spkr[$i2]="";$i2++;}

?>

</table>

<?php
$i++;
}
?>
</body>
</html>

<?php
mysql_close();

function WriteRow($Round, $Judge, $Oppn, $Side, $Outcome, $Spkr, $studentNum)
{
   echo "<tr>";
   echo "<td>".$Round."</td>";
   echo "<td>".trim($Judge)."</td>";
   echo "<td>".$Oppn."</td>";
   echo "<td>".$Side."</td>";
   echo "<td>".trim($Outcome)."</td>";
   $i2=0;
   while ($i2 < $studentNum) {
     echo "<td>".trim($Spkr[$i2])."</td>";
     $i2++;
    }
// echo "<td>".trim($Spkr[1])."</td>";
   echo "</tr>";
}

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

function getjudgename($judge)
{
   $judgename="";
   $query="SELECT * FROM judge WHERE id=".$judge;
   $dude=mysql_query($query);
   while ($row = mysql_fetch_array($dude, MYSQL_BOTH)) 
     {$judgename=$row['first']." ".$row['last'];}
   return $judgename;
}

$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n";
?>

</body>
</html>

