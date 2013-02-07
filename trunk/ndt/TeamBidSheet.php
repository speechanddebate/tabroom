<?php
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';
$student1=$_GET['id1'];
$student2=$_GET['id2'];
$teamname=""; $schoolname=""; $studentname= array();
$chapterid=0;

$query="SELECT *, chapter.name as chapter_name, chapter.id as chapter_id, student.id as student_id FROM student, chapter where (student.id=".$student1." or student.id=".$student2.") and chapter.id=student.chapter";
$ballots=mysql_query($query); 
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
   {
    $chapterid=$row['chapter_id'];
    $schoolname=$row['chapter_name'];
    if ($teamname <> "") {$teamname .= " and ";}
    $teamname .= $row['first']." ".$row['last'];
    if ($row['student_id']==$student1) {$studentname[1]=$row['first']." ".$row['last'];}
    if ($row['student_id']==$student2) {$studentname[2]=$row['first']." ".$row['last'];}
   }
$teamname=$schoolname." -- ".$teamname;

$coaches=""; $coachemail=""; $coachphone=""; $address="N/A";
$query="SELECT * FROM chapter_admin, account where chapter_admin.chapter=".$chapterid." and account.id=chapter_admin.account";
$adstaff=mysql_query($query); 
   while ($row = mysql_fetch_array($adstaff, MYSQL_BOTH)) 
   {
    if ($coaches<> "") {$coaches .= ", ";}
    $coaches .= $row['first']." ".$row['last'];
    if ($row['email'] == "") {$row['email']="N/A";}
    if ($coachemail<> "") {$coachemail .= ", ";}
    $coachemail .= $row['email'];
    if ($row['phone']=="") {$row['phone']="N/A";}
    if ($coachphone<> "") {$coachphone .= ", ";}
    $coachphone .= $row['phone'];
    if ($row['street']<>"") {$address=$row['street'].", ".$row['city'].", ".$row['state'].", ".$row['zip'];}
   }
?>

<body>
<h2>TEAM BID SHEET FOR <?php echo $teamname; ?></h2>
<br>
<h2>SECTION I: CONTACT INFORMATION</H2>
       <table id="contact" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Item</th>
		<th class="smallish">Data</th>
	</tr>
	</thead>

    <tbody id="myTbodytourneytot">

<?php

echo "<tr><td>School</td><td>".$schoolname."</td></tr>";
echo "<tr><td>Debater One</td><td>".$studentname[1]."</td></tr>";
echo "<tr><td>Debater Two</td><td>".$studentname[2]."</td></tr>";
echo "<tr><td>Director and Coaches</td><td>".$coaches."</td></tr>";
//echo "<tr><td>Address</td><td>".$address."</td></tr>";
//echo "<tr><td>Coach email</td><td>".$coachemail."</td></tr>";
//echo "<tr><td>Coach phone</td><td>".$coachphone."</td></tr>";


?>
	</tbody>
       </table>
<br>
<h2>section II: tabular summary</h2>
Round robin and JV results do not count in prelim totals but appear separately.  Elim rounds are counted as win/loss and not as ballot counts.  Elim byes and close-outs are not counted in elim totals.
<h4>RECORD of debaters as a team</h4>

<?php

$balfor=0; $balvs=0; 
$round= array(); $tourn= array(); $tournid=array(); $tourndate= array(); $side= array(); $panel=array(); $roundlabel=array();
$win = array(); $outcome = array(); $isprelim= array(); $spkr1 = array(); $spkr2 = array(); $isRR=array(); $isopen=array();
$oppn = array(); $event = array(); $x=0; $panel[0]=-1; $ballotid=-1;
$year=date("Y"); $month=date("M"); if ($month<8) {$year=$year-1;}
$query="SELECT *, ballot.id as ballot_id, entry.id as entry_id, event.type as event_type, round.label as round_label, round.name as rd_name, ballot_value.value as ballot_decision, tourn.name as tourn_name, event.name as event_name, event.id as event_id, panel.id as panel_id, round.name as rd_name, tourn.id as tourn_id FROM ballot_value, ballot, entry_student, panel, round, event, tourn, entry WHERE ballot_value.tag='ballot' and ballot_value.ballot=ballot.id and tourn.id=event.tourn and event.id=round.event and round.id=panel.round and panel.id=ballot.panel and ballot.entry=entry_student.entry and (entry_student.student=".$student1." or entry_student.student=".$student2.") and entry.id=entry_student.entry and tourn.start>'".$year."-09-01' ORDER BY tourn.start asc, tourn.id asc, round.name asc, panel.id asc, ballot.id asc";
$ballots=mysql_query($query); 

  while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
     { 
      if ($row['panel_id']<>$panel[$x]) {$x++; $balfor=0; $balvs=0;}
      if ($row['ballot_decision'] == 1 and $row['ballot_id']<>$ballotid) {$balfor +=1;}
      if ($row['ballot_decision'] == 0 and $row['ballot_id']<>$ballotid) {$balvs +=1;}
         $round[$x]=""; $tourn[$x]=""; $event[$x]=""; $outcome[$x]=""; $win[$x]=0; $isRR[$x]=0; $isopen[$x]=0;
         if(DivisionIsOpen($row['event_id']) == TRUE) {$isopen[$x]=1;}
         $round[$x]=$row['rd_name'];
         $roundlabel[$x]=$row['round_label'];
         $event[$x]=$row['event_name'];
         $tourn[$x]=$row['tourn_name'];
         $tournid[$x]=$row['tourn_id'];
         $oppn[$x]=getopponent($row['panel_id'], $row['entry_id']);
         $tourndate[$x]=$row['start'];
         $side[$x]=$row['side'];
         getspeakers($spkr1[$x], $spkr2[$x], $row['entry_id']);
         $outcome[$x]=makeoutcomestring($balfor, $balvs);
         if($balfor > $balvs) {$win[$x] = 1;}
         $isprelim[$x]=1; if ($row['rd_name'] > 9) {$isprelim[$x]=0;}
         if ($row['event_type']=='roundrobin') {$isRR[$x]=1;}
         $panel[$x]=$row['panel_id']; 
         $ballotid=$row['ballot_id']; 
     }
?>
       <table id="recgrid" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Tourney</th>
		<th class="smallish">Division</th>
		<th class="smallish">Date</th>
		<th class="smallish">Prelims</th>
		<th class="smallish">Triples</th>
		<th class="smallish">Doubles</th>
		<th class="smallish">Octos</th>
		<th class="smallish">Quarters</th>
		<th class="smallish">Semis</th>
		<th class="smallish">Finals</th>
		<th class="smallish">Total</th>
	</tr>
	</thead>
        <tbody id="myTbodytotals">
<?php

$i=1; $pwin=0; $ploss=0; $trip=""; $doub=""; $octo=""; $qrtr=""; $semi=""; $finl=""; $totwin=0; $totloss=0; $rrwin=0; $rrloss=0;
$totpwin=0; $totploss=0; $totewin=0; $toteloss=0; $jvwin=0; $jvloss=0;
while ($i <= $x) {
if (teammatch($spkr1[$i], $spkr2[$i], $student1, $student2)==TRUE)
{
 if ($win[$i]==1 and $isprelim[$i]==1) {$pwin++;}
 if ($win[$i]==0 and $isprelim[$i]==1) {$ploss++;}
 if ($win[$i]==1 and $isprelim[$i]==0 and $isopen[$i]==1) {$totewin++;}
 if ($win[$i]==0 and $isprelim[$i]==0 and $isopen[$i]==1) {$toteloss++;}
 if ($win[$i]==1 and $isprelim[$i]==0 and $isopen[$i]==0) {$jvwin++;}
 if ($win[$i]==0 and $isprelim[$i]==0 and $isopen[$i]==0) {$jvloss++;}
 if ($win[$i]==1) {$totwin++;}
 if ($win[$i]==0) {$totloss++;}
 if ($round[$i]==11) {$trip=$outcome[$i];}
 if ($round[$i]==12) {$doub=$outcome[$i];}
 if ($round[$i]==13) {$octo=$outcome[$i];}
 if ($round[$i]==14) {$qrtr=$outcome[$i];}
 if ($round[$i]==15) {$semi=$outcome[$i];}
 if ($round[$i]==16) {$finl=$outcome[$i];}
 if (($i<$x and $tourn[$i]<>$tourn[$i+1]) OR $i==$x)
  {
  echo "<tr>";
  echo "<td>".$tourn[$i]."</td>";
  echo "<td>".$event[$i]."</td>";
  $date = strtotime($tourndate[$i]);
  echo "<td>".date('d/M/Y', $date)."</td>";
  echo "<td>".$pwin."-".$ploss."</td>";
  echo "<td>".$trip."</td>";
  echo "<td>".$doub."</td>";
  echo "<td>".$octo."</td>";
  echo "<td>".$qrtr."</td>";
  echo "<td>".$semi."</td>";
  echo "<td>".$finl."</td>";
  echo "<td>".$totwin."-".$totloss."</td>";
  echo "</tr>";
  if ($isRR[$i]==1 and $isopen[$i]==1) {$rrwin+=$pwin; $rrloss+=$ploss;}
  if ($isRR[$i]==0 and $isopen[$i]==1) {$totpwin+=$pwin; $totploss+=$ploss;}
  if ($isopen[$i]==0) {$jvwin+=$pwin; $jvloss+=$ploss;}
  $pwin=0; $ploss=0; $trip=""; $doub=""; $octo=""; $qrtr=""; $semi=""; $finl=""; $totwin=0; $totloss=0;
  }
}             // end of big if, only process for exact matches
$i++;
}

?>
	</tbody>
       </table>

       <h4>SUMMARY of record as a team</h4>

       <table id="summrec" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Item</th>
		<th class="smallish">Summary</th>
		<th class="smallish">Percent</th>
	</tr>
	</thead>
        <tbody id="myTbodytotals">

<?php

echo "<tr><td>A1. Prelim Record</td><td>".$totpwin."-".$totploss."</td><td>".getpctstring($totpwin,$totploss)."%</td></tr>";
echo "<tr><td>A2. Elim Record</td><td>".$totewin."-".$toteloss."</td><td>".getpctstring($totewin,$toteloss)."%</td></tr>";
echo "<tr><td>B. Round Robins</td><td>".$rrwin."-".$rrloss."</td><td>".getpctstring($rrwin,$rrloss)."%</td></tr>";
echo "<tr><td>C. JV/Novice</td><td>".$jvwin."-".$jvloss."</td><td>".getpctstring($jvwin,$jvloss)."%</td></tr>";
$totwin = $totpwin + $totewin + $rrwin + $jvwin;
$totloss = $totploss + $toteloss + $rrloss + $jvloss;
echo "<tr><td>C. Total Record</td><td>".$totwin."-".$totloss."</td><td>".getpctstring($totwin,$totloss)."%</td></tr>";

?>

       </tbody>
       </table>

<h4>RECORD OF INDIVIDUAL DEBATERS WITH OTHER COLLEAGUES</h4>
<h4><?php echo $studentname[1] ?> with other colleagues</h4>

       <table id="recgrid" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Colleague</th>
		<th class="smallish">Tourney</th>
		<th class="smallish">Division</th>
		<th class="smallish">Date</th>
		<th class="smallish">Prelims</th>
		<th class="smallish">Triples</th>
		<th class="smallish">Doubles</th>
		<th class="smallish">Octos</th>
		<th class="smallish">Quarters</th>
		<th class="smallish">Semis</th>
		<th class="smallish">Finals</th>
		<th class="smallish">Total</th>
	</tr>
	</thead>
        <tbody id="myTbodytotals">

<?php

$i=1; $pwin=0; $ploss=0; $trip=""; $doub=""; $octo=""; $qrtr=""; $semi=""; $finl=""; $totwin=0; $totloss=0; $rrwin=0; $rrloss=0;
$totpwin=0; $totploss=0; $totewin=0; $toteloss=0; $jvwin=0; $jvloss=0;
while ($i <= $x) {
if (onlyone($spkr1[$i], $spkr2[$i], $student1, $student2)==TRUE)
{
 if ($win[$i]==1 and $isprelim[$i]==1) {$pwin++;}
 if ($win[$i]==0 and $isprelim[$i]==1) {$ploss++;}
 if ($win[$i]==1 and $isprelim[$i]==0 and $isopen[$i]==1) {$totewin++;}
 if ($win[$i]==0 and $isprelim[$i]==0 and $isopen[$i]==1) {$toteloss++;}
 if ($win[$i]==1 and $isprelim[$i]==0 and $isopen[$i]==0) {$jvwin++;}
 if ($win[$i]==0 and $isprelim[$i]==0 and $isopen[$i]==0) {$jvloss++;}
 if ($win[$i]==1) {$totwin++;}
 if ($win[$i]==0) {$totloss++;}
 if ($round[$i]==11) {$trip=$outcome[$i];}
 if ($round[$i]==12) {$doub=$outcome[$i];}
 if ($round[$i]==13) {$octo=$outcome[$i];}
 if ($round[$i]==14) {$qrtr=$outcome[$i];}
 if ($round[$i]==15) {$semi=$outcome[$i];}
 if ($round[$i]==16) {$finl=$outcome[$i];}
 if (($i<$x and $tourn[$i]<>$tourn[$i+1]) OR $i==$x)
  {
  echo "<tr>";
  $colleaguename="N/A";
  if ($spkr1[$i]<>$student1) {$colleaguename=getname($spkr1[$i]);}
  if ($spkr2[$i]<>$student1) {$colleaguename=getname($spkr2[$i]);}
  echo "<td>".$colleaguename."</td>";
  echo "<td>".$tourn[$i]."</td>";
  echo "<td>".$event[$i]."</td>";
  $date = strtotime($tourndate[$i]);
  echo "<td>".date('d/M/Y', $date)."</td>";
  echo "<td>".$pwin."-".$ploss."</td>";
  echo "<td>".$trip."</td>";
  echo "<td>".$doub."</td>";
  echo "<td>".$octo."</td>";
  echo "<td>".$qrtr."</td>";
  echo "<td>".$semi."</td>";
  echo "<td>".$finl."</td>";
  echo "<td>".$totwin."-".$totloss."</td>";
  echo "</tr>";
  if ($isRR[$i]==1 and $isopen[$i]==1) {$rrwin+=$pwin; $rrloss+=$ploss;}
  if ($isRR[$i]==0 and $isopen[$i]==1) {$totpwin+=$pwin; $totploss+=$ploss;}
  if ($isopen[$i]==0) {$jvwin+=$pwin; $jvloss+=$ploss;}
  $pwin=0; $ploss=0; $trip=""; $doub=""; $octo=""; $qrtr=""; $semi=""; $finl=""; $totwin=0; $totloss=0;
  }
}             // end of big if, only process for exact matches
$i++;
}

?>
	</tbody>
       </table>

<h4><?php echo $studentname[2] ?> with other colleagues</h4>

       <table id="recgrid" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Colleague</th>
		<th class="smallish">Tourney</th>
		<th class="smallish">Division</th>
		<th class="smallish">Date</th>
		<th class="smallish">Prelims</th>
		<th class="smallish">Triples</th>
		<th class="smallish">Doubles</th>
		<th class="smallish">Octos</th>
		<th class="smallish">Quarters</th>
		<th class="smallish">Semis</th>
		<th class="smallish">Finals</th>
		<th class="smallish">Total</th>
	</tr>
	</thead>
        <tbody id="myTbodytotals">

<?php

$i=1; $pwin=0; $ploss=0; $trip=""; $doub=""; $octo=""; $qrtr=""; $semi=""; $finl=""; $totwin=0; $totloss=0; $rrwin=0; $rrloss=0;
$totpwin=0; $totploss=0; $totewin=0; $toteloss=0; $jvwin=0; $jvloss=0;
while ($i <= $x) {
if (onlyone($spkr1[$i], $spkr2[$i], $student2, $student1)==TRUE)
{
 if ($win[$i]==1 and $isprelim[$i]==1) {$pwin++;}
 if ($win[$i]==0 and $isprelim[$i]==1) {$ploss++;}
 if ($win[$i]==1 and $isprelim[$i]==0 and $isopen[$i]==1) {$totewin++;}
 if ($win[$i]==0 and $isprelim[$i]==0 and $isopen[$i]==1) {$toteloss++;}
 if ($win[$i]==1 and $isprelim[$i]==0 and $isopen[$i]==0) {$jvwin++;}
 if ($win[$i]==0 and $isprelim[$i]==0 and $isopen[$i]==0) {$jvloss++;}
 if ($win[$i]==1) {$totwin++;}
 if ($win[$i]==0) {$totloss++;}
 if ($round[$i]==11) {$trip=$outcome[$i];}
 if ($round[$i]==12) {$doub=$outcome[$i];}
 if ($round[$i]==13) {$octo=$outcome[$i];}
 if ($round[$i]==14) {$qrtr=$outcome[$i];}
 if ($round[$i]==15) {$semi=$outcome[$i];}
 if ($round[$i]==16) {$finl=$outcome[$i];}
 if (($i<$x and $tourn[$i]<>$tourn[$i+1]) OR $i==$x)
  {
  echo "<tr>";
  $colleaguename="N/A";
  if ($spkr1[$i]<>$student2) {$colleaguename=getname($spkr1[$i]);}
  if ($spkr2[$i]<>$student2) {$colleaguename=getname($spkr2[$i]);}
  echo "<td>".$colleaguename."</td>";
  echo "<td>".$tourn[$i]."</td>";
  echo "<td>".$event[$i]."</td>";
  $date = strtotime($tourndate[$i]);
  echo "<td>".date('d/M/Y', $date)."</td>";
  echo "<td>".$pwin."-".$ploss."</td>";
  echo "<td>".$trip."</td>";
  echo "<td>".$doub."</td>";
  echo "<td>".$octo."</td>";
  echo "<td>".$qrtr."</td>";
  echo "<td>".$semi."</td>";
  echo "<td>".$finl."</td>";
  echo "<td>".$totwin."-".$totloss."</td>";
  echo "</tr>";
  if ($isRR[$i]==1 and $isopen[$i]==1) {$rrwin+=$pwin; $rrloss+=$ploss;}
  if ($isRR[$i]==0 and $isopen[$i]==1) {$totpwin+=$pwin; $totploss+=$ploss;}
  if ($isopen[$i]==0) {$jvwin+=$pwin; $jvloss+=$ploss;}
  $pwin=0; $ploss=0; $trip=""; $doub=""; $octo=""; $qrtr=""; $semi=""; $finl=""; $totwin=0; $totloss=0;
  }
}             // end of big if, only process for exact matches
$i++;
}

?>
	</tbody>
       </table>

<h4>NUMBER OF PRELIMINARY ROUNDS (a debater may participate in no more than 120 prelim rrounds of debate before the NDT)</h4>

       <table id="recgrid" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Participant(s)</th>
		<th class="smallish">Rounds</th>
	</tr>
	</thead>
        <tbody id="myTbodytotals">

<?php

$i=1; $trds=0; $s1rds=0; $s2rds=0;
while ($i <= $x) {
 if (teammatch($spkr1[$i], $spkr2[$i], $student1, $student2)==TRUE) {$trds++;}
 if ($spkr1[$i]==$student1 or $spkr2[$i]==$student1) {$s1rds++;}
 if ($spkr1[$i]==$student2 or $spkr2[$i]==$student2) {$s2rds++;}
 $i++;
}

  echo "<tr><td>".$teamname."</td><td>".$trds."</td></tr>";
  echo "<tr><td>".$studentname[1]."</td><td>".$s1rds."</td></tr>";
  echo "<tr><td>".$studentname[2]."</td><td>".$s2rds."</td></tr>";

?>
	</tbody>
       </table>

<BR><h2>section III: Total record of debaters</h2>

       <table id="recgrid" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">Participant(s)</th>
		<th class="smallish">Breakdown</th>
		<th class="smallish">Record</th>
		<th class="smallish">Percent</th>
	</tr>
	</thead>
        <tbody id="myTbodytotals">

<?php

$stucount=1;
While ($stucount <= 2)
{

$i=1; $pwin=0; $ploss=0; $totwin=0; $totloss=0; $rrwin=0; $rrloss=0;
$ewin=0; $eloss=0; $jvwin=0; $jvloss=0;
while ($i <= $x) 
{
$match=FALSE;
if ($stucount==1 and ($spkr1[$i]==$student1 or $spkr2[$i]==$student1)) {$match=TRUE;}
if ($stucount==2 and ($spkr1[$i]==$student2 or $spkr2[$i]==$student2)) {$match=TRUE;}
if ($match==TRUE)
 {
  if ($win[$i]==1) 
   {
    if ($isprelim[$i]==1 and $isopen[$i]==1 and $isRR[$i]==0) {$pwin++;}
    if ($isprelim[$i]==0 and $isopen[$i]==1 and $isRR[$i]==0) {$ewin++;}
    if ($isopen[$i]==0) {$jvwin++;}
    if ($isRR[$i]==1 and $isopen[$i]==1) {$rrwin++;}
    $totwin++;
   }
  if ($win[$i]==0) 
   {
    if ($isprelim[$i]==1 and $isopen[$i]==1 and $isRR[$i]==0) {$ploss++;}
    if ($isprelim[$i]==0 and $isopen[$i]==1 and $isRR[$i]==0) {$eloss++;}
    if ($isopen[$i]==0) {$jvloss++;}
    if ($isRR[$i]==1 and $isopen[$i]==1) {$rrloss++;}
    $totloss++;
   }
 }           //ends the student match if
$i++;
}            //ends the while looping through debates

  echo "<tr><td>".$studentname[$stucount]."</td><td>A1. Prelim record</td><td>".$pwin."-".$ploss."</td><td>".getpctstring($pwin,$ploss)."%</td></tr>";
  echo "<tr><td>".$studentname[$stucount]."</td><td>A2. Elim record</td><td>".$ewin."-".$eloss."</td><td>".getpctstring($ewin,$eloss)."%</td></tr>";
  echo "<tr><td>".$studentname[$stucount]."</td><td>B. Round Robins</td><td>".$rrwin."-".$rrloss."</td><td>".getpctstring($rrwin,$rrloss)."%</td></tr>";
  echo "<tr><td>".$studentname[$stucount]."</td><td>C. JV/Novice</td><td>".$jvwin."-".$jvloss."</td><td>".getpctstring($jvwin,$jvloss)."%</td></tr>";
  echo "<tr><td>".$studentname[$stucount]."</td><td>D. Total Record</td><td>".$totwin."-".$totloss."</td><td>".getpctstring($totwin,$totloss)."%</td></tr>";

$stucount++;
}            //  ends the big while looping students

?>

	</tbody>
       </table>

<BR><h2>section IV: INDIVIDUAL TOURNAMENTS</h2>
One asterisk (*) indicates an opponent who cleared.  
Honors are supplied by applicants and do not draw from the database.

<?php 
$loopnum=1; $match=FALSE;
while ($loopnum <=3)
{
if ($loopnum==1) {echo "<h4>INDIVIDUAL TOURNAMENTS AS A TEAM</H4>";}
if ($loopnum==2) {echo "<h4>INDIVIDUAL TOURNAMENTS FOR ".$studentname[1]."</H4>";}
if ($loopnum==3) {echo "<h4>INDIVIDUAL TOURNAMENTS FOR ".$studentname[2]."</H4>";}

?>
       <table id="recgrid" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
        <tbody id="myTbodytotals">
<?php
$i=1; $pwin=0; $ploss=0; $totwin=0; $totloss=0; $rrwin=0; $rrloss=0; $lasttourn=-1;
$ewin=0; $eloss=0; $jvwin=0; $jvloss=0;
while ($i <= $x) 
{
 $match=FALSE;
 if ($loopnum==1 and teammatch($spkr1[$i], $spkr2[$i], $student1, $student2)==TRUE) {$match=TRUE;}
 if ($loopnum==2 and onlyone($spkr1[$i], $spkr2[$i], $student1, $student2)==TRUE) {$match=TRUE;}
 if ($loopnum==3 and onlyone($spkr1[$i], $spkr2[$i], $student2, $student1)==TRUE) {$match=TRUE;}
 if ($match==TRUE)
  {
   if ($tourn[$i] <> $lasttourn) 
    {if ($i>1) {echo "<tr><td>Prelims:</td><td>".$pwin."-".$ploss."</td></tr>";
     echo "<tr><td>Elims:</td><td>".$ewin."-".$eloss."</td></tr>";
     echo "<tr><td>Total:</td><td>".($pwin+$ewin)."-".($ploss+$eloss)."</td><td>".gethonors($student2, $tournid[$i-1])."</td></tr>";}
     echo "<tr><th style='text-align:center' colspan=4><B>".$tourn[$i]."</B></th></tr>";
     echo "<tr><td>Round</td><td>Side</td><td>Outcome</td><td>Opponent</td></tr>";
     $pwin=0; $ploss=0; $ewin=0; $eloss=0;
    }
   echo "<tr><td>".$roundlabel[$i]."</td><td>".getsidestring($side[$i])."</td><td>".$outcome[$i]."</td><td>".$oppn[$i]."</td></tr>";
   if ($isprelim[$i]==1 and $win[$i]==1) {$pwin++;}
   if ($isprelim[$i]==1 and $win[$i]==0) {$ploss++;}
   if ($isprelim[$i]==0 and $win[$i]==1) {$ewin++;}
   if ($isprelim[$i]==0 and $win[$i]==0) {$eloss++;}
   $lasttourn=$tourn[$i];
  }
 $i++;
}           // end of debate loop
  echo "<tr><td>Prelims:</td><td>".$pwin."-".$ploss."</td></tr>";
  echo "<tr><td>Elims:</td><td>".$ewin."-".$eloss."</td></tr>";
  echo "<tr><td>Total:</td><td>".($pwin+$ewin)."-".($ploss+$eloss)."</td><td>".gethonors($student1, $tournid[$i-1]).gethonors($student2, $tournid[$i-1])."</td></tr>";

?>

	</tbody>
       </table>

<?php
$loopnum++;
} // ends the looppnum



//                                                  -------------------------------- CLOSING SECTION AND FUNCTIONS

$time_end = microtime(true);
$time = $time_end - $time_start;
?><hr/><?php

echo "Total load time is $time seconds\n";

mysql_close();
require 'scripts/tabroomfooter.html';

function microtime_float()
{
    list($usec, $sec) = explode(" ", microtime());
    return ((float)$usec + (float)$sec);
}

function gethonors($studentid, $tournid)
{
$query="SELECT * FROM result, round, event, tourn where student=".$studentid." and result.round=round.id and event.id=round.event and event.tourn=tourn.id and tourn.id=".$tournid;
$rtnvalue="";
$spkrs=mysql_query($query); 
   while ($row = mysql_fetch_array($spkrs, MYSQL_BOTH)) 
   {$rtnvalue.=$row['honor'];}
if ($rtnvalue<>'') {return "Honors: ".getname($studentid)." ".$rtnvalue.". ";}
return '';
}

function getpctstring ($win, $loss)
{
 $winpct=0;
 if ($win+$loss>0) {$winpct=number_format($win/($win+$loss)*100,2);}
 return $winpct;
}

function makeoutcomestring ($balfor, $balvs)
{
 $outcome=""; 
 for ($i=1; $i <= $balfor; $i++) {$outcome.="W";}
 for ($i=1; $i <= $balvs; $i++) {$outcome.="L";}
 return $outcome;
}

function DivisionIsOpen($event)
{
$rtnvalue=FALSE;
$query="SELECT * FROM event_setting WHERE tag='Level' and event_setting.event=".$event;
$spkrs=mysql_query($query); 
   while ($row = mysql_fetch_array($spkrs, MYSQL_BOTH)) 
   {if ($row['value']=='open') {$rtnvalue=TRUE;}}
return $rtnvalue;
}

function getspeakers (&$spkr1, &$spkr2, $entry)
{
$i=0;
$query="SELECT *, entry_student.student as entry_student_speakerID FROM entry_student, entry WHERE entry_student.entry=entry.id and entry.id=".$entry;
$spkrs=mysql_query($query); 
   while ($row = mysql_fetch_array($spkrs, MYSQL_BOTH)) 
   {
    $i++;
    if ($i==1) {$spkr1=$row['entry_student_speakerID'];}
    if ($i==2) {$spkr2=$row['entry_student_speakerID'];}
   }
}

function teammatch ($spkr1, $spkr2, $student1, $student2)
{
 $match=FALSE;
 if (($spkr1==$student1 or $spkr1==$student2) AND ($spkr2==$student1 or $spkr2==$student2)) {$match=TRUE;}
 return $match;
}

function onlyone ($spkr1, $spkr2, $student1, $student2)
//tests to see if the team has $student1 but not $student2
{
 $match=FALSE;
 if ($student2<>$spkr1 and $student2<>$spkr2) {$match=TRUE;}
 return $match;
}

function getname($studentID)
{
$studentname="N/A";
$query="SELECT * FROM student where student.id=".$studentID;
$ballots=mysql_query($query); 
   while ($row = mysql_fetch_array($ballots, MYSQL_BOTH)) 
   {$studentname=$row['first']." ".$row['last'];}
return $studentname;
}

function getopponent($panel, $entry)  //pulls all entries on the panel that are not the one received in $entry variable
{
$ctr=0;
$teamname="";$schoolname="";$entryID=0;
$query="SELECT *, chapter.name as chapter_name, entry.id as entry_id FROM entry, ballot, entry_student, student, chapter WHERE chapter.id=student.chapter and student.id=entry_student.student and entry_student.entry=entry.id and ballot.panel=".$panel." and ballot.entry<>".$entry." and entry.id=ballot.entry";
$student=mysql_query($query); 
   while ($row = mysql_fetch_array($student, MYSQL_BOTH)) 
   {
    $ctr++;
    if ($ctr<=2)
     {
      $schoolname=$row['chapter_name'];
      if ($teamname <> "") {$teamname .= " and ";}
      $teamname .= $row['first']." ".$row['last'];
      $entryID=$row['entry_id'];
     }
   }
 if ($schoolname=="") {return "Bye or closeout";}
 return $schoolname." -- ".$teamname.didclear($entryID);
 
}

function didclear($entry)
{
$returnstring="";
$query="SELECT *, round.name as round_name FROM ballot, entry, round, panel WHERE ballot.entry=".$entry." and entry.id=ballot.entry and panel.id=ballot.panel and round.id=panel.round";
$debates=mysql_query($query); 
   while ($row = mysql_fetch_array($debates, MYSQL_BOTH)) 
   {
    if ($row['round_name'] >9) {$returnstring="*";}
   }
 return $returnstring;
}

function getsidestring($side)
{
 $sidestr="";
 if ($side==1) {$sidestr="Aff";}
 if ($side==2) {$sidestr="Neg";}
 return $sidestr;
}
?>
