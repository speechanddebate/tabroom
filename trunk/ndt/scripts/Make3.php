<?php
//takes a panel, 
//check that there are 3 ballots; duplicate the existing ballot if not
//check for ballot_value for all ballots; add one as a win if not

require 'databaseconnect.php';

$panel=$_POST['panel'];

$savequery="";
$entry=0;

//read in all ballots, stop if anything other than 1 ballot

   $query="SELECT * FROM ballot WHERE ballot.panel=".$panel;
   $decision=mysql_query($query);
   $entryNum = mysql_num_rows($decision);
   if ($entryNum<>1) {die("This only works for panels with 1 and only 1 ballot<br>");}

//it's a loop, but it really just reads the only row and assigns a value to the entry
   while ($row = mysql_fetch_array($decision, MYSQL_BOTH)) 
    {
    $entry=$row['entry'];
    }

//creates 2 new ballots
for($i = 0; $i < 2; ++$i) 
 {
    $savequery="INSERT INTO ballot (panel, entry, side) VALUES (".$panel.",".$entry.",1)";
    //echo $savequery;
    mysql_query($savequery);
 }


mysql_close();

header('Location: https://www.tabroom.com/jbruschke/scripts/AddDecisionsByPanel.php?panel='.$panel);

?>