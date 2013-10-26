
<?php
require 'scripts/tabroomtemplate.html';
$year_str = date("Y"); $mo_str = date("m"); $seas_str=$year_str."-"; $seas_str .= $year_str+1; 
if ($mo_str<=6) { $year_str--; $seas_str=$year_str."-".$year_str+1; }
$date_str=$year_str."-07-01";
?>

<body>

<?php                    // this does seasonal totals
$time_start = microtime(true);

require 'scripts/databaseconnect.php';

$teamname = array();
$pwin = array();
$prd = array();
$ewin = array();
$erd = array();
$ebalwin = array();
$ebaloss = array();
$efinish= array();
$tourneys = array(); 
$team = array(); //array to hold speakers and teams; $team[0][0] is $team[team][speaker]
$teamindex = array();

//Load all entries for all tourneys in the circuit; 43 is the NDT circuit
//$query="SELECT *, event.name as event_name, tourn.name as tourn_name, tourn.id as tourn_id, event.id as event_id, entry.id as entry_id, entry.name as fullname FROM entry, event, tourn, tourn_circuit where tourn_circuit.circuit=43 and tourn_circuit.tourn=tourn.id and event.tourn=tourn.id and entry.event=event.id and (tourn.id=1507 or tourn.id=1522 or tourn.id=1510) and tourn.start > '".$date_str."' ORDER BY fullname asc";
$query="SELECT *, event.name as event_name, tourn.name as tourn_name, tourn.id as tourn_id, event.id as event_id, entry.id as entry_id, entry.name as fullname FROM entry, event, tourn, tourn_circuit where tourn_circuit.circuit=43 and tourn_circuit.tourn=tourn.id and event.tourn=tourn.id and entry.event=event.id and event.type='policy' and tourn.start > '".$date_str."' ORDER BY fullname asc";

$entry=mysql_query($query);
$entryNum = mysql_num_rows($entry);

//loop entries; see if there's already a team.  If so, markt the number, if not, create a new one
$i=0;
while ($i < $entryNum) {

//echo mysql_result($entry,$i,"tourn_name").mysql_result($entry,$i,"event_name").mysql_result($entry,$i,"fullname")."<br>";

//load speakers on current team, and loop them into an array -- $SpkrNum[x] now holds all the speakers
$query="SELECT *, student.id as student_id FROM student, entry_student where entry=".mysql_result($entry,$i,"id")." and student.id=entry_student.student";
$entry_student=mysql_query($query);
$studentNum=0;
while ($row = mysql_fetch_array($entry_student, MYSQL_BOTH)) {
 $SpkrNum[$studentNum]=$row['student_id'];
 $studentNum++;
}

//loop through all existing teams to see if you can find a match; $studentNum is the number of students on the current team
$bigmatch=false;
$match = array(); 
for ($x=1; $x < count($team)+1; $x++) {                                    //loop all existing teams
 for ($y=0; $y < $studentNum; $y++) {                                    //loop all students in current record; the ones to match  
  $match[$y]=false;
  for ($n=0; $n < $studentNum; $n++) {                                   //loop all students on existing teams 
   //echo "TEAM;".$x." Spkr:".$n." is ".$team[$x][$n]." for ".$teamname[$x]." and current spkr is ".$SpkrNum[$y];
   if (empty($team[$x][$n])==false) {
   if ($team[$x][$n]==$SpkrNum[$y]) {$match[$y]=true;}
   }
   //echo "MATCH".$match[$y]."<br>";
  }                                                                      // next n; loop to next speaker on existing team
 }                                                                       // next y; loop to next speaker on team to match

 //now checking for a match
 $smallmatch=true;
 for ($y=0; $y<$studentNum; $y++) {
  if ($match[$y]==false) {$smallmatch=false;}
 }
 if ($smallmatch==true) {$bigmatch=true; $recnum=$x;}

}                                                                        // next x; next existing team

// if ($bigmatch==true) {echo mysql_result($entry,$i,"fullname")."MATCHED".$recnum."<br>";}
 if ($bigmatch==false) {
     $recnum = count($team) + 1;
     $teamname[$recnum] = mysql_result($entry,$i,"fullname");
     //echo "No match; adding ".$teamname[$recnum]." that has ".$studentNum." speakers<br>";	
     for ($n=0; $n < $studentNum; $n++) {$team[$recnum][$n] = $SpkrNum[$n];}
//     if (empty($team[$recnum][1])) {$team[$recnum][1]=0;}
     }

$teamindex[$i] = $recnum;
$i++;                                                                   //this goes back to that very first entry
}

//now make real team names

//echo "<br>FINAL TEAM LIST:<br>";
$studentNum=2;
for ($y=1; $y < count($team)+1; $y++) 
 {
  if (empty($team[$y][1])) {$team[$y][1]=1123;}
  if (empty($team[$y][0])) {$team[$y][0]=1123;}
  $school = "";
  $students = "";
  for ($n=0; $n < $studentNum; $n++) 
   {
   $query="SELECT *, student.first as student_first from student, chapter where student.id=".$team[$y][$n]." and chapter.id=student.chapter";
   $school=mysql_query($query); 
   $numResults = mysql_num_rows($school);
   if ($numResults > 0) {
    if ($students <> "") {$students.=" & ";}
    $students .= mysql_result($school,0,"first"). " ".mysql_result($school,0,"last");
    if ($school <> mysql_result($school,0,"name")) {$school = mysql_result($school,0,"name");}
     } else {$students="Empty Record";}
  }
   $teamname[$y]=$school." - ".$students;
 }

//Pull prelim results

for ($x=1; $x < count($team)+1; $x++)                 //scroll through the master list of teams
{
 $pwin[$x]=0; $prd[$x]=0; $ewin[$x]=0; $erd[$x]=0; $ebalwin[$x] = 0; $ebaloss[$x]=0;
 $i=0;
 while ($i < $entryNum) {
 $lastisprelim=true; $panel=-1; $balfor=0; $balvs=0;
  if ($teamindex[$i] == $x)                      //now scroll back through entries and if a match pull records
   {
    //echo $teamname[$x]." matched with ".mysql_result($entry,$i,"fullname")."<br>";
    $query="SELECT *, panel.id as panel_id, tourn.name as tourn_name, round.name as rd_name, round.label as rd_label, ballot_value.value as ballot_decision FROM tourn, event, round, panel, ballot, ballot_value WHERE tourn.id=event.tourn and event.id=round.event and round.post_results>0 and round.id=panel.round and panel.id=ballot.panel and ballot.id=ballot_value.ballot and ballot_value.tag='ballot' and ballot.entry=".mysql_result($entry,$i,'entry_id')." order by tourn asc, round.name asc, panel.id asc";
    $query="SELECT *, panel.id as panel_id, tourn.name as tourn_name, round.name as rd_name, round.label as rd_label, ballot_value.value as ballot_decision FROM tourn, event, event_setting, round, panel, ballot, ballot_value WHERE round.post_results>0 and event_setting.event=event.id and event_setting.tag='level' and event_setting.value='".$_GET['division']."' and tourn.id=event.tourn and event.id=round.event and round.id=panel.round and panel.id=ballot.panel and ballot.id=ballot_value.ballot and ballot_value.tag='ballot' and ballot.entry=".mysql_result($entry,$i,'entry_id')." order by tourn asc, round.name asc, panel.id asc";
    //echo $query."<br>";
    $ballots=mysql_query($query); 
     while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
     {
      //echo "<tr><td>".$teamname[$x]."</td>";
      //echo "<td>".$row['tourn_name']."</td>";
      //echo "<td>".$row['panel_id']."</td>";
      //echo "<td>".$panel."</td>";
      //echo "<td>".$lastisprelim."</td>";

      if ($row['rd_name'] > 9 AND $row['panel_id'] <> $panel AND $lastisprelim == false) 
        {$erd[$x] += 1;
         if($balfor > $balvs) {$ewin[$x] += 1;}
         $ebalwin[$x] += $balfor; 
         $ebaloss[$x] += $balvs;
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
         $lastisprelim=false;
        }

      if ($row['ballot_decision'] == 1) {$balfor +=1;}
      if ($row['ballot_decision'] == 0) {$balvs +=1;}

      if ($row['rd_name'] <= 9 AND $row['panel_id'] <> $panel) 
        {$prd[$x] += 1;
         if($balfor > $balvs) {$pwin[$x] += 1;}
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
        }

      $panel=$row['panel_id'];
      if ($row['rd_name'] > 9) {$lastisprelim = false;}
      if ($row['rd_name'] <= 9) {$lastisprelim = true;}

      //echo "<td>".$row['rd_name']."</td>";
      //echo "<td>".$row['rd_label']."</td>";
      //echo "<td>".$row['ballot_decision']."</td>";
      //echo "<td>".$pwin[$x]."</td>";
      //echo "<td>".$prd[$x]."</td>";
      //echo "<td>".$ewin[$x]."</td>";
      //echo "<td>".$erd[$x]."</td>";
      //echo "<td>".$ebalwin[$x]."</td>";
      //echo "<td>".$ebaloss[$x]."</td></tr>";
     }
     if ($lastisprelim == false)
      {$erd[$x] += 1;
       if($balfor > $balvs) {$ewin[$x] += 1;}
       $ebalwin[$x] += $balfor; 
       $ebaloss[$x] += $balvs;
       //echo "<tr><td>Ewin:".$ewin[$x]." ERd:".$erd[$x]." Ebalwin:".$ebalwin[$x]." Ebaloss:".$ebaloss[$x]."</td></tr>";
      }
   }
 $i++;
}   //ends the while
}   //ends the for $x
?>
<h2>Seasonal Totals <?php echo $seas_str ?>
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/SeasonalTotals.php?division=open'> open</a>
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/SeasonalTotals.php?division=jv'> jv</a>
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/SeasonalTotals.php?division=novice'>novice</a>
<span style="font-size:24px"><a class="smallish" href='https://www.tabroom.com/jbruschke/TeamResults.php'>return to results main</a>
</span></h2>
Clicking on any team will launch their NDT bid sheet.  Note that columns are sortable.  Use links above to select division.</br></br>

	<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">

	<thead>

	<tr class="yellowrow">
		<th class="smallish">Team</th>
		<th class="smallish">Prelim wins</th>
		<th class="smallish">Prelim rds</th>
		<th class="smallish">Prelim %</th>
		<th class="smallish">Elim wins</th>
		<th class="smallish">Elim rds</th>
		<th class="smallish">Elim %</th>
		<th class="smallish">Elim ballots won</th>
		<th class="smallish">Elim ballots lost</th>
		<th class="smallish">Elim ballot%</th>
	</tr>
	</thead>

	<tbody>

<?php
for ($x=1; $x<count($team)+1; $x++) 
 {
  if ($prd[$x]>0) 
  {
  // echo "<tr>\n<td class=\"smallish\">".$teamname[$x]."Main Results Dashboard</a></td>";
  // echo "<tr><td><a class=\"smallish\" href='https://www.tabroom.com/jbruschke/TeamBidSheet.php'>".trim($teamname[$x])."</a></td>";
  echo "<tr><td><a href='https://www.tabroom.com/jbruschke/TeamBidSheet.php?id1=".$team[$x][0]."&id2=".$team[$x][1]."'>".trim($teamname[$x])."</a></td>";
  echo "<td>".$pwin[$x]."</td>";
  echo "<td>".$prd[$x]."</td>";
  if ($prd[$x]==0) {$prd[$x]=1;}
  echo "<td>".number_format($pwin[$x]/$prd[$x],2)."</td>";
  echo "<td>".$ewin[$x]."</td>";
  echo "<td>".$erd[$x]."</td>";
  $epct="0";
  $totebal=$ewin[$x]+$erd[$x];
  if ($totebal>0) {$epct=number_format($ewin[$x]/$erd[$x],2);}
  echo "<td>".$epct."</td>";
  echo "<td>".$ebalwin[$x]."</td>";
  echo "<td>".$ebaloss[$x]."</td>";
  $epct="0";
  $totebal=$ebalwin[$x]+$ebaloss[$x];
  if ($totebal>0) {$epct = number_format($ebalwin[$x]/$totebal,2);}
  echo "<td>".$epct."</td></tr>\n";
  }
 }

mysql_close();

?>

	<tbody>
</table>

<?php
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
