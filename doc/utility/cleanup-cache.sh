#!/bin/bash
#
# Moves and recycles the object cache directory, an important step because old
# cached objects and object code can interfere with updates sometimes
#
# Moved into a separate script because Ansible does not handle recursive
# deletes well, and deleting & re-creating a whole directory leads to a short
# window that kicks errors due to the cache directory's absence.

/usr/bin/chmod 1777 /www/tabroom/web/mason
/usr/bin/chgrp www-data /www/tabroom/web/mason
/usr/bin/mv /www/tabroom/web/mason/obj /www/tabroom/web/mason/o2

/usr/bin/mv /www/tabroom/web/tmp /www/tabroom/web/tmp2
/usr/bin/mkdir /www/tabroom/web/tmp
/usr/bin/chmod 1777 /www/tabroom/web/tmp
/usr/bin/chgrp www-data /www/tabroom/web/tmp


# The Tabroom user should have sudo permissions to run these through the
# ansible definitions.

sudo /usr/bin/rm -r /www/tabroom/web/mason/o2
sudo /usr/bin/rm -r /www/tabroom/web/tmp2

echo "Complete";

