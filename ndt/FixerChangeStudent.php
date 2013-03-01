<?php
require 'scripts/databaseconnect.php';

$entryid=$_POST['entryid'];
$schoolid=$_POST['schoolid'];
$oldid=$_POST['oldid'];
$newid=$_POST['newid'];

//$query="UPDATE ballot_value SET ballot_value.student=".$newid." WHERE ballot_value.student=".$oldid;
//$query2="UPDATE entry_student SET entry_student.student=".$newid." WHERE entry_student.student=".$oldid;

if ($entryid>0)
{
//update the entry
$query2="UPDATE entry_student SET entry_student.student=".$newid." WHERE entry_student.student=".$oldid." and entry_student.entry=".$entryid;
echo $query2."<br>";
mysql_query($query2);

//update the school
 if ($schoolid>0)
 {
   $query3="UPDATE entry set school=".$schoolid." WHERE id=".$entryid;
   mysql_query($query3);
   echo $query3."<br>";
 }

//Now load and change all the ballot_values
$query="Select * from ballot_value, ballot where ballot_value.student=".$oldid." and ballot.id=ballot_value.ballot and ballot.entry=".$entryid;
echo $query."<br>";
$ballot_value=mysql_query($query);
$entryNum = mysql_num_rows($ballot_value);

for ($i=0; $i <= $entryNum-1; $i++)
{
$query3="UPDATE ballot_value set student=".$newid." WHERE id=".mysql_result($ballot_value,$i,"ballot_value.id");
echo $query3."<br>";
mysql_query($query3);
}

//delete if necessary
if (isset($_POST['DeleteOld']))
 {$deleteold=$_POST['DeleteOld'];
  if ($deleteold="TRUE")
   {
    $query3="DELETE from student where ID=".$oldid;
    echo $query3."<br>";
    mysql_query($query3);
   }
 }

echo "QUERIES PROCESSED.<BR>";
}


?>

<br><hr>ENTER THE NAMES TO CHANGE 
This will replace one student as entered in a tournament with another. 
It will also change all results for that student.

<form action="FixerChangeStudent.php" method="post">
 <p>Entry ID: <input type="text" size="50" name="entryid"></input></p>
 <p>New school/chapter ID for ENTRY: <input type="text" size="50" name="schoolid"></input></p>
 <p>Student to remove: <input type="text" size="50" name="oldid"></input></p>
 <p>Replace removed student with this one: <input type="text" size="50" name="newid"></input></p>
 <p>Delete old student ID?<input type="checkbox" name="DeleteOld" value="TRUE"  /><br />
 <p><input type="submit" /></p>
</form>

<?php

mysql_close();

?>