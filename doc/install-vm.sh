#!/bin/sh

# Deployment script for tabroom.com web installations; this version creates a
# Tabroom head node and skips the mysql server dump bits

# Tabroom must be installed from /www/tabroom/web; otherwise many more things
# other than this script will break.

echo
echo "Welcome to the Tabroom.com system installer."
echo

echo
echo "Installing the necessary software packages along with some ones I like having...."
echo

/usr/bin/apt-get update

/usr/bin/apt-get -y -q install apache2 \
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
	nano \
	ncurses-base \
	ncurses-bin \
	nmap \
	neovim \
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
	texlive-luatex \
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
	libjson-webtoken-perl \
	libjavascript-minifier-perl \
	libcss-minifier-perl \
	pv \
	cpanminus \
	librest-application-perl \
	libtext-csv-encoded-perl \
	libtext-csv-perl

cpanm Lingua::EN::Nums2Words
cpanm REST::Client
cpanm Text::Undiacritic

apt -y remove cups
apt autoremove

echo
echo "Fixing Class DBI to talk to modern MySQL/MariaDB..."
echo

mv /usr/share/perl5/Class/DBI.pm /usr/share/perl5/Class/DBI.pm.orig
cp /www/tabroom/doc/lib/Class-DBI.pm.fixed /usr/share/perl5/Class/DBI.pm

ln -s /cache/mason /www/tabroom/web/mason

/bin/mkdir -p /cache/mason
/bin/chmod 1777 /cache/mason

echo
echo "Utility Scripts"
echo

ln -s /www/tabroom/doc/utility/refresh /usr/local/bin/
ln -s /www/tabroom/doc/utility/deploy /usr/local/bin/

echo
echo "Postfix mail"
echo

apt -y install postfix

echo
echo "Amazon S3 file access"
echo

sudo -u tabroom scp castor:/www/tabroom/web/lib/s3.config /www/tabroom/web/lib/

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

cp /www/tabroom/doc/conf/tabroom.com.conf /etc/apache2/sites-available
sudo -u tabroom scp castor:/www/tabroom/web/lib/Tab/General.pm /www/tabroom/web/lib/Tab/General.pm
cp /www/tabroom/doc/apache/mods-available/status.conf /etc/apache2/mods-available/
cp /www/tabroom/doc/apache/mods-available/mpm_prefork.conf /etc/apache2/mods-available/

ln -s /www/tabroom/doc/utility/refresh /usr/local/bin/
ln -s /www/tabroom/doc/utility/deploy /usr/local/bin/

cp /www/tabroom/doc/conf/status.conf /etc/apache2/mods-available/
cp /www/tabroom/doc/conf/mpm_prefork.conf /etc/apache2/mods-available/

cp /www/tabroom/doc/apache/mods-available /etc/apache2/mods-available/

/usr/sbin/a2enmod apreq
/usr/sbin/a2enmod proxy
/usr/sbin/a2enmod status

a2ensite tabroom.com

echo
echo "Starting Apache..."
echo

systemctl restart apache2

echo
echo "Yippee.  All done!  Unless, of course, you just saw errors."
echo

echo "Your tabroom instance should now be ready."
