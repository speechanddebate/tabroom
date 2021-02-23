#!/bin/bash

mysql -f --silent itab < add-settings.sql

./convert-database.pl
./convert-ballots.pl

mysql -f --silent itab < post-settings.sql

