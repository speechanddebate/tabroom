#!/bin/bash

mysql --silent itab < add-settings.sql

./convert-database.pl

mysql --silent itab < post-settings.sql

