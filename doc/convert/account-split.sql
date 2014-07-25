
CREATE TABLE `person` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(127) NOT NULL DEFAULT '',
  `first` varchar(63) DEFAULT NULL,
  `last` varchar(63) DEFAULT NULL,
  `phone` varchar(27) DEFAULT NULL,
  `alt_phone` varchar(27) DEFAULT NULL,
  `provider` varchar(63) DEFAULT NULL,
  `street` varchar(127) DEFAULT NULL,
  `city` varchar(63) DEFAULT NULL,
  `state` varchar(11) DEFAULT NULL,
  `zip` varchar(15) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `ualt_id` int(11) DEFAULT NULL,
  `site_admin` tinyint(1) DEFAULT NULL,
  `no_email` tinyint(4) NOT NULL DEFAULT '0',
  `multiple` int(11) DEFAULT NULL,
  `tz` varchar(63) DEFAULT NULL,
  `started_judging` date DEFAULT NULL,
  `diversity` tinyint(1) DEFAULT NULL,
  `flags` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `person_email` (`email`),
  UNIQUE KEY `person_ualt_id` (`ualt_id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;


CREATE TABLE `login` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `username` varchar(63) NOT NULL,
    `password` varchar(63),
    `salt` varchar(63),
	`name` varchar(63),
	`person` int(11) NOT NULL,
	`accesses` int(11) NOT NULL DEFAULT 0,
    `last_access` datetime,
    `pass_changekey` varchar(127),
    `pass_timestamp` datetime,
    `pass_change_expires` datetime,
    `source` char(4),
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `person` (`person`),
  	UNIQUE KEY `login_username` (`username`),
	FOREIGN KEY (`person`) references person(id) on delete cascade on update cascade
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

