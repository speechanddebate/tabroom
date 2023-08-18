#!/bin/sh

# Deployment script for tabroom.com web installations; the developer's edition.
# b/c this is for dev only, it only is set up to work with ubuntu linux.  Error
# checking is a little light.  Mysql should be running with a blank root
# password if it hasn't been already installed.  Adjust below if this squicks
# you out, but hey, it's only for dev copies, so it shouldn't.

# Tabroom must be installed from /www/tabroom/web; otherwise many more things
# other than this script will break.

# Last updated 4/7/20

echo
echo "Welcome to the Tabroom.com system installer."
echo

echo
echo "Installing the latest version of Node and related files..."
echo

cp /www/tabroom/doc/conf/nodesource.list /etc/apt/sources.list.d;

echo
echo "Installing the necessary software packages along with some ones I like having...."
echo

/usr/bin/apt update

/usr/bin/apt -y -q install apache2 \
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
	libcrypt-jwt-perl \
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
	liburi-encode-perl \
	libwww-perl \
	libtext-unidecode-perl \
	make \
	mariadb-client \
	mariadb-common \
	mariadb-server \
	nmap \
	neovim \
	nodejs \
	npm \
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
	procps \
	psmisc \
	rsync \
	screen \
	ssh \
	ssl-cert \
	texlive \
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
	libswitch-perl \
	libjson-perl \
	libjson-webtoken-perl \
	libjavascript-minifier-perl \
	libcss-minifier-perl \
	pv \
	cpanminus \
	librest-application-perl \
	libtext-csv-encoded-perl \
	libtext-csv-perl \
	libgeoip2-perl \
	libmaxmind-db-reader-perl \
	geoip-database \
	geoipupdate \
	libgeoip1:amd64

cpanm Lingua::EN::Nums2Words
cpanm REST::Client
cpanm Text::Undiacritic
cpanm HTTP::UA::Parser

# This is for the NYT Profiler tools for development.  I'm mostly putting it
# here to remove the XS dependency from cpanel that breaks it because I can
# never remember that fix.

cpanm Devel::NYTProf
apt remove libcpanel-json-xs-perl

echo
echo "Fixing Class DBI to talk to modern MySQL/MariaDB..."
echo

mv /usr/share/perl5/Class/DBI.pm /usr/share/perl5/Class/DBI.pm.orig
cp /www/tabroom/doc/lib/Class-DBI.pm.fixed /usr/share/perl5/Class/DBI.pm

echo
echo "Installing some fonts rather than download the 700MB TexLive repo with all its fonts in Sanskrit..."
echo

rsync -av /www/tabroom/doc/tex/electrum /usr/share/texlive/texmf-dist/tex/latex/

rsync -av /www/tabroom/doc/tex/bera /usr/share/texlive/texmf-dist/tex/latex/
mkdir -p /usr/share/texlive/texmf-dist/fonts/tfm/public/bera/
rsync -av /www/tabroom/doc/tex/bera/tfm/ /usr/share/texlive/texmf-dist/fonts/tfm/public/bera

mkdir -p /usr/share/texlive/texmf-dist/fonts/vf/public/bera/
rsync -av /www/tabroom/doc/tex/bera/vf/ /usr/share/texlive/texmf-dist/fonts/vf/public/bera

mkdir -p /usr/share/texlive/texmf-dist/fonts/type1/public/bera
rsync -av /www/tabroom/doc/tex/bera/type1/ /usr/share/texlive/texmf-dist/fonts/type1/public/bera

mkdir -p /usr/share/texlive/texmf-dist/fonts/map/dvips/bera
cp /www/tabroom/doc/tex/bera/bera.map /usr/share/texlive/texmf-dist/fonts/map/dvips/bera

mktexlsr
texhash
updmap-sys --force --enable Map=bera.map

echo
echo "Creating database from schema file.  Uncompressing database file (takes a little bit of time)..."
echo

echo
echo "Creating the tab database schema and setting permissions..."
echo

/usr/bin/mysqladmin -u root -f drop tabroom
/usr/bin/mysqladmin -u root create tabroom
/usr/bin/mysql -u root -f tabroom < /www/tabroom/doc/init/grant.sql

echo
echo "Loading the database schematic"
echo
/usr/bin/mysql -u root tabroom < /www/tabroom/doc/sql/current-schema.sql


echo
echo "Utility Scripts"
echo

ln -s /www/tabroom/doc/utility/database-sync /usr/local/bin/

echo
echo "Configuring the Tabroom logging..."
echo

/bin/mkdir /var/log/tabroom
chown syslog.adm /var/log/tabroom
cp /www/tabroom/doc/conf/rsyslog.conf /etc/rsyslog.d/90-tabroom.conf
systemctl restart rsyslog

echo
echo "Configuring the local Apache webserver..."
echo

cp /www/tabroom/doc/conf/local.tabroom.com.conf /etc/apache2/sites-available
cp /www/tabroom/web/lib/Tab/General.pm.default /www/tabroom/web/lib/Tab/General.pm

echo "ServerName  local.tabroom.com" >> /etc/apache2/conf.d/hostname
echo "127.0.1.21 local local.tabroom.com" >> /etc/hosts

# Do it this way because the syntax for a virtual site that Ubuntu expects is a
# moving target apparently.  Sometimes it wants you to include the .conf,
# sometimes it just wants the domain....

ln -s /etc/apache2/sites-available/local.tabroom.com.conf /etc/apache2/sites-enabled/0-local.tabroom.conf

/usr/sbin/a2enmod apreq
/usr/sbin/a2enmod proxy

echo
echo "Starting Apache..."
echo

/etc/init.d/apache2 restart

echo
echo "Installing some Node/NPM necessities"
echo

cd /www/tabroom/api; npm install
cd /www/tabroom/api; npm install pm2 -g
cd /www/tabroom/client; npm install


echo
echo "Adapting permissions"
echo

useradd tabroom;
chown -R tabroom.tabroom /www/tabroom/
/bin/mkdir -p /www/tabroom/web/tmp
/bin/mkdir -p /www/tabroom/web/mason

/bin/chmod 1777 /www/tabroom/web/tmp
/bin/chmod 1777 /www/tabroom/web/mason

echo
echo "Staring the indexcards API"
echo
cp /www/tabroom/doc/conf/indexcards.service /etc/systemd/system/indexcards.service
systemctl enable --now indexcards

echo
echo "Yippee.  All done!  Unless, of course, you just saw errors."
echo

echo "Your tabroom instance is now ready at http://local.tabroom.com."
echo "You may wish to add your local user to the tabroom group if you need to edit files directly"
echo "To connect with other computers will require more technical"
echo "tweaking.  See the manual in doc/howtos."
