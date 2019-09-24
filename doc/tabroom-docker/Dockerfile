FROM ubuntu:trusty
LABEL maintainer "cecilgol"
ENV DEBIAN_FRONTEND=noninteractive
RUN apt-get upgrade && apt-get update && apt-get install -y -q --no-install-recommends \
    curl \
    cvs \
    git \
    libapache-session-perl \
    libapache-session-wrapper-perl \
    libapache2-mod-apreq2 \
    libapache2-mod-perl2 \
    libapache2-mod-perl2-dev \
    libapache2-mod-perl2-doc \
    libapache2-mod-php5 \
    libapache2-request-perl \
    libbytes-random-secure-perl \
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
    libcrypt-rijndael-perl \
    libcrypt-ssleay-perl \
    libcss-minifier-perl \
    libdate-manip-perl \
    libdatetime-format-builder-perl \
    libdatetime-format-iso8601-perl \
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
    libhtml-scrubber-perl \
    libhtml-strip-perl \
    libhtml-tableextract-perl \
    libhtml-tagset-perl \
    libhtml-tree-perl \
    libjavascript-minifier-perl \
    libjson-perl \
    liblingua-en-numbers-ordinate-perl \
    libmailtools-perl \
    libmath-round-perl \
    libmime-lite-perl \
    libmime-tools-perl \
    libnet-ldap-perl \
    libphp-serialization-perl \
    libswitch-perl \
    libuniversal-can-perl \
    liburi-perl \
    libwww-perl \
    libxml-simple-perl \
    make \
    openprinting-ppds \
    openssl-blacklist \
    perl-doc \
    perlmagick \
    pm-utils \
    s3cmd \
    wget \
    rsyslog \
    #texlive-latex-extra \
    #texlive-fonts-extra \
    #texlive-base
    texlive-full

RUN curl -L https://cpanmin.us | perl - App::cpanminus
RUN cpanm install JSON::WebToken
RUN cpanm install Text::Trim
RUN cpanm install REST::Client 

COPY /www/tabroom/doc/local.tabroom.com.conf /etc/apache2/sites-available
#COPY General.pm.default /root/General.pm.default
RUN mv /usr/share/perl5/Class/DBI.pm /usr/share/perl5/Class/DBI.pm.orig
COPY /www/tabroom/doc/Class-DBI.pm.fixed /usr/share/perl5/Class/DBI.pm
RUN echo "ServerName  local.tabroom.com" >> /etc/apache2/apache2.conf

ENV APACHE_LOCK_DIR=/var/lock/apache2
ENV APACHE_PID_FILE=/var/run/apache2/apache2.pid
ENV APACHE_RUN_USER=www-data
ENV APACHE_RUN_GROUP=www-data
ENV APACHE_LOG_DIR=/var/log/apache2
ENV APACHE_ULIMIT_MAX_FILES='ulimit -n 65536'

RUN ln -s /etc/apache2/sites-available/local.tabroom.com.conf /etc/apache2/sites-enabled/0-local.tabroom.conf

COPY cmd.sh /root/cmd.sh
RUN chmod +x /root/cmd.sh
CMD ["/bin/sh","-c","/root/cmd.sh"]
