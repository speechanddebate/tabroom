#!/bin/sh

# Deployment script for tabroom.com web installations; the developer's edition.
# b/c this is for dev only, it only is set up to work with ubuntu linux.  Error
# checking is a little light.  Mysql should be running with a blank root
# password if it hasn't been already installed.  Adjust below if this squicks
# you out, but hey, it's only for dev copies, so it shouldn't.

# Tabroom must be installed from /www/tabroom/web; otherwise many more things
# other than this script will break.

# -- Palmer

# Last updated 7/7/13

echo
echo "Welcome to the Tabroom.com system installer."
echo

echo
echo "Installing the necessary software packages along with some ones I like having...."
echo

/usr/bin/apt-get update

/usr/bin/apt-get -y -q install apache2 apache2-mpm-prefork apache2-utils libapache-session-perl libapache-session-wrapper-perl libapache2-mod-apreq2 libapache2-mod-perl2 libapache2-mod-perl2-dev libapache2-mod-perl2-doc libapache2-mod-php5 libapache2-request-perl libcgi-untaint-perl libclass-accessor-perl libclass-container-perl libclass-data-inheritable-perl libclass-dbi-abstractsearch-perl libclass-dbi-fromcgi-perl libclass-dbi-mysql-perl libclass-dbi-perl libclass-dbi-plugin-abstractcount-perl libclass-dbi-plugin-perl libclass-factory-util-perl libclass-singleton-perl libclass-trigger-perl libclone-perl libcompress-raw-zlib-perl libcrypt-passwdmd5-perl libcrypt-ssleay-perl libdate-manip-perl libdatetime-format-builder-perl libdatetime-format-mail-perl libdatetime-format-mysql-perl libdatetime-format-strptime-perl libdatetime-locale-perl libdatetime-perl libdatetime-set-perl libdatetime-timezone-perl libdbd-mysql-perl libdbi-perl libdbix-contextualfetch-perl libhtml-fromtext-perl libhtml-mason-perl libhtml-parser-perl libhtml-tagset-perl libhtml-tree-perl libmailtools-perl libmime-tools-perl libmime-lite-perl liburi-perl libwww-perl mysql-client mysql-common mysql-server nano ncurses-base ncurses-bin nmap openprinting-ppds openssh-client openssh-server openssl openssl-blacklist passwd perl perl-base perl-doc perl-modules perlmagick pm-utils popularity-contest procps psmisc pwgen rdesktop rsync screen ssh ssl-cert tcsh texlive cvs liblingua-en-numbers-ordinate-perl libuniversal-can-perl texlive-latex-extra libhtml-strip-perl libxml-simple-perl libnet-ldap-perl libdatetime-format-iso8601-perl libhtml-tableextract-perl libcrypt-rijndael-perl libphp-serialization-perl libmath-round-perl libhtml-scrubber-perl libbytes-random-secure-perl s3cmd texlive-fonts-extra libswitch-perl


echo
echo "Creating database from schema file.  Uncompressing database file (takes a little bit of time)..."
echo

echo
echo "Creating the tab database schema and setting permissions..."
echo

/usr/bin/mysqladmin -u root -f drop tabroom
/usr/bin/mysqladmin -u root create tabroom
/usr/bin/mysql -u root -f tabroom < /www/tabroom/doc/grant.sql

echo
echo "Loading the database file (sometimes takes a while, too.)..."
echo

/usr/bin/mysql -u root tabroom < /www/tabroom/doc/tabroom-schema.sql
/usr/bin/mysql -u root -f -s tabroom < /www/tabroom/doc/account-create.sql

echo
echo "Updating the database to the latest version.  Please ignore errors here, there will be some..."
echo

sleep 2

/usr/bin/mysql -u root -f -s tabroom < /www/tabroom/doc/schema-updates.sql

/bin/mkdir -p /www/tabroom/web/tmp
/bin/mkdir -p /www/tabroom/web/mason

/bin/chmod 1777 /www/tabroom/web/tmp
/bin/chmod 1777 /www/tabroom/web/mason

echo
echo "Configuring the local Apache webserver..."
echo

cp /www/tabroom/doc/local.tabroom.com.conf /etc/apache2/sites-available
cp /www/tabroom/web/lib/Tab/General.pm.default /www/tabroom/web/lib/Tab/General.pm

echo "ServerName  local.tabroom.com" >> /etc/apache2/conf.d/hostname
echo "127.0.1.21 local local.tabroom.com" >> /etc/hosts

ln -s /etc/apache2/sites-available/local.tabroom.com.conf /etc/apache2/sites-enabled/0-local.tabroom.conf

/usr/sbin/a2enmod apreq

echo
echo "Starting Apache..."
echo

/etc/init.d/apache2 restart

echo 
echo "Yippee.  All done!  Unless, of course, you just saw errors."
echo

echo "Your tabroom instance is now ready at http://local.tabroom.com."
echo "To connect with other computers will require more technical"
echo "tweaking.  See the manual in doc/howtos."
