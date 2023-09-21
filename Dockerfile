FROM ubuntu:23.04
COPY ./docker /opt/docker

# Do package install here so we can cache it
RUN /usr/bin/apt update
RUN /usr/bin/apt -y -q install apache2 \
	apache2-utils \
	bzip2 \
	rsync \
	cpanminus \
	libapache2-mod-apreq2 \
	libapache2-mod-perl2 \
	libapache2-mod-perl2-dev \
	libapache2-mod-perl2-doc \
	libapache2-request-perl \
	libhtml-restrict-perl \
	libclass-accessor-perl \
	libclass-container-perl \
	libclass-data-inheritable-perl \
	libclass-dbi-abstractsearch-perl \
	libclass-dbi-mysql-perl \
	libclass-dbi-perl \
	libcompress-raw-zlib-perl \
	libcrypt-passwdmd5-perl \
	libcryptx-perl \
	libdatetime-format-mysql-perl \
	libdatetime-perl \
	libdatetime-set-perl \
	libdatetime-timezone-perl \
	libdbd-mysql-perl \
	libdbi-perl \
	libhtml-fromtext-perl \
	libhtml-mason-perl \
	libio-socket-ssl-perl \
	libmime-lite-perl \
	liburi-perl \
	libtext-unidecode-perl \
	liblingua-en-numbers-ordinate-perl \
	libhtml-strip-perl \
	libmath-round-perl \
	libhtml-scrubber-perl \
	libswitch-perl \
	libjavascript-minifier-perl \
	libcss-minifier-perl \
	librest-application-perl \
	libtext-csv-perl \
	libwww-perl \
	libgeoip2-perl \
	libmaxmind-db-reader-perl \
	make \
	mariadb-client \
	mariadb-common \
	perl \
	perl-base \
	perl-doc \
	perl-modules \
	perlmagick \
	psmisc \
	ssl-cert \
	texlive \
	texlive-latex-extra \
	s3cmd \
	pv \
	geoip-database \
	geoipupdate \
	libgeoip1

RUN cpanm REST::Client
RUN cpanm Text::Undiacritic
RUN cpanm HTTP::UA::Parser
RUN cpanm JSON@4.02
RUN cpanm JSON::WebToken
RUN cpanm Crypt::JWT

COPY ./doc/install-docker.sh /usr/local/bin/install-docker.sh
RUN chmod +x /usr/local/bin/install-docker.sh
RUN /usr/local/bin/install-docker.sh

EXPOSE 80
CMD ["/usr/sbin/apache2ctl", "-D", "FOREGROUND"]
