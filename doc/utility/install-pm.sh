#/bin/bash

apt update
apt -y install nodejs npm

cd /www/tabroom/api
npm install

mkdir -p /www/tabroom/api/.pm2
mkdir -p /var/run/pm2/
mkdir -p /var/log/indexcards/

chown -R tabroom /www/tabroom/api/.pm2
chown -R tabroom /var/run/pm2
chown -R tabroom /var/log/indexcards

npm install -g pm2
pm2 install pm2-logrotate

pm2 set pm2-logrotate:max_size 1G
pm2 set pm2-logrotate:compress true
pm2 set pm2-logrotate:rotateInterval '0 0 * * *'

scp freyr:/www/tabroom/api/config/config.js /www/tabroom/api/config/

cp /www/tabroom/doc/conf/indexcards.service /etc/systemd/system
systemctl enable indexcards

a2enmod proxy_http
