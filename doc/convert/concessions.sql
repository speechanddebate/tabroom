
create table `concession_type` ( 
	id int NOT NULL AUTO_INCREMENT,

	name varchar(31) NOT NULL,
	description text,

	concession int NOT NULL,
	timestamp timestamp,

	PRIMARY KEY (`id`),
	KEY `concession` (`concession`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `concession_option` ( 
	id int NOT NULL AUTO_INCREMENT,

	name varchar(8) NOT NULL,
	description varchar(31) NOT NULL,

	disabled bool,

	concession_type int NOT NULL,
	timestamp timestamp,

	PRIMARY KEY (`id`),
	KEY `concession_type` (`concession_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `concession_purchase_option` ( 
	id int NOT NULL AUTO_INCREMENT,

	concession_purchase int NOT NULL,
	concession_option int NOT NULL,

	timestamp timestamp,

	PRIMARY KEY (`id`),
	KEY `concession_purchase` (`concession_purchase`),
	KEY `concession_option` (`concession_option`),
	UNIQUE KEY `uk_purchase_option` (`concession_purchase`,`concession_option`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

