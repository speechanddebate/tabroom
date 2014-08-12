<?php

  mysql_connect('localhost', 'jbruschke', 'Oof9iyeeGh9jeeg')
    or die("<p>Error connecting to database: " . mysql_error() . "</p>");

  echo "<p>Connected to MySQL!</p>";

  mysql_select_db("")
    or die("<p>Error selecting the database " . 'TourneyData' . mysql_error() . "</p>");

  echo "<p>Connected to MySQL, using database " . 'TourneyData' . ".</p>";

  $result = mysql_query("SHOW TABLES;");

  if (!$result) {
    die("<p>Error in listing tables: " . mysql_error() . "</p>");
  }

  echo "<p>Tables in database:</p>";
  echo "<ul>";
  while ($row = mysql_fetch_row($result)) {
    echo "<li>Table: {$row[0]}</li>";
  }
  echo "</ul>";

?>
