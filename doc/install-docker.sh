#!/bin/sh

# Deployment script targeted at the eventual ansible-ization of Tabroom into
# the NSDA cloud infrastructure.

echo
echo "Welcome to the Tabroom.com production system installer."
echo

#echo
#echo "Configuring the system to actually act like a server."
#echo
#cat /www/tabroom/doc/conf/sysctl.conf >> /etc/sysctl.conf
#sysctl -p

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
echo "Configuring and downloading the GeoIP Database"
echo

# GeoIP.conf will require ansible secrets
# GeoIP2-ISP.mmdb also needs to be pulled in ....somehow?
#cp /www/tabroom/doc/conf/GeoIP.conf /etc/GeoIP.conf

mkdir -p /var/lib/GeoIP/
/usr/bin/geoipupdate
systemctl enable geoipupdate.timer

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

/usr/sbin/a2enmod apreq
/usr/sbin/a2enmod proxy
/usr/sbin/a2enmod proxy_http

cp /www/tabroom/doc/conf/tabroom.com.conf.docker /etc/apache2/sites-available/tabroom.com.conf
a2ensite tabroom.com

# General.pm will require ansible secrets
cp /www/tabroom/doc/conf/General.pm /www/tabroom/web/lib/Tab/General.pm

cp /www/tabroom/doc/conf/mpm_prefork.conf /etc/apache2/mods-available

#echo "ServerName  www.tabroom.com" >> /etc/apache2/conf.d/hostname
#echo "127.0.1.21 www www.tabroom.com" >> /etc/hosts


echo
echo "Starting Apache..."
echo

/etc/init.d/apache2 restart

echo
echo "Adapting permissions"
echo

useradd tabroom;
chown -R tabroom:tabroom /www/tabroom/
/bin/mkdir -p /home/tabroom
chown -R tabroom:tabroom /home/tabroom/

/bin/mkdir -p /www/tabroom/web/tmp
/bin/mkdir -p /www/tabroom/web/mason

/bin/chmod 1777 /www/tabroom/web/tmp
/bin/chmod 1777 /www/tabroom/web/mason

echo
echo "Yippee.  All done!  Unless, of course, you just saw errors."
echo

echo "Your tabroom instance is now ready at http://www.tabroom.com."
echo "You may wish to add your local user to the tabroom group if you need to edit files directly"
echo "To connect with other computers will require more technical"
echo "tweaking.  See the manual in doc/howtos."
