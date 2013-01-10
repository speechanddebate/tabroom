<?php

require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';
?>

If the speaker awards are not appearing in the correct order, it is more than likely the case that he tiebreakers in iDebateTabroom 
are not the same as those used in the offline tabulation software.  The fix is quite easy, and can be done by the tournament director.</br></br>

If the problem does not seem to be the tiebreakers, but appears to be a calculation problem, email me at jbruschke@gmail.com</br></br>

<a href='https://www.tabroom.com/jbruschke/TeamResults.php'>Return to Results Dashboard</a> (Use the back button on your
browser to return to the speaker awards page)</br></br>
IF YOU ARE A PARTICIPANT: Contact the tournament director about the issue, and direct them to this page.</br></br>

IF YOU ARE THE DIRECTOR:</br></br>

1) Log in using the link in the top-right, and click the link to go to your homepage (also top-right).</br>
2) Under the "tournaments" box in the right-hand column, click the name of your tournament.  If your tournament doesn't appear, 
click "See Past Tournaments" and a list will appear.  Click on the name of the tournament in question.  You are now on the 
Director's Page.</br>
3) Click on Settings->Tournament Rules</br>
4) In the right-hand column will be a link for "Tiebreaks for Speaker Awards."  Click on it.</br>
5) If a speaker tiebreaker exists, it will appear in the right-hand column, and you should click on it.  If no set exists, you will be promopted to create a tiebreaker set.  Enter a name (something like "speaker awards") and
click the green "create" button.</br>
6) You can now enter the tiebreakers in any order you want, or re-name the tiebreaker set.</br>
7) Click on Settings->Events, find the setting for "Speaker Award Tiebreakers," and select the tiebreaker you just created from the drop-down list.  Remember to hit the green "Save Event" button on the bottom of the page when you're done.</br>
8) Finally, make sure you have published the speaker points.  The easiest way to do this is paneling->publish.</br></br>

More detailed descriptions about tiebreakers, and why they exist in sets, can be found by clicking the help button on the CAT tiebreakers
setup page.</br></br>

You can return to the tiebreaker set at any time by following the Director's Page->Settings->Tournament Rules sequence,
clicking on "tiebreakers for placement" in the right-hand column, and then clicking on the tiebreaker set by name (it
will appear in the right-hand column).</br></br>

<a href='https://www.tabroom.com/jbruschke/TeamResults.php'>Return to Results Dashboard</a>

<?php
require 'scripts/tabroomfooter.html';
?>