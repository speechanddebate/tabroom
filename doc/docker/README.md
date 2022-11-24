# A development environment for tabroom.com

## Overview

This document describes setting up a development environment for the software powering tabroom.com

You'll want to ensure you have the following prerequisite software packages installed

- [docker-ce](https://docs.docker.com/install/) and [docker compose 2](https://docs.docker.com/compose/install/), or [docker desktop](https://www.docker.com/products/docker-desktop/)
- **(Optional)** s3cmd - amazon s3 cloud storage command line tool
- **(Optional)** pv - the pipe viewer

All other dependencies should be resolved by the docker virtualization platform.

## Setting up the development environment

1. Clone the git repository and cd into the folder. Windows users will probably require `git config core.symlinks true` and running all commands inside WSL.

`git clone https://github.com/NationalSpeechandDebateAssociation/tabroom.git && cd tabroom`

2. Create a copy of General.pm in tabroom/web/lib/Tab

`cp ./web/lib/Tab/General.pm.default ./web/lib/Tab/General.pm`

3. Edit ./web/lib/Tab/General.pm to meet your needs and user credentials. Most of the configuration is self documenting.
   This document assumes the root password for the docker managed MySQL instance is "root" and the user-space credentials are "tabroom:tabroom". The docker compose instructions in `docker-compose.yml` assumes the internal network addressing calls the tabroom.com database "db". Editing the relevant parts of the source can look something like this:

`$hostname = "localhost:8000";`

...

```
#Database name
$dbname = "tabroom";
#Database host.  "localhost" will use the mysql local socket, not the network
$dbhost = "db";
#Database username
$dbuser = "tabroom";
#Database password
$dbpass = "tabroom";
```

You can also edit ./doc/docker/.env to change the config for the Nodejs and React version.

4. From the doc/docker directory, start the docker containers by using the docker compose script

`cd doc/docker && docker compose up --build -d`

This will help you evaluate any errors that may stem from your setup or this process using dockers logging utilities.

5. **(Optional)** Download & import a copy of the tabroom.com database.

You may use the docker-database-sync bash script to accomplish this, or if you've been provided a copy of the database, you can use it as an example of how to get the database into the docker-managed MySQL container.

the relevant lines to build from is:

```
docker exec -u root $(docker ps -f=name=tabroom-docker_db -q) /usr/bin/mysqladmin -u root -proot create tabroom
pv tabroom.sql | docker exec -i $(docker ps -f=name=tabroom-docker_db -q) /usr/bin/mysql -u root -proot tabroom

```

6. Point your browser to `localhost:8000` to verify the webserver system has started up, and is reading from the database.

7. To view the react version of the app, point your browser to `localhost:3000`
