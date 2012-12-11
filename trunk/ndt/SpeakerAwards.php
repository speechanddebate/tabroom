<?php

//pull event, then round, then tiebreak_set, then tiebreak
//this means you need to find the last prelim round

require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$event=$_GET['event'];

//PULL THE TIEBREAKERS
$query="SELECT * from tiebreak where tiebreak.tb_set=".gettbset($event)." order by priority asc";
$tb=mysql_query($query);
//you've now got all the tiebreakers stored in a mysql_fetch_array
//while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
// {echo $row['name']." ". $row['tb']."<br>";}

//PULL THE SPEAKERS
$query="SELECT * from entry, entry_student, student where entry.event=".$event." and entry_student.entry=entry.id and student.id=entry_student.student";
$spkr=mysql_query($query);
$nspkrs= mysql_num_rows($spkr);
//you've now got all the speakers stored in a mysql_fetch_array

$tbtable = array(); //array to hold speakers; $tbtable[0][0] is $tbtable[speaker][tiebreaker score] 0=name, 1=school, 2=entry, 3+ = tiebreaker scores

$i=0;$j=0;
while ($row = mysql_fetch_array($spkr, MYSQL_BOTH)) 
 {$i++;
  $tbtable[$i]['name']=$row['last']." ". $row['first'];
  $tbtable[$i][1]=$row['chapter'];
  $tbtable[$i][2]=$row['entry'];
  $j=2;
  mysql_data_seek($tb,0);
  while ($row2 = mysql_fetch_array($tb, MYSQL_BOTH)) 
   {$j++;
    $tbtable[$i][$j]=getscore($row['student'], $row['entry'], $row2['name'], $row2['highlow']);
   }
 }

//sort
sort_by_key($tbtable, 'name')

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

$i=0;
while ($i<$nspkrs)
 {
  $i++;
  echo "<tr>";
  echo "<td>".$tbtable[$i]['name']."</td>";
  echo "<td>".$tbtable[$i][1]."</td>";
        mysql_data_seek($tb,0);
        $j=2;
        while ($row = mysql_fetch_array($tb, MYSQL_BOTH)) 
         {$j++;
          echo "<td>".$tbtable[$i][$j]."</td>";
         }
  echo "</tr>";
 }
?>

	</tbody>
       </table>


<?php

mysql_close();

function getlastprelim($event)
{
   $lastprelim=0;
   $query="SELECT *, round.id as round_id from round, timeslot where round.event=".$event." and round.type<>'elim' and timeslot.id=round.timeslot order by timeslot.start";
   $round=mysql_query($query);
    while ($row = mysql_fetch_array($round, MYSQL_BOTH)) 
     {$lastprelim=$row['round_id'];
     }
    return $lastprelim;
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
   $score=0;
   $query="SELECT * from ballot, ballot_value where ballot.entry=".$entry." and ballot_value.ballot=ballot.id and tag='".$scoretype."' and student=".$studententry." and ballot_value.value>0 order by ballot_value.value asc";
   $pts=mysql_query($query);
   $nrows= mysql_num_rows($pts); //echo $nrows." rows returned<br>";
   $i=0;
    while ($row = mysql_fetch_array($pts, MYSQL_BOTH)) 
     {
      $i++;
      if ($i>$hilo and $i<=($nrows-$hilo)) {$score=$score+$row['value'];}
     }
   return $score;
}

function sort_by_key ($arr,$key) { 
    global $key2sort; 
    $key2sort = $key; 
    uasort($arr, 'sbk'); 
    return ($arr); 
} 
function sbk ($a, $b) {global $key2sort; return (strcasecmp ($a[$key2sort],$b[$key2sort]));} 

?>