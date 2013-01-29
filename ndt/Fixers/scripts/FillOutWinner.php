<?php
//takes a panel, 

require 'databaseconnect.php';

$panel=$_POST['panel'];
$entry=$_POST['entry'];

$savequery="";

//Now loops all 3 ballots, checks for a ballot_value, and adds one if it can't find it

   $query="SELECT * FROM ballot WHERE ballot.panel=".$panel." and entry=".$entry;
   $decision=mysql_query($query);
   while ($row = mysql_fetch_array($decision, MYSQL_BOTH)) 
    {
    $query2="SELECT * FROM ballot_value WHERE ballot=".$row['id'];
    $balvalue=mysql_query($query2);
    $ballotNum = mysql_num_rows($balvalue);
    if ($ballotNum==0)
      {
       $savequery="INSERT INTO ballot_value (tag, ballot, value) VALUES ('ballot',".$row['id'].",1)";
       //echo $savequery;
       mysql_query($savequery);
      }
    }


mysql_close();

header('Location: https://www.tabroom.com/jbruschke/FixerViewPanel.php?panel='.$panel);

?>