<?php

require 'scripts/databaseconnect.php';

$panel=$_POST['panel'];
$event=$_POST['event'];
echo $event."<br>";

$flipquery="";

   $query="SELECT * FROM ballot WHERE ballot.panel=".$panel;
   echo $query."<br>";
   $decision=mysql_query($query);
   while ($row = mysql_fetch_array($decision, MYSQL_BOTH)) 
    {
    if ($row['side']==1) {$flipquery="Update ballot set side=1 where ballot.id=".$row['id'];}
    if ($row['side']==2) {$flipquery="Update ballot set side=2 where ballot.id=".$row['id'];}
    mysql_query($flipquery);
    }

mysql_close();

header('Location: https://www.tabroom.com/jbruschke/UglyResultDisplay.php?event='.$event);

?>