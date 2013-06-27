<?php die; ?>
<a href="FixerElimByeAutofixer.php">Elim byes</a>  (Create panel for a bye if there isn't one, make sure there are 3 ballots there for the bye) </br>
<br/>
<a href="FixerMakePanel.php">Create a panel and add entries to it</a>  (Create a new panel for a closeout) </br>
<br/>
<a href="FixerMarkDivLevels.php">Mark division levels</a></br>
<br/>
<a href="FixerResults.php">Add a ballot_value to an elim ballot that doesn't have one</a></br>
<br/>
<a href="FixerUglyBallotEditor.php">Change the ballot_value on a ballot</a></br>
<br/>
<a href="FixerUglyResultDisplay.php">Flip sides or decisions/run custom query</a></br>
<br/>
<a href="FixerShowAllRounds.php">Show all rounds</a></br>
<br/>
<a href="FixerChangeStudent.php">Replace false student record with an actual student</a></br>
<br/>
<a href="FixerGive30Win.php">Just give a team a 3-0 bye</a></br>
<br/>
<form action="FixerViewPanel.php" method="get">
Enter panel number to view: <input type="text" size="30" name="panel" />
  <input type="submit" value="View panel"/>
</form>
</br>
<form action="scripts/DoSQLUpdate.php" method="post">
 Run this SQL query: <input type="text" size="50" name="querystring" />
 <input type="submit" value="Run query"/>
</form>
<form action="FixerViewRounds.php" method="get">
 Enter event ID: <input type="text" size="50" name="event" />
 <input type="submit" value="Show rounds"/>
</form>
