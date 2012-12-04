<?php

require 'databaseconnect.php';

$panel=$_POST['panel'];
$judge=$_POST['judge'];
$event=$_POST['event'];
echo $event."<br>";

$flipquery="";

   $query="SELECT *, ballot_value.id as ballot_value_id, ballot_value.value as ballot_value_value FROM ballot_value, ballot WHERE ballot.panel=".$panel." and ballot.judge=".$judge." and ballot_value.ballot=ballot.id and ballot_value.tag='ballot'";
   echo $query."<br>";
   $decision=mysql_query($query);
   while ($row = mysql_fetch_array($decision, MYSQL_BOTH)) 
    {
    if ($row['ballot_value_value']==1) {$flipquery="Update ballot_value set value=0 where ballot_value.id=".$row['ballot_value_id'];}
    if ($row['ballot_value_value']==0) {$flipquery="Update ballot_value set value=1 where ballot_value.id=".$row['ballot_value_id'];}
    mysql_query($flipquery);
    }

mysql_close();

header('Location: https://www.tabroom.com/jbruschke/FixerUglyResultDisplay.php?event='.$event);

?>