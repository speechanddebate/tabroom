<?php                    //This makes cume sheets
$time_start = microtime(true);
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

$query="select tourn from timeslot where timeslot.id=".$_GET['timeslot']." limit 1"; 
$result = mysql_query($query) or die(); 
$row = mysql_fetch_object($result);
$tourn = $row->tourn; 

?>
<h2>ballots still out</h2>
<table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">

<tr>
<th>Judge</th>
<th>room</th>
</tr>

<?php

$query="SELECT distinct judge, room, ballot.id as ballot_id FROM panel, ballot, round where ballot.panel=panel.id and panel.round=round.id and round.timeslot=".$_GET['timeslot']." order by room asc";
$ballot=mysql_query($query); $judge=-99;
$x=0;
while ($row = mysql_fetch_array($ballot, MYSQL_BOTH)) 
 {
   if (ballotin($row['ballot_id'])=="FALSE" and $judge<>$row['judge']) 
    {
    echo "<tr><td>".getjudgename($row['judge'])."</td><td>".getroomname($row['room'])."</td></tr>";
    $x++;
    }
   $judge=$row['judge'];
 }

echo $x." ballots out.";
?>
</table>

<?php
mysql_close();

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

function getroomname($room)
{
   $roomname="";
   $query="SELECT * FROM room WHERE id=".$room;
   $dude=mysql_query($query);
   while ($row = mysql_fetch_array($dude, MYSQL_BOTH)) 
     {$roomname=$row['name'];}
   return $roomname;
}

function ballotin($ballot)
{
   $query="SELECT * FROM ballot_value WHERE ballot=".$ballot;
   $dude=mysql_query($query);
   while ($row = mysql_fetch_array($dude, MYSQL_BOTH)) 
     {
     if ($row['value']>0) {return "TRUE";}
     }
   return "FALSE";
}


$time_end = microtime(true);
$time = $time_end - $time_start;
echo "Total load time is $time seconds\n";
?>

</body>
</html>

