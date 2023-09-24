#!/bin/sh

# Deployment script targeted at the eventual ansible-ization of Tabroom into
# the NSDA cloud infrastructure.

echo
echo "Welcome to the Tabroom.com Docker image builder"
echo

echo
echo "Fixing Class DBI bug so it will talk to modern MariaDB..."
echo "Since I am the last person on earth who actually uses Class DBI I think."
echo "I'm working on it, okay?"
echo

mv /usr/share/perl5/Class/DBI.pm /usr/share/perl5/Class/DBI.pm.orig
cp /opt/config-tabroom/lib/Class-DBI.pm.fixed /usr/share/perl5/Class/DBI.pm

echo
echo "Installing some fonts rather than download the 700MB TexLive repo with all its fonts in Sanskrit..."
echo

rsync -av /opt/config-tabroom/tex/electrum /usr/share/texlive/texmf-dist/tex/latex/
rsync -av /opt/config-tabroom/tex/bera /usr/share/texlive/texmf-dist/tex/latex/

mkdir -p /usr/share/texlive/texmf-dist/fonts/tfm/public/bera/
rsync -av /opt/config-tabroom/tex/bera/tfm/ /usr/share/texlive/texmf-dist/fonts/tfm/public/bera

mkdir -p /usr/share/texlive/texmf-dist/fonts/vf/public/bera/
rsync -av /opt/config-tabroom/tex/bera/vf/ /usr/share/texlive/texmf-dist/fonts/vf/public/bera

mkdir -p /usr/share/texlive/texmf-dist/fonts/type1/public/bera
rsync -av /opt/config-tabroom/tex/bera/type1/ /usr/share/texlive/texmf-dist/fonts/type1/public/bera

mkdir -p /usr/share/texlive/texmf-dist/fonts/map/dvips/bera
cp /opt/config-tabroom/tex/bera/bera.map /usr/share/texlive/texmf-dist/fonts/map/dvips/bera

mktexlsr
texhash
updmap-sys --force --enable Map=bera.map

echo
echo "Configuring and downloading the GeoIP Database"
echo

# GeoIP.conf will require ansible secrets
# The GeoIP2-ISP.mmdb also needs to be downloaded separately.
#cp /opt/config-tabroom/conf/GeoIP.conf /etc/GeoIP.conf

mkdir -p /var/lib/GeoIP/
/usr/bin/geoipupdate
systemctl enable geoipupdate.timer

echo
echo "Configuring the Tabroom logging..."
echo

/bin/mkdir /var/log/tabroom
chown syslog:adm /var/log/tabroom
cp /opt/config-tabroom/conf/rsyslog.conf /etc/rsyslog.d/90-tabroom.conf
systemctl restart rsyslog

echo
echo "Configuring the local Apache webserver..."
echo

/usr/sbin/a2enmod apreq
/usr/sbin/a2enmod proxy
/usr/sbin/a2enmod proxy_http

cp /opt/config-tabroom/docker/tabroom.com.conf /etc/apache2/sites-available/tabroom.com.conf
a2ensite tabroom.com

# General.pm will require ansible secrets
cp /opt/config-tabroom/docker/General.pm /www/tabroom/web/lib/Tab/General.pm
cp /opt/config-tabroom/docker/mpm_prefork.conf /etc/apache2/mods-available
#echo "ServerName  www.tabroom.com" >> /etc/apache2/conf.d/hostname
#echo "127.0.1.21 www www.tabroom.com" >> /etc/hosts

echo
echo "Starting Apache..."
echo

/etc/init.d/apache2 restart

echo
echo "Yippee.  All done!  Unless, of course, you just saw errors."
echo

echo "Your Docker image has been created"

