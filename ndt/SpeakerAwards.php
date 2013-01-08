<?php

require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$event=$_GET['event'];

//TOURN NAME
$query="SELECT *, tourn.name as tourn_name, event.name as event_name from tourn, event where event.id=".$event." and tourn.id=event.tourn";
$tourn=mysql_query($query);
while ($row = mysql_fetch_array($tourn, MYSQL_BOTH)) 
 {$tournname=$row['tourn_name']; $eventname=$row['event_name'];}

//PULL THE TIEBREAKERS
$query="SELECT * from tiebreak where tiebreak.tb_set=".gettbset($event)." order by priority asc";
$tb=mysql_query($query);
$ntb= mysql_num_rows($tb); 
if ($ntb<1) 
 {echo "Speaker tiebreakers do not appear to be entered for this tournament.<br><br>"; 
  echo "<a href='https://www.tabroom.com/jbruschke/SpeakerAwardHelp.php'>Click here for help on missing tiebreakers</a>";
  die;
 }

//you've now got all the tiebreakers stored in a mysql_fetch_array
//while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
// {echo $row['name']." ". $row['tb']."<br>";}

//PULL THE SPEAKERS
$query="SELECT * from entry, entry_student, student where entry.event=".$event." and entry_student.entry=entry.id and student.id=entry_student.student";
$spkr=mysql_query($query);
$nspkrs= mysql_num_rows($spkr);
//you've now got all the speakers stored in a mysql_fetch_array

$tbtable = array(); //array to hold speakers; $tbtable[tiebreaker][speaker], 0=name, 1=school, 2=entry, 3+ = tbreaks
$avgjudpts = array(); //holds avg pts for each judge; 0=judge, 1=avg pts
makeavgpts(&$avgjudpts, $event);

$i=0;$j=0;
while ($row = mysql_fetch_array($spkr, MYSQL_BOTH)) 
 {$i++;
  $tbtable[0][$i]=$row['last'].", ". $row['first'];
  $tbtable[1][$i]=$row['chapter'];
  $tbtable[2][$i]=$row['entry'];
  $j=2;
  mysql_data_seek($tb,0);
  while ($row2 = mysql_fetch_array($tb, MYSQL_BOTH)) 
   {$j++;
    $tbtable[$j][$i]=getscore($row['student'], $row['entry'], $row2['name'], $row2['highlow']);
   }
 }

//sort
// $dummy='$tbtable[3],SORT_DESC, $tbtable[4]';
 $dummy="";
 $dummy=makesortstring($tb);
 $sort="array_multisort(".$dummy.");"; 
  eval($sort); 
?>

<h2><center>
<?php echo $tournname." - ".$eventname." - "; ?>
Speakers in Order</center></h2>
<a href='https://www.tabroom.com/jbruschke/SpeakerAwardHelp.php'>Click if these don't seem to be correct</a> <a href='https://www.tabroom.com/jbruschke/TeamResults.php'>Return to main results page</a></br></br>
<?php
//<span style="font-size:large;"><a href='https://www.tabroom.com/jbruschke/SpeakerAwardHelp.php'>Click if these don't seem to be correct</a>
//</span>
?>

	<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
	<thead>
	<tr class="yellowrow">
		<th class="smallish">Place</th>
		<th class="smallish">Speaker</th>
		<th class="smallish">School</th>
<?php
        mysql_data_seek($tb,0);
        while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
         {echo "<th>".$row['name']." ". $row['highlow']." HL drop(s)</th>";}
?>
	</tr>
	</thead>
	<tbody>
<?php

$i=-1;
while ($i<$nspkrs-1)
 {
  $i++;
  echo "<tr>";
  if ($i<($nspkrs/2)) {echo "<td>".($i+1)."</td>";} else {echo "<td>-</td>";}
  echo "<td>".$tbtable[0][$i]."</td>";
  echo "<td>".getschoolname($tbtable[1][$i])."</td>";
        mysql_data_seek($tb,0);
        $j=2;
        while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
         {$j++;
          echo "<td>".number_format($tbtable[$j][$i], 2, '.', '')."</td>";
         }
  echo "</tr>";
 }
?>

	</tbody>
       </table>


<?php

mysql_close();
require 'scripts/tabroomfooter.html';

function makeavgpts(&$avgjudpts, $event)
{
 $query="SELECT *, judge.id as judge_id from judge, event where event.id=".$event." and judge.judge_group=event.judge_group";
 $judge=mysql_query($query);
 $i=0;
  while ($row = mysql_fetch_array($judge, MYSQL_BOTH)) 
     {
      $i++; 
      $avgjudpts[$i]['judge']=$row['judge_id'];
      $query2="SELECT * from ballot, ballot_value where ballot.judge=".$row['judge_id']." and ballot_value.ballot=ballot.id and tag='points' and ballot_value.value>0";
      $pts=mysql_query($query2); $totpts=0;
      $nrows= mysql_num_rows($pts);
      while ($row2 = mysql_fetch_array($pts, MYSQL_BOTH)) 
        {$totpts=$totpts+$row2['value'];}
        $avgjudpts[$i]['avg']=0;
        if ($nrows>0) {$avgjudpts[$i]['avg']=$totpts/$nrows;}
      //if ($row['judge_id']==117213) {echo $totpts." ".$nrows."<br>";}
     }
//$x=0; while ($x<$i) {echo $avgjudpts[$x]['judge']." ".$avgjudpts[$x]['avg']."<br>"; $x++;}
//var_dump($avgjudpts);  
}

function gettbset($event)
{
   $tbset=0;
   $query="SELECT * from event_setting where event_setting.event=".$event." and tag='speaker_tbset'";
   $round=mysql_query($query);
    while ($row = mysql_fetch_array($round, MYSQL_BOTH)) 
     {$tbset=$row['value'];
     }
    return $tbset;
}

function getscore($studententry, $entry, $scoretype, $hilo)
{
   //echo $studententry." ".$entry." ".$scoretype." ".$hilo."<br>";
   $dojudgevar=FALSE;
   if ($scoretype=="judgevar") {$dojudgevar=TRUE; $scoretype="points";}
   $score=0;
   if ($scoretype=='ranks') {$scoretype='rank';}
   $query="SELECT *, round.name as round_name from ballot, ballot_value, panel, round where round.id=panel.round and panel.id=ballot.panel and ballot.entry=".$entry." and ballot_value.ballot=ballot.id and tag='".$scoretype."' and student=".$studententry." and round.name<10 and round.post_results>1 order by ballot_value.value asc";
   $pts=mysql_query($query);
   $nrows= mysql_num_rows($pts); //echo $nrows." rows returned<br>";
   $i=0; $scorevalue=0;
    while ($row = mysql_fetch_array($pts, MYSQL_BOTH)) 
     {
      $i++;
      $scorevalue=$row['value']; 
      if ($dojudgevar==TRUE) {$scorevalue=getjudvar($row['judge'], $scorevalue);}
      if ($scorevalue == -1) {$scorevalue=getavg($studententry, $entry, $scoretype);}
      //if ($studententry == 105520) {echo $studententry." ".$scorevalue." ". $row['round_name']."<br>";}
      if ($i>$hilo and $i<=($nrows-$hilo)) {$score=$score+$scorevalue;}
     }
   if (empty($score)) {$score=0;}
   return $score;
}

function getjudvar($judge, $pts)
{
if ($pts==0) {return 0;}
global $avgjudpts;
$rows = count($avgjudpts,0);
$i=0;
while ($i<$rows)
 {
  $i++; 
  if ($avgjudpts[$i]['judge']==$judge) {return $pts-$avgjudpts[$i]['avg']; echo $avgjudpts[$i]['judge']." ".$pts." ".$avgjudpts[$i]['avg']."<br>";
                                       }
 }
return 0;
}

function getavg($studententry, $entry, $scoretype)
{
   $avg=0;
   $query="SELECT * from ballot, ballot_value where ballot.entry=".$entry." and ballot_value.ballot=ballot.id and tag='".$scoretype."' and student=".$studententry." and ballot_value.value<>-1 order by ballot_value.value asc";
   $pts=mysql_query($query);
   $nrows= mysql_num_rows($pts); //echo $nrows." rows returned<br>";
   $total=0;
    while ($row = mysql_fetch_array($pts, MYSQL_BOTH)) 
     {
      $total=$total+$row['value'];
     }
   return $total/$nrows;
}

function makesortstring($tb)
{
 $dummy="";
 mysql_data_seek($tb,0);
 $i=2;
 while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
  {
  $i++;
  $dummy.='$tbtable['.$i.'],';
  if ($row['name']=="ranks") {$dummy.="SORT_ASC,";}
  if ($row['name']=="points") {$dummy.="SORT_DESC,";}
  }
$dummy.='$tbtable[0],$tbtable[1],$tbtable[2]';
return $dummy;
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


?>