FROM ubuntu:23.04
COPY ./web /www/tabroom
COPY ./doc/install-docker.sh /usr/local/bin/install-docker.sh
RUN chmod +x /usr/local/bin/install-docker.sh
RUN /usr/local/bin/install-docker.sh

EXPOSE 80
CMD ["/usr/sbin/apache2ctl", "-D", "FOREGROUND"]
