#!/bin/sh

# Deployment script for tabroom.com web installations; the developer's edition.
# b/c this is for dev only, it only is set up to work with ubuntu linux.  Error
# checking is a little light.  Mysql should be running with a blank root
# password if it hasn't been already installed.  Adjust below if this squicks
# you out, but hey, it's only for dev copies or emergency restore situations, so it shouldn't.

# Tabroom must be installed from /www/tabroom/web; otherwise many more things
# other than this script will break.

# Last updated 5/13/20

echo
echo "Welcome to the Tabroom.com system installer."
echo

/usr/bin/apt-get update

echo
echo "Installing basic software toolchain"
echo

apt-get -y -q install \
    wget \
    curl \
    git \
    unzip \
    make \
    rsyslog \
    openssl \
    bzip2 \
    awscli \
    s3cmd

echo
echo "Installing the necessary software packages along with some ones I like having...."
echo

/usr/bin/apt-get -y -q install 
	apache2 \
	apache2-utils \
	bzip2 \
	libapache-session-perl \
	libapache-session-wrapper-perl \
	libapache2-mod-apreq2 \
	libapache2-mod-perl2 \
	libapache2-mod-perl2-dev \
	libapache2-mod-perl2-doc \
	libapache2-mod-php \
	libapache2-request-perl \
	libcgi-untaint-perl \
	libclass-accessor-perl \
	libclass-container-perl \
	libclass-data-inheritable-perl \
	libclass-dbi-abstractsearch-perl \
	libclass-dbi-fromcgi-perl \
	libclass-dbi-mysql-perl \
	libclass-dbi-perl \
	libclass-dbi-plugin-abstractcount-perl \
	libclass-dbi-plugin-perl \
	libclass-factory-util-perl \
	libclass-singleton-perl \
	libclass-trigger-perl \
	libclone-perl \
	libcompress-raw-zlib-perl \
	libcrypt-passwdmd5-perl \
	libcrypt-ssleay-perl \
	libdate-manip-perl \
	libdatetime-format-builder-perl \
	libdatetime-format-mail-perl \
	libdatetime-format-mysql-perl \
	libdatetime-format-strptime-perl \
	libdatetime-locale-perl \
	libdatetime-perl \
	libdatetime-set-perl \
	libdatetime-timezone-perl \
	libdbd-mysql-perl \
	libdbi-perl \
	libdbix-contextualfetch-perl \
	libhtml-fromtext-perl \
	libhtml-mason-perl \
	libhtml-parser-perl \
	libhtml-tagset-perl \
	libhtml-tree-perl \
	libmailtools-perl \
	libmime-tools-perl \
	libmime-lite-perl \
	liburi-perl \
	libwww-perl \
	make \
	mariadb-client \
	mariadb-common \
	mariadb-server \
	nano \
	ncurses-base \
	ncurses-bin \
	nmap \
	openprinting-ppds \
	openssh-client \
	openssh-server \
	openssl \
	passwd \
	perl \
	perl-base \
	perl-doc \
	perl-modules \
	perlmagick \
	pm-utils \
	popularity-contest \
	procps \
	psmisc \
	pwgen \
	rdesktop \
	rsync \
	screen \
	ssh \
	ssl-cert \
	tcsh \
	texlive \
	cvs \
	liblingua-en-numbers-ordinate-perl \
	libuniversal-can-perl \
	texlive-latex-extra \
	libhtml-strip-perl \
	libxml-simple-perl \
	libnet-ldap-perl \
	libdatetime-format-iso8601-perl \
	libhtml-tableextract-perl \
	libcrypt-rijndael-perl \
	libphp-serialization-perl \
	libmath-round-perl \
	libhtml-scrubber-perl \
	libbytes-random-secure-perl \
	s3cmd \
	texlive-fonts-extra \
	libswitch-perl \
	libjson-perl \
	libjavascript-minifier-perl \
	libcss-minifier-perl \
	pv \
	cpanminus \
	librest-application-perl \
	libtext-csv-encoded-perl \
	libtext-csv-perl

cpanm JSON::WebToken
cpanm Lingua::EN::Nums2Words
cpanm REST::Client

echo
echo "Fixing Class DBI to talk to modern MySQL/MariaDB..."
echo

mv /usr/share/perl5/Class/DBI.pm /usr/share/perl5/Class/DBI.pm.orig
cp /www/tabroom/doc/Class-DBI.pm.fixed /usr/share/perl5/Class/DBI.pm

echo
echo "Creating the tab database schema and setting permissions..."
echo

/usr/bin/mysqladmin -u root -f drop tabroom
/usr/bin/mysqladmin -u root create tabroom
/usr/bin/mysql -u root -f tabroom < /www/tabroom/doc/init/grant.sql

echo
echo "Downloading and uncompressing database file (takes a little bit of time)..."
echo

/usr/bin/s3cmd get "s3://speechanddebate-db/$(date -I)/tabroom.sql.bz2" .
/bin/bzip2 -d tabroom.sql.bz2

echo
echo "Loading the database file (sometimes takes a while, too.)..."
echo

/usr/bin/mysql -u root tabroom < /www/tabroom/doc/tabroom.sql

echo
echo "Removing sql dump file after importing"
echo

/bin/rm /www/tabroom/doc/tabroom.sql

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

# Commenting out as this is only for development purposes
# echo "127.0.1.21 local local.tabroom.com" >> /etc/hosts

ln -s /etc/apache2/sites-available/local.tabroom.com.conf /etc/apache2/sites-enabled/0-local.tabroom.conf

/usr/sbin/a2enmod apreq

echo
echo "Starting Apache..."
echo

/etc/init.d/apachectl start

echo 
echo "Yippee.  All done!  Unless, of course, you just saw errors."
echo

echo "Your tabroom instance is now ready at http://local.tabroom.com."
echo "To connect with other computers will require more technical"
echo "tweaking.  See the manual in doc/howtos."
