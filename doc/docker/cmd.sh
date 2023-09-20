#!/bin/bash

#/bin/cp /root/General.pm.default /www/tabroom/web/lib/Tab/General.pm

ln -sf /www/tabroom/web/mason /cache/mason

/bin/rm -rf /www/tabroom/web/tmp
/bin/rm -rf /cache/mason
#
/bin/mkdir -p /www/tabroom/web/tmp
/bin/mkdir -p /cache/mason
#
/bin/chmod 1777 /www/tabroom/web/tmp
/bin/chmod 1777 /cache/mason
#
rsyslogd
apachectl start
echo "Server started!"
tail --retry --quiet -f /var/log/syslog
