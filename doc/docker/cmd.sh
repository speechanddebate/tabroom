#!/bin/bash

#/bin/cp /root/General.pm.default /www/tabroom/web/lib/Tab/General.pm

/bin/rm -rf /www/tabroom/web/tmp
/bin/rm -rf /www/tabroom/web/mason
#
/bin/mkdir -p /www/tabroom/web/tmp
/bin/mkdir -p /www/tabroom/web/mason
#
/bin/chmod 1777 /www/tabroom/web/tmp
/bin/chmod 1777 /www/tabroom/web/mason
#
service rsyslog start
apachectl start
tail -f /dev/null
