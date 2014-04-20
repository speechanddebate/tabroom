
<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$year_str = date("Y"); $mo_str = date("m"); $seas_str=$year_str."-"; $seas_str .= $year_str+1; 
if ($mo_str<=6) { $year_str--; $seas_str=$year_str."-".$year_str+1; }
$date_str=$year_str."-07-01";
?>

<body>
<?php                    // this does NDT Points and checker pages
$time_start = microtime(true);

$teamname = array();
$pwin = array();
$ploss = array();
$epts = array();
$tourney = array(); 
$school1 = array(); 
$school2 = array(); 
$teamschoolname = array(); 
$team = array(); //array to hold speakers and teams; $team[0][0] is $team[team][speaker]
$teamindex = array();
$ElimStr = array();
$eventname = array();

//Load all entries for all tourneys in the circuit; 43 is the NDT circuit
$query="SELECT *, event.name as event_name, tourn.name as tourn_name, tourn.id as tourn_id, event.id as event_id, entry.id as entry_id, entry.name as fullname FROM entry, event, tourn, tourn_circuit WHERE tourn_circuit.circuit=43 and tourn.hidden=false and tourn.id=tourn_circuit.tourn and event.tourn=tourn.id and entry.event=event.id and event.type='policy' and dropped=false and tourn.start > '".$date_str."' ORDER BY tourn.id, entry.id";
$entry=mysql_query($query);
$entryNum = mysql_num_rows($entry); 

$pwin = array(); $epts = array(); $totpts = array(); $vpts=array();

$i=0;
while ($i < $entryNum) {
$pwin[$i]=0; $ploss[$i]=0; $vpts[$i]=0; $epts[$i]=0; $panel=0; $balfor=0; $balvs=0; $totpts[$i]=0;
$school1[$i]=0;$school2[$i]=0;

$tourneyname[$i]=mysql_result($entry,$i,"tourn_name"); $eventname[$i]=isvarsity(mysql_result($entry,$i,'event_id'));
$schoolname_temp="";
$teamname[$i]=getfullteamname(mysql_result($entry,$i,"entry_id"), $schoolN1, $schoolN2, $schoolname_temp);
//if ($pringflag==true) { print $i." entry id=".mysql_result($entry,$i,"entry_id")." ".$tourneyname[$i]." ".$teamname[$i]."<br>"; }
$school1[$i]=$schoolN1; $school2[$i]=$schoolN2; $teamschoolname[$i]=$schoolname_temp; $lastisprelim=false; $ElimStr[$i]="";

  $query="SELECT *, ballot.id as ballot_id, ballot_value.value as ballot_decision, panel.id as panel_id, round.name as rd_name FROM (ballot LEFT OUTER JOIN ballot_value on ballot.id=ballot_value.ballot), panel, round WHERE round.post_results>0 and panel.id=ballot.panel and round.id=panel.round and (ballot_value.tag='ballot' or ballot_value.tag is null) and ballot.entry=".mysql_result($entry,$i,'entry_id')." order by rd_name ASC";
  $ballots=mysql_query($query); 
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
     {
      $printflag=false; //if (mysql_result($entry,$i,'entry_id') == 555691) {$printflag=true; }
      if ($printflag==true) {echo "Panel=".$row['panel_id']." ballot=".$row['ballot_id']." ".$row['ballot_decision']." panel marker=".$panel;}

      if (($row['type'] == 'elim' or $row['type'] == 'final') AND $row['panel_id'] <> $panel AND $lastisprelim == false) 
        {
         if($balfor > $balvs and $balvs==0) {$epts[$i] += 6;}
         if($balfor > $balvs and $balvs>0) {$epts[$i] += 5;}
         if($balvs > $balfor and $balfor>0) {$epts[$i] += 4;}
         if($balvs > $balfor and $balfor==0) {$epts[$i] += 3;}
	  $ElimStr[$i] = $ElimStr[$i].$balfor."-".$balvs." "; 
	  if ($printflag == true) {echo " Elim Ballots for:".$balfor." Ballots vs:".$balvs." Panel:".$row['panel_id']." ".$row['label']."<br>";}
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
         $lastisprelim=false;
	  if ( $printflag == true ) { echo "Elim points are now:".$epts[$i]."<br>"; }
        }

      if ( (($row['type'] <> 'elim' and $row['type'] <> 'final') or $lastisprelim==true) AND $row['panel_id'] <> $panel and ($balfor+$balvs)>0 ) 
        {
	  if ($printflag==true) {echo "Ballots for=".$balfor." ballots against=".$balvs;}
         if($balfor > $balvs) {$pwin[$i] += 1;}
         if($balfor < $balvs) {$ploss[$i] += 1;}
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
         if ($printflag==true) {echo "Prelim record is now: ".$pwin[$i]."-".$ploss[$i]."<br>";}
        }

      if ($row['ballot_decision'] == 1) {$balfor +=1;}
      if ($row['ballot_decision'] == 0) {$balvs +=1;}
      if ($row['bye'] == 1 and $row['ballot_decision'] == 1 and ($row['type'] == 'elim' or $row['type'] == 'final')) {$balfor=3; $balvs=0;}

      $panel=$row['panel_id'];
      $elimrd=$row['rd_name'];
      $lastisprelim = true;
      if ($row['type'] == 'elim' or $row['type'] == 'final') {$lastisprelim = false;}
     }

      if ($lastisprelim == false) 
        {
         if($balfor > $balvs and $balvs==0) {$epts[$i] += 6;}
         if($balfor > $balvs and $balvs>0) {$epts[$i] += 5;}
         if($balvs > $balfor and $balfor>0) {$epts[$i] += 4;}
         if($balvs > $balfor and $balfor==0) {$epts[$i] += 3;}
	  $ElimStr[$i] = $ElimStr[$i].$balfor."-".$balvs." "; 
	  if ($printflag == true) {echo " Ballots for:".$balfor." Ballots vs:".$balvs." Panel:".$row['panel_id']."<br>";}
        }

      if ($lastisprelim == true) 
        {
	  if ($printflag==true) {echo "Ballots for=".$balfor." ballots against=".$balvs;}
         if($balfor > $balvs) {$pwin[$i] += 1;}
         if($balfor < $balvs) {$ploss[$i] += 1;}
         $balfor=0; $balvs=0;
         $panel=$row['panel_id'];
         if ($printflag==true) {echo "Prelim record is now: ".$pwin[$i]."-".$ploss[$i]."<br>";}
        }


$i++;
}    //Array is now built; now need to write total and checker pages

//ASSIGN TOTAL POINTS AND SORT
$i=0; 
while ($i < $entryNum) {
 //if (mysql_result($entry,$i,'entry_id') == 485375) {$printflag=true; }
 $totpts[$i]=$epts[$i];
 if ($printflag == true) { echo "Elim points:".$epts[$i]."<br>"; }
 if ($pwin[$i]>=4 and $ploss[$i]==0) {$totpts[$i] += 16;}
 if ($pwin[$i]>=5 and $ploss[$i]==1) {$totpts[$i] += 15;}
 if ($pwin[$i]==6 and $ploss[$i]==2) {$totpts[$i] += 14;}
 if ($pwin[$i]==3 and $ploss[$i]==1) {$totpts[$i] += 14;}
 if ($pwin[$i]==5 and $ploss[$i]==2) {$totpts[$i] += 14;}
 if ($pwin[$i]==4 and $ploss[$i]==2) {$totpts[$i] += 13;}
 if ($pwin[$i]==4 and $ploss[$i]==3) {$totpts[$i] += 13;}
 if ($pwin[$i]==5 and $ploss[$i]==3) {$totpts[$i] += 13;}
 if ($pwin[$i]==3 and $ploss[$i]==2) {$totpts[$i] += 13;}
 if ($pwin[$i]==$ploss[$i] and $pwin[$i]>1) {$totpts[$i] += 12;}
 if ($pwin[$i]==3 and $ploss[$i]==5) {$totpts[$i] += 11;}
 if ($pwin[$i]==3 and $ploss[$i]==4) {$totpts[$i] += 11;}
 if ($pwin[$i]==2 and $ploss[$i]==5) {$totpts[$i] += 11;}
 if ($pwin[$i]==2 and $ploss[$i]==4) {$totpts[$i] += 11;}
 if ($pwin[$i]==1 and $ploss[$i]==3) {$totpts[$i] += 10;}
 if ($pwin[$i]==2 and $ploss[$i]==6) {$totpts[$i] += 10;}
 if ($pwin[$i]==1 and $ploss[$i]<>3) {$totpts[$i] += 9;}
 if ($pwin[$i]==0 and $ploss[$i]>=4) {$totpts[$i] += 8;}

 if ($school1[$i]<>$school2[$i] and $school2[$i]>0) {$totpts[$i]=0;} //hybrid adjustment
 if (isvarsity(mysql_result($entry,$i,'event_id'))=="open") {$vpts[$i]=$totpts[$i];} //copy to varsity points if appropriate
 //if ($school1[$i]=6260 and $totpts[$i]>0) {echo " event=".mysql_result($entry,$i,'event_id')." tot:".$totpts[$i]." var:".$vpts[$i]."<br>";}
 if ($printflag == true) { echo "Prelim points:".($totpts[$i]-$epts[$i])."<br>"; }
 $printflag=false;
$i++;
}

array_multisort($teamschoolname, $tourneyname, $totpts, SORT_DESC, $pwin, $ploss, $epts, $vpts, $teamname, $school1, $ElimStr, $eventname);

//CREATE THE SCHOOL CHECKER PAGE
//totschool=string schoolname, tottourney=string tourneyname, totschoolpoints=points for school at tourney
$totschool = array(); $tottourney = array(); $totschoolpoints = array(); $totvpts=array(); $totschoolnum=array();
$schoolctr=0; $tournctr=0; $teamctr=0; $totschoolpoints[0]=0;$totschool[0]="";$tottourney[0]=""; $totvpts[0]=0; $totschoolnum[0]=0; 

$i=0;
while ($i < $entryNum) {
if ($tourneyname[$i]<>$tottourney[$schoolctr]) 
  {
  $schoolctr++; 
  $totschool[$schoolctr]=$teamschoolname[$i]; 
  $totschoolnum[$schoolctr]=$school1[$i]; 
  $tottourney[$schoolctr]=$tourneyname[$i]; 
  $totschoolpoints[$schoolctr]=0;
  $teamctr=0;
  $totvpts[$schoolctr]=0;
  }
if ($teamctr<2) {$totschoolpoints[$schoolctr] += $totpts[$i]; }
$teamctr++;
$i++;
}

//now do again for varsity points
array_multisort($teamschoolname, $tourneyname, $vpts, SORT_DESC, $pwin, $ploss, $epts, $totpts, $teamname, $school1, $ElimStr, $eventname);
$currschool=0;
$i=0;
while ($i < $entryNum) {
if ($i==0 OR $tourneyname[$i]<>$tourneyname[$i-1]) 
  {
   $x=0;
   while ($x < $schoolctr) {
    if ($teamschoolname[$i] == $totschool[$x] and $tourneyname[$i] == $tottourney[$x]) { $currschool = $x; $x = $schoolctr+1; }
    $x++;
   }
   $teamctr=0;
  }

//echo $teamschoolname[$i]." is from ".$totschool[$currschool]."<br>";
if ($teamctr<2) { $totvpts[$currschool] += $vpts[$i]; }
$teamctr++;
$i++;
}

//now sort school totals
array_multisort($totschool, $totschoolpoints, SORT_DESC, $totvpts, SORT_DESC, $tottourney, $totschoolnum);

//NOW FIGURE TOP 8
$topeightschool = array(); $toteightpts = array(); $region = array(); $topeightschoolnum=array();
$topeightpts[0]=0;$topeightschool[0]=""; $region[0]=""; $topeightschoolnum[0]=0;
$tournctr=0; $topeightctr=0;

$i=1;                                                              //loop through school checker arrays
while ($i < $schoolctr) 
 {
  if ($tottourney[$i]<>$tottourney[$i-1]) {$tournctr++;}
  if ($totschool[$i]<>$totschool[$i-1]) {$tournctr=1; $topeightctr++; $topeightschool[$topeightctr]=$totschool[$i]; $topeightschoolnum[$topeightctr]=$totschoolnum[$i]; $topeightpts[$topeightctr]=0; $region[$topeightctr]=getndtdistrict($totschool[$i]);}
  if ($tournctr < 9) {$topeightpts[$topeightctr] += $totschoolpoints[$i];}
  $i++;
 }
array_multisort($topeightpts, SORT_DESC, $topeightschool, $region, $topeightschoolnum);

//PRINT TOP 8 

?>
<h2>NDT POINTS <?php echo $seas_str ?></h2>
Three tables display below; the TOTALS table will show the official points (sum of the top 2 teams at the top 8 tourneys).  The TOURNEY TOTALS table will show 
points for each school by tourney.  The CHECKER page shows points by team and tourney. 
By default all tables will display in the order listed above; you can hide or display any table using the links below. For district or community college standings, click the approriate column on the TOTALS table.
To restore national standings click the total points column.<br><br>
<a onclick="document.getElementById('sortme').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide TOTALS</a>
<a onclick="document.getElementById('sortme').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show TOTALS</a>
<a onclick="document.getElementById('sortmeVar').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide VARSITY</a>
<a onclick="document.getElementById('sortmeVar').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show VARSITY</a>
<a onclick="document.getElementById('tourneytot').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide TOURNEY</a>
<a onclick="document.getElementById('tourneytot').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show TOURNEY</a>
<a onclick="document.getElementById('tourneyteamtot').style.display='none';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">hide CHECKERS</a>
<a onclick="document.getElementById('tourneyteamtot').style.display='';return false;" href="" style="text-decoration:none;border-bottom:1px dotted blue;">show CHECKERS</a>
<br><br>
       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <caption>OVERALL Point Totals</caption>
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Total Points</th>
		<th class="smallish">School</th>
		<th class="smallish">District</th>
		<th class="smallish">Community College</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php

$i=0;
while ($i < $topeightctr) {
 echo "<tr>";
 echo "<td>".$topeightpts[$i]."</td>";
 echo "<td>".$topeightschool[$i]."</td>";
 echo "<td>".$region[$i]."</td>";
 echo "<td>".iscc($topeightschoolnum[$i])."</td>";
 echo "</tr>";
$i++;
}
//FINISH TOP 8 AND RECALC FOR VARSITY ONLY
?>

	</tbody>
       </table>

<?php

array_multisort($totschool, $totvpts, SORT_DESC, $totschoolpoints, SORT_DESC, $tottourney);

$topeightschool = array(); $toteightpts = array(); $region = array();
$topeightpts[0]=0;$topeightschool[0]=""; $region[0]="";
$tournctr=0; $topeightctr=0;

$tournctr=0; $topeightctr=0;
$i=1;
while ($i < $schoolctr) 
 {
  if ($tottourney[$i]<>$tottourney[$i-1]) {$tournctr++;}
  if ($totschool[$i]<>$totschool[$i-1]) {$tournctr=1; $topeightctr++; $topeightschool[$topeightctr]=$totschool[$i]; $topeightschoolnum[$topeightctr]=$totschoolnum[$i]; $topeightpts[$topeightctr]=0; $region[$topeightctr]=getndtdistrict($totschool[$i]);}
  if ($tournctr < 9) {$topeightpts[$topeightctr] += $totvpts[$i];}
  $i++;
 }
array_multisort($topeightpts, SORT_DESC, $topeightschool, $region, $topeightschoolnum);

?>

       <table id="sortmeVar" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <caption>VARSITY Point Totals</caption>
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Varsity Points</th>
		<th class="smallish">School</th>
		<th class="smallish">Region</th>
		<th class="smallish">Community College</th>
	</tr>
	</thead>

    <tbody id="myTbodytotals">

<?php

$i=0;
while ($i < $topeightctr) {
 echo "<tr>";
 echo "<td>".$topeightpts[$i]."</td>";
 echo "<td>".$topeightschool[$i]."</td>";
 echo "<td>".$region[$i]."</td>";
 echo "<td>".iscc($topeightschoolnum[$i])."</td>";
 echo "</tr>";
$i++;
}
//FINISH VARSITY AND PRINT SCHOOL TOTAL PAGE
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
		<th class="smallish">Varsity points</th>
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
 echo "<td>".$totvpts[$i]."</td>";
 echo "</tr>";
$i++;
}
                                            //CLOSE VARSITY, SHOW TEAM BY TEAM CHECKERS
?>         

	</tbody>
       </table>

       <table id="tourneyteamtot" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <caption>Checker Sheets</caption>
       <thead>
	<tr class="yellowrow">
		<th class="smallish">School</th>
		<th class="smallish">Team</th>
		<th class="smallish">Event</th>
		<th class="smallish">Tournament</th>
		<th class="smallish">Prelim record</th>
		<th class="smallish">Elim results</th>
		<th class="smallish">Elim points</th>
		<th class="smallish">Total points</th>
		<th class="smallish">Varsity points</th>
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
 echo "<td>".$eventname[$i]."</td>";
 echo "<td>".$tourneyname[$i]."</td>";
 echo "<td>".$pwin[$i]."-".$ploss[$i]."</td>";
 $ElimStr[$i] = substr_replace($ElimStr[$i] ,"",-1);
 echo "<td class='nowrap'>".$ElimStr[$i]."</td>";
 echo "<td>".$epts[$i]."</td>";
 echo "<td>".$totpts[$i]."</td>";
 echo "<td>".$vpts[$i]."</td>";
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

function getndtdistrict($schoolname)
{
   $ndtdistrict="N/A";
   $query="SELECT * FROM chapter WHERE name='".$schoolname."'";
   $chapters=mysql_query($query);
   $numResults = mysql_num_rows($chapters);
   if ($numResults > 0) {
    while ($row = mysql_fetch_array($chapters, MYSQL_BOTH)) 
     {
      if ($row['state']=="CA" or $row['state']=="AZ" or $row['state']=="NV" or $row['state']=="HI" or $row['state']=="AK") {$ndtdistrict="One";}
      if ($row['state']=="AK" or $row['state']=="CO" or $row['state']=="ID" or $row['state']=="MT" or $row['state']=="OR" or $row['state']=="UT" or $row['state']=="WA" or $row['state']=="WY") {$ndtdistrict="Two";}
      if ($row['state']=="MO" or $row['state']=="KS" or $row['state']=="OK" or $row['state']=="LA" or $row['state']=="TX" or $row['state']=="AR" or $row['state']=="NM") {$ndtdistrict="Three";}
      if ($row['state']=="IA" or $row['state']=="WI" or $row['state']=="MN" or $row['state']=="SD" or $row['state']=="ND" or $row['state']=="NE") {$ndtdistrict="Four";}
      if ($row['state']=="OH" or $row['state']=="IN" or $row['state']=="MI" or $row['state']=="IL") {$ndtdistrict="Five";}
      if ($row['state']=="AL" or $row['state']=="FL" or $row['state']=="GA" or $row['state']=="KY" or $row['state']=="MS" or $row['state']=="NC" or $row['state']=="SC" or $row['state']=="TN") {$ndtdistrict="Six";}
      if ($row['state']=="PA" or $row['state']=="DE" or $row['state']=="DC" or $row['state']=="MD" or $row['state']=="VA" or $row['state']=="NJ" or $row['state']=="WV") {$ndtdistrict="Seven";}
      if ($row['state']=="ME" or $row['state']=="MA" or $row['state']=="RI" or $row['state']=="NH" or $row['state']=="NY" or $row['state']=="CT" or $row['state']=="VT") {$ndtdistrict="Eight";}
     }
   }
    return $ndtdistrict;
}

function isvarsity($event)
{
   $query="SELECT * from event_setting where event_setting.event=".$event." and event_setting.tag='level'";
   $settings=mysql_query($query);
   $numResults = mysql_num_rows($settings);
   $bool=FALSE;
   if ($numResults > 0) 
    {
     while ($row = mysql_fetch_array($settings, MYSQL_BOTH)) 
     //{if ($row['value']=="open") {$bool=TRUE;}}
     return $row['value'];
    }
    //return $bool;
}

function iscc($chapter)
{
 $bool="False";
 if ($chapter==6298 or $chapter==6347 or $chapter==6111 or $chapter==6299) {$bool="True";}
 return $bool;
}

?>

