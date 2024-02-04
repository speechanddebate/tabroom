#!/bin/bash
#
# Moves and recycles the object cache directories, an important step because
# old cached objects and object code can interfere with updates when the cache
# sometimes does not properly validate.
#
# Moved into a separate script because Ansible does not handle recursive
# deletes well, and deleting & re-creating a whole directory leads to a short
# window that kicks errors due to the cache directory's absence.

/usr/bin/mv /www/tabroom/web/mason/cache /www/tabroom/web/mason/c2
/usr/bin/mv /www/tabroom/web/mason/obj /www/tabroom/web/mason/o2
/usr/bin/mv /www/tabroom/web/tmp /www/tabroom/web/tmp2
/usr/bin/mkdir /www/tabroom/web/tmp
/usr/bin/chmod 1777 /www/tabroom/web/tmp

/usr/bin/rm -r /www/tabroom/web/mason/c2
/usr/bin/rm -r /www/tabroom/web/mason/o2
/usr/bin/rm -r /www/tabroom/web/tmp2

echo "Complete\n";

