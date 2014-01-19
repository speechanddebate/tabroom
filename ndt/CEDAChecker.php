
<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$year_str = date("Y"); $mo_str = date("m"); $seas_str=$year_str."-"; $seas_str .= $year_str+1; 
if ($mo_str<=6) { $year_str--; $seas_str=$year_str."-".$year_str+1; }
$date_str=$year_str."-07-01";

?>

<body>
<?php                    // this does CEDA Checker pages
$time_start = microtime(true);

$teamname = array();
$pwin = array();
$ebalwin = array();
$tourney = array(); 
$school1 = array(); 
$school2 = array(); 
$teamschoolname = array(); 
$team = array(); //array to hold speakers and teams; $team[0][0] is $team[team][speaker]
$teamindex = array();

//Load all entries for all tourneys in the circuit; 43 is the NDT circuit
$query="SELECT *, event.name as event_name, tourn.name as tourn_name, tourn.id as tourn_id, event.id as event_id, entry.id as entry_id, entry.name as fullname FROM entry, event, tourn, tourn_circuit where tourn_circuit.circuit=43 and tourn.hidden=false and tourn.id=tourn_circuit.tourn and event.tourn=tourn.id and entry.event=event.id and event.type='policy' and tourn.start > '".$date_str."' ORDER BY tourn.id";
$entry=mysql_query($query);
$entryNum = mysql_num_rows($entry);

//loop entries; see if there's already a team.  If so, markt the number, if not, create a new one
$pwin = array(); $prd = array(); $ewin = array(); $erd = array(); $ebalwin = array(); $ebaloss = array(); $tbpts = array(); $totpts = array();

$i=0;
while ($i < $entryNum) {
$pwin[$i]=0; $prd[$i]=0; $ewin[$i]=0; $erd[$i]=0; $ebalwin[$i] = 0; $ebaloss[$i]=0; $panel=0; $balfor=0; $balvs=0; $tbpts[$i]=0; $totpts[$i]=0;
$school1[$i]=0;$school2[$i]=0;$pwin1=0; $pwin2=0;

$tourneyname[$i]=mysql_result($entry,$i,"tourn_name");
$schoolname_temp="";
$teamname[$i]=getfullteamname(mysql_result($entry,$i,"entry_id"), $schoolN1, $schoolN2, $schoolname_temp);
$school1[$i]=$schoolN1; $school2[$i]=$schoolN2; $teamschoolname[$i]=$schoolname_temp; $lastisprelim=false;

  $query="SELECT *, ballot.id as ballot_id, ballot_value.value as ballot_decision, panel.id as panel_id, round.name as rd_name FROM ballot, ballot_value, panel, round where panel.id=ballot.panel and round.id=panel.round and ballot.id=ballot_value.ballot and ballot_value.tag='ballot' and ballot.entry=".mysql_result($entry,$i,'entry_id')." order by rd_name ASC";
  $ballots=mysql_query($query); 
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
     {
	if (($row['type'] == 'elim' or $row['type'] == 'final') AND $row['panel_id'] <> $panel AND $lastisprelim == false) 
        {$erd[$i] += 1;
         if($balfor > $balvs) {$ewin[$i] += 1;}
	  if ($balfor > 3) { $balfor=3; }
         if ($balvs > 0 and $balfor>2) { $balfor=2; }
         $ebalwin[$i] += $balfor; 
         $ebaloss[$i] += $balvs;
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
         $lastisprelim=false;
        }

      if ($row['ballot_decision'] == 1) {$balfor +=1;}
      if ($row['bye'] == 1 and ($row['type'] == 'elim' or $row['type'] == 'final') and $row['ballot_decision'] == 1) {$balfor +=2; }
      if ($row['ballot_decision'] == 0) {$balvs +=1;}
//    if ($row['entry'] == 494669) { echo "rd name=".$row['rd_name']." label=".$row['label']."panel=".$row['panel_id']." ballots=".$balfor."<br>"; }

      if ($row['type'] <> 'elim' AND $row['panel_id'] <> $panel) 
        {$prd[$i] += 1;
         if($balfor > $balvs) 
          {$pwin[$i] += 1;
          if($row['rd_name']==1) {$pwin1 = 1;}
          if($row['rd_name']==2) {$pwin2 = 1;}}
         if ($row['rd_name']==7) {$pwin[$i] -= $pwin1;}
         if ($row['rd_name']==8) {$pwin[$i] -= $pwin2;}
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
        }

      $panel=$row['panel_id'];
      $elimrd=$row['rd_name'];
      if ($row['type'] == "elim" or $row['type'] == "final") {$lastisprelim = false;}
      if ($row['type'] <> "elim") {$lastisprelim = true;}

     }

     if ($lastisprelim == false)
      {$erd[$i] += 1;
       if($balfor > $balvs) {$ewin[$i] += 1;}
       if ($row['type'] == "final" and $balfor > $balvs) {$tbpts[$i]=5;}
       if ($row['type'] == "final" and $balfor < $balvs) {$tbpts[$i]=3;}
       if ($elimrd==15 ) {$tbpts[$i]=1;}
       if ($balfor > 3) { $balfor=3; }
       if ($balvs > 0 and $balfor>2) { $balfor=2; }

       $ebalwin[$i] += $balfor; 
       $ebaloss[$i] += $balvs;
      }

$i++;
}    //Array is now built; now need to write total and checker pages

//ASSIGN TOTAL POINTS AND SORT
$i=0;
while ($i < $entryNum) {
 $totpts[$i]=$pwin[$i]+$ebalwin[$i];
 if ($school1[$i]<>$school2[$i] and $school2[$i]>0) {$totpts[$i]=$totpts[$i]/2; $tbpts[$i]=$tbpts[$i]/2;} //hybrid adjustment
$i++;
}

//HYBRID ADJUSTMENT

$i=0;
while ($i < $entryNum) {
 if (empty($school2[$i])) {$school2[$i]=0;}
 if (empty($school1[$i])) {$school1[$i]=0;}
 if ($school1[$i]<>$school2[$i] and $school2[$i]>0)
  {
    $entryNum++;
    $teamschoolname[$i]=getschoolname($school1[$i]);
    $teamschoolname[$entryNum]=getschoolname($school2[$i]);
    $tourneyname[$entryNum]=$tourneyname[$i];
    $teamname[$entryNum]=$teamname[$i];
    $pwin[$entryNum]=$pwin[$i];
    $prd[$entryNum]=$prd[$i];
    $ewin[$entryNum]=$ewin[$i];
    $erd[$entryNum]=$erd[$i];
    $ebalwin[$entryNum]=$ebalwin[$i];
    $ebaloss[$entryNum]=$ebaloss[$i];
    $tbpts[$entryNum]=$tbpts[$i];
    $totpts[$entryNum]=$totpts[$i];
    $school1[$entryNum]=$school2[$i];
    $school2[$entryNum]=0;
  }
$i++;
}

array_multisort($teamschoolname, $tourneyname, $totpts, SORT_DESC, $tbpts, $pwin, $ebalwin, $teamname);

//CREATE THE SCHOOL CHECKER PAGE
//totschool=string schoolname, tottourney=string tourneyname, totschoolpoints=points for school at tourney
$totschool = array(); $tottourney = array(); $totschoolpoints = array(); $tottbpts=array(); 
$schoolctr=0; $tournctr=0; $teamctr=0; $totschoolpoints[0]=0;$totschool[0]="";$tottourney[0]=""; $tottbpts[0]=0;

$i=0;
while ($i < $entryNum) {
if ($tourneyname[$i]<>$tottourney[$schoolctr]) 
 {$schoolctr++; 
  $totschool[$schoolctr]=$teamschoolname[$i]; 
  $tottourney[$schoolctr]=$tourneyname[$i]; 
  $totschoolpoints[$schoolctr]=0;
  $tottbpts[$schoolctr]=0;
  $teamctr=0;}
   if ($teamctr<2)
    {$totschoolpoints[$schoolctr] += $totpts[$i]; $tottbpts[$schoolctr] += $tbpts[$i];}
   $teamctr++;
$i++;
}
array_multisort($totschool, $totschoolpoints, SORT_DESC, $tottbpts, SORT_DESC, $tottourney);

//NOW FIGURE TOP 6
$topsixschool = array(); $totsixpts = array(); $totsixtb = array(); $region = array();
$topsixpts[0]=0;$topsixschool[0]=""; $topsixtb[0]=0; $region[0]="";
$tournctr=0; $topsixctr=0;

$i=1;                                                              //loop through school checker arrays
while ($i < $schoolctr) 
 {
  if ($tottourney[$i]<>$tottourney[$i-1]) {$tournctr++;}
  if ($totschool[$i]<>$totschool[$i-1]) {$tournctr=1; $topsixctr++; $topsixschool[$topsixctr]=$totschool[$i]; $topsixpts[$topsixctr]=0; $topsixtb[$topsixctr]=0; $region[$topsixctr]=getcedaregion($totschool[$i]);}
  if ($tournctr < 7) {$topsixpts[$topsixctr] += $totschoolpoints[$i]; $topsixtb[$topsixctr] += $tottbpts[$i];}
  $i++;
 }
array_multisort($topsixpts, SORT_DESC, $topsixtb, SORT_DESC, $topsixschool, $region);

//PRINT TOP 6 

?>
<h2>CEDA POINTS <?php echo $seas_str ?></h2>
Three tables display below; the TOTALS table will show the official points (sum of the top 2 teams at the top 6 tourneys).  The TOURNEY TOTALS table will show 
points for each school by tourney.  The CHECKER page shows points by team and tourney. 
By default all tables will display in the order listed above; you can hide or display any table using the links below. For regional standings, click the region column for the TOTALS table.
To restore national standings click the total points column.<br><br>
<a onclick="document.getElementById('sortme').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide TOTALS</a>
<a onclick="document.getElementById('sortme').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show TOTALS</a>
<a onclick="document.getElementById('tourneytot').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide TOURNEY TOTALS</a>
<a onclick="document.getElementById('tourneytot').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show TOURNEY TOTALS</a>
<a onclick="document.getElementById('tourneyteamtot').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide CHECKERS</a>
<a onclick="document.getElementById('tourneyteamtot').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show CHECKERS</a>
<br><br>
       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <caption>OVERALL Point Totals</caption>
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Total Points</th>
		<th class="smallish">Tiebreaker Points</th>
		<th class="smallish">School</th>
		<th class="smallish">Region</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php

$i=0;
while ($i < $topsixctr) {
 echo "<tr>";
 echo "<td>".$topsixpts[$i]."</td>";
 echo "<td>".$topsixtb[$i]."</td>";
 echo "<td>".$topsixschool[$i]."</td>";
 echo "<td>".$region[$i]."</td>";
 echo "</tr>";
$i++;
}
//FINISH TOP 6 AND PRINT SCHOOL TOTAL PAGE
?>

	</tbody>
       </table>

       <table id="tourneytot" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <caption>Team Totals by Tourney</caption>
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Team</th>
		<th class="smallish">Tournament</th>
		<th class="smallish">Total points</th>
		<th class="smallish">Tiebreaker points</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">

<?php

$i=1;
while ($i < $schoolctr) {
 echo "<tr>";
 echo "<td>".$totschool[$i]."</td>";
 echo "<td>".$tottourney[$i]."</td>";
 echo "<td>".$totschoolpoints[$i]."</td>";
 echo "<td>".$tottbpts[$i]."</td>";
 echo "</tr>";
$i++;
}

//CLOSE SCHOOL/TOURNEY CHECKER SHEETS AND PRINT TEAM BY TOURNEY CHECKER
?>
	</tbody>
       </table>

       <table id="tourneyteamtot" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <caption>Checker Sheets</caption>
       <thead>
	<tr class="yellowrow">
		<th class="smallish">School</th>
		<th class="smallish">Team</th>
		<th class="smallish">Tournament</th>
		<th class="smallish">Prelim wins</th>
		<th class="smallish">Elim ballots</th>
		<th class="smallish">Total points</th>
		<th class="smallish">Tiebreaker points</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneyteamtot">

<?php
//PRINT CHECKER
$i=0;
while ($i < $entryNum) {
if (trim($teamname[$i])<>"--")
 {
 echo "<tr>";
 echo "<td>".$teamschoolname[$i]."</td>";
 echo "<td>".$teamname[$i]."</td>";
 echo "<td>".$tourneyname[$i]."</td>";
 echo "<td>".$pwin[$i]."</td>";
 echo "<td>".$ebalwin[$i]."</td>";
 echo "<td>".$totpts[$i]."</td>";
 echo "<td>".$tbpts[$i]."</td>";
 echo "</tr>";
 }
$i++;
}
?>
	</tbody>
       </table>
<?php

$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n";

mysql_close();
require 'scripts/tabroomfooter.html';

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

function getfullteamname($entry, &$school1, &$school2, &$schoolname_temp)
{
   $schoolname="";$fullname=""; $x=0; $school1=0; $school2=0;
// $query="SELECT *, chapter.name as chapter_name, chapter.id as chapter_id FROM student, entry_student, chapter WHERE entry.id=".$entry." and chapter.id=student.chapter and student.id=student_entry.student";
   $query="SELECT *, chapter.name as chapter_name, chapter.id as chapter_id FROM student, entry_student, chapter WHERE entry_student.entry=".$entry." and student.id=entry_student.student and chapter.id=student.chapter";

   $student=mysql_query($query);
   while ($row = mysql_fetch_array($student, MYSQL_BOTH)) 
    {
    $x++;
    if ($fullname<>"") {$fullname.=" and ";}
    $fullname.=$row['first']." ".$row['last'];
    if ($x==1) {$school1=$row['chapter_id'];}
    if ($x==1) {$schoolname=$row['chapter_name'];}
    if ($x==2) {$school2=$row['chapter_id'];}
    if ($x==2 and $school1<>$school2) {$schoolname.="/".$row['chapter_name'];}
    }
    $schoolname_temp=$schoolname;
    return $schoolname." -- ".$fullname;
}

function getschoolname($school)
{
   $schoolname="";
   $query="SELECT * FROM chapter WHERE id=".$school;
   $school=mysql_query($query);
   while ($row = mysql_fetch_array($school, MYSQL_BOTH)) 
     {$schoolname=$row['name'];}
    return $schoolname;
}

function getcedaregion($schoolname)
{
   $cedaregion="N/A";
   $schoolname = mysql_real_escape_string($schoolname);
   $query="SELECT * FROM chapter WHERE name='".$schoolname."'";
   //echo $schoolname."<br>";
   $chapters=mysql_query($query);
   $numResults = mysql_num_rows($chapters);
   if ($numResults > 0) {
    while ($row = mysql_fetch_array($chapters, MYSQL_BOTH)) 
     {
      if ($row['state']=="CA" or $row['state']=="AZ" or $row['state']=="NV" or $row['state']=="HI") {$cedaregion="Pacific";}
      if ($row['state']=="AK" or $row['state']=="CO" or $row['state']=="ID" or $row['state']=="MT" or $row['state']=="NM") {$cedaregion="West";}
      if ($row['state']=="OR" or $row['state']=="UT" or $row['state']=="WA" or $row['state']=="WY") {$cedaregion="West";}
      if ($row['state']=="IA" or $row['state']=="WI" or $row['state']=="MN" or $row['state']=="SD") {$cedaregion="North Central";}
      if ($row['state']=="ND" or $row['state']=="NE" or $row['state']=="IL") {$cedaregion="North Central";}
      if ($row['state']=="MO" or $row['state']=="KS" or $row['state']=="OK") {$cedaregion="Mid-America";}
      if ($row['state']=="OH" or $row['state']=="IN" or $row['state']=="MI" or $row['state']=="WV") {$cedaregion="East Central";}
      if ($row['state']=="LA" or $row['state']=="TX") {$cedaregion="South Central";}
      if ($row['state']=="AL" or $row['state']=="AR" or $row['state']=="FL" or $row['state']=="GA") {$cedaregion="Southeast";}
      if ($row['state']=="KY" or $row['state']=="MS" or $row['state']=="NC" or $row['state']=="SC" or $row['state']=="TN") {$cedaregion="Southeast";}
      if ($row['state']=="ME" or $row['state']=="MA" or $row['state']=="RI" or $row['state']=="NH" or $row['state']=="NJ") {$cedaregion="Northeast";}
      if ($row['state']=="VT" or $row['state']=="WV" or $row['state']=="NY" or $row['state']=="CT") {$cedaregion="Northeast";}
      if ($row['state']=="PA" or $row['state']=="DE" or $row['state']=="DC" or $row['state']=="MD" or $row['state']=="VA") {$cedaregion="Mid-Atlantic";}
     }
   }
    return $cedaregion;
}

?>

