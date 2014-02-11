#!/bin/bash
ln -s /bin/chmod /usr/bin/chmod
rsync -avzru amazon@azuen.net:/home/palmer/Dropbox/Tech/Backup/files /www/
rsync -avzru /www/files amazon@azuen.net:/home/palmer/Dropbox/Tech/Backup/
chown -R www-data.www.data /www/files


