function getjudgename($judge, $njudge)
{
   $judgename="";
   $query="SELECT * FROM judge WHERE id=".$judge;
   $dude=mysql_query($query);
   while ($row = mysql_fetch_array($dude, MYSQL_BOTH)) 
     {$judgename=$row['first']." ".$row['last'];}
   if ($njudge>0) {$judgename=", ".$judgename;}
   return $judgename;
}
