Takes a panel and a team, deletes all existing info for that panel and gives the team a 3-0 decision<br>
<form action="scripts/Give30Win.php" method="post">
  Panel: <input type="text" size="30" name="panel" /><br>
  Entry/winner: <input type="text" size="30" name="entry" /><br>
  Is a bye (0 for judge unknown, -1 for bye): <input type="text" size="30" name="isbye" /><br>
  <input type="submit" value="Give a 3-0 win"/>
</form>

<br/>
<a href="FixMaster.php">Main Menu</a></br>

