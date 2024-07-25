#!/bin/bash
#
# Moves and recycles the object cache directory, an important step because old
# cached objects and object code can interfere with updates sometimes
#
# Moved into a separate script because Ansible does not handle recursive
# deletes well, and deleting & re-creating a whole directory leads to a short
# window that kicks errors due to the cache directory's absence.

/usr/bin/chmod 1777 /www/tabroom/web/mason
/usr/bin/mv /www/tabroom/web/mason/cache /www/tabroom/web/mason/cache.old
/usr/bin/mkdir /www/tabroom/web/mason/cache
/usr/bin/chown www-data:www-data /www/tabroom/web/mason/cache

/usr/bin/mv /www/tabroom/web/mason/obj /www/tabroom/web/mason/obj.old
/usr/bin/mkdir /www/tabroom/web/mason/obj
/usr/bin/chown www-data:www-data /www/tabroom/web/mason/obj

/usr/bin/mv /www/tabroom/web/tmp /www/tabroom/web/mason/tmp.old
/usr/bin/mkdir /www/tabroom/web/tmp
/usr/bin/chown www-data:www-data /www/tabroom/web/tmp

sudo /usr/bin/rm -r /www/tabroom/web/mason/cache.old
sudo /usr/bin/rm -r /www/tabroom/web/mason/obj.old
sudo /usr/bin/rm -r /www/tabroom/web/tmp.old

echo "Complete";

