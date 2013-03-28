<?php
require 'scripts/tabroomtemplate.html';
require 'scripts/databaseconnect.php';

?>

<!--
<form action="PublicBallotTable.php" method="get">
  Round ID: <input type="text" size="30" name="RoundID" />
  <input type="submit" value="Show Round"/>
</form>
-->

       <table id="sortme" class="hovertable sortable" border="2" cellspacing="2" cellpadding="2">
       <thead>
	<tr class="yellowrow">
		<th class="smallish">First</th>
		<th class="smallish">Last</th>
		<th class="smallish">School</th>
		<th class="smallish">Cell</th>
		<th class="smallish">email</th>
	</tr>
	</thead>

<?php
$query="SELECT * FROM judge, account, judge_group, school WHERE school.id=judge.school and account.id=judge.account and judge_group.id=judge.judge_group and judge_group.tourn=1518";
$tourn=mysql_query($query);
$entryNum = mysql_num_rows($tourn);
$stillout=0; $alreadyin=0;

for ($i=0; $i <= $entryNum-1; $i++)
{
echo "<tr>";
echo "<td>".mysql_result($tourn,$i,"first")."</td>";
echo "<td>".mysql_result($tourn,$i,"last")."</td>";
echo "<td>".mysql_result($tourn,$i,"school.name")."</td>";
echo "<td>".mysql_result($tourn,$i,"phone")."</td>";
echo "<td>".mysql_result($tourn,$i,"email")."</td>";
echo "</tr>";
}

?>

    </tbody></table>

<? 
function ballotsout($panel, &$stillout, &$alreadyin)
{
$query="SELECT distinct judge FROM ballot where ballot.panel=".$panel;
$tourn=mysql_query($query);
$Nballots = mysql_num_rows($tourn);

$query="SELECT * FROM ballot, ballot_value WHERE ballot_value.ballot=ballot.id and ballot_value.tag='ballot' and ballot_value.value=1 and ballot.panel=".$panel;
//echo $query;
$tourn=mysql_query($query);
$alreadyin = mysql_num_rows($tourn);

$stillout=$Nballots-$alreadyin; 
}

mysql_close(); 
?>