#!/bin/bash

rm /tmp/itab-backup.sql

echo "Accessing latest data dump"

cp ~/Dropbox/DebateTech/Backup/itab-backup.sql.bz2 /tmp

echo "Decompressing data dump"

bunzip2 /tmp/itab-backup.sql.bz2

echo "Dropping old database"

/usr/bin/mysqladmin -u root -f drop itab

echo "Creating new blank database"

/usr/bin/mysqladmin -u root create itab

echo "Loading datafile"

/usr/bin/mysql -u root -f itab < /tmp/itab-backup.sql

echo "Removing data dump"

rm /tmp/itab-backup.sql

echo "All set!"


