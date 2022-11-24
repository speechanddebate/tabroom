FROM ubuntu:22.04
LABEL maintainer "cecilgol"
ENV DEBIAN_FRONTEND=noninteractive

COPY doc/conf/nodesource.list /etc/apt/sources.list.d
RUN apt update
# This step takes forever so seperate it to make sure its cached
RUN apt install -y -q texlive \
    texlive-latex-extra \
    texlive-luatex \
    texlive-fonts-extra

RUN /usr/bin/apt -y -q install apache2 \
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
    build-essential \
    mariadb-client \
    mariadb-common \
    mariadb-server \
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
    rsyslog \
    screen \
    ssh \
    ssl-cert \
    tcsh \
    cvs \
    liblingua-en-numbers-ordinate-perl \
    libuniversal-can-perl \
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
    libmaxmind-db-reader-perl

RUN a2enmod proxy

RUN cpanm Lingua::EN::Nums2Words
RUN cpanm REST::Client
RUN cpanm Text::Undiacritic
RUN cpanm HTTP::UA::Parser

# RUN cpanm Devel::NYTProf
# RUN apt remove libcpanel-json-xs-perl

# Load connect version of database interface
RUN mv /usr/share/perl5/Class/DBI.pm /usr/share/perl5/Class/DBI.pm.orig
COPY doc/lib/Class-DBI.pm.fixed /usr/share/perl5/Class/DBI.pm

RUN ln -s doc/utility/databse-sync /usr/local/bin

# Setup logging
RUN mkdir /var/log/tabroom
RUN chmod 1777 /var/log/tabroom
COPY doc/conf/local.rsyslog.conf /etc/rsyslog.d/90-tabroom.conf
# Don't try to read kernel logs
RUN sed -i '/imklog/d' /etc/rsyslog.conf 

# Set up apache server
COPY doc/conf/local.tabroom.com.conf /etc/apache2/sites-available
RUN echo "ServerName  local.tabroom.com" >> /etc/apache2/apache2.conf
# RUN echo "127.0.1.21 local local.tabroom.com" >> /etc/hosts
RUN ln -s /etc/apache2/sites-available/local.tabroom.com.conf /etc/apache2/sites-enabled/0-local.tabroom.conf

ENV APACHE_LOCK_DIR=/var/lock/apache2
ENV APACHE_PID_FILE=/var/run/apache2/apache2.pid
ENV APACHE_RUN_USER=www-data
ENV APACHE_RUN_GROUP=www-data
ENV APACHE_LOG_DIR=/var/log/apache2
ENV APACHE_ULIMIT_MAX_FILES='ulimit -n 65536'


COPY doc/docker/cmd.sh /root/cmd.sh
RUN chmod +x /root/cmd.sh
CMD ["/bin/sh","-c","/root/cmd.sh"]
