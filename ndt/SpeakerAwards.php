<?php

require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$event=$_GET['event'];

//PULL THE TIEBREAKERS
$query="SELECT * from tiebreak where tiebreak.tb_set=".gettbset($event)." order by priority asc";
$tb=mysql_query($query);
$ntb= mysql_num_rows($tb);
//you've now got all the tiebreakers stored in a mysql_fetch_array
//while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
// {echo $row['name']." ". $row['tb']."<br>";}

//PULL THE SPEAKERS
$query="SELECT * from entry, entry_student, student where entry.event=".$event." and entry_student.entry=entry.id and student.id=entry_student.student";
$spkr=mysql_query($query);
$nspkrs= mysql_num_rows($spkr);
//you've now got all the speakers stored in a mysql_fetch_array

$tbtable = array(); //array to hold speakers; $tbtable[tiebreaker][speaker], 0=name, 1=school, 2=entry, 3+ = tbreaks

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

<h2>Speakers</h2>
	<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
	<thead>
	<tr class="yellowrow">
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
  echo "<td>".$tbtable[0][$i]."</td>";
  echo "<td>".getschoolname($tbtable[1][$i])."</td>";
        mysql_data_seek($tb,0);
        $j=2;
        while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
         {$j++;
          echo "<td>".$tbtable[$j][$i]."</td>";
         }
  echo "</tr>";
 }
?>

	</tbody>
       </table>


<?php

mysql_close();

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
   $score=0;
   if ($scoretype=='ranks') {$scoretype='rank';}
   $query="SELECT * from ballot, ballot_value where ballot.entry=".$entry." and ballot_value.ballot=ballot.id and tag='".$scoretype."' and student=".$studententry." and ballot_value.value>0 order by ballot_value.value asc";
   $pts=mysql_query($query);
   $nrows= mysql_num_rows($pts); //echo $nrows." rows returned<br>";
   $i=0;
    while ($row = mysql_fetch_array($pts, MYSQL_BOTH)) 
     {
      $i++;
      if ($i>$hilo and $i<=($nrows-$hilo)) {$score=$score+$row['value'];}
     }
   if (empty($score)) {$score=0;}
   return $score;
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