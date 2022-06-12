DROP TABLE IF EXISTS person_quiz;
DROP TABLE IF EXISTS quiz;

CREATE TABLE `quiz` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(63) DEFAULT NULL,
  `label` varchar(255) DEFAULT NULL,
  `questions` text,
  `description` varchar(511),
  `sitewide` bool NOT NULL DEFAULT 0,
  `badge` varchar(511) DEFAULT NULL,
  `person` int(11) NOT NULL,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `person` (`person`),
  CONSTRAINT `fk_quiz_tourn` FOREIGN KEY (`tourn`) REFERENCES `tourn` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_quiz_circuit` FOREIGN KEY (`circuit`) REFERENCES `circuit` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `person_quiz` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `person` int(11) NOT NULL,
  `quiz` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_pq_person` FOREIGN KEY (`person`) REFERENCES `person` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_pq_quiz` FOREIGN KEY (`quiz`) REFERENCES `quiz` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `caselist` (
	`id` int(11) NOT NULL AUTO_INCREMENT,
	`slug` varchar(255) NOT NULL,
	`eventcode` int(6) NOT NULL DEFAULT 0,
	`person` int(11) NOT NULL,
	`partner` int(11) NULL,
	PRIMARY KEY (`id`),
	KEY `slug` (`slug`),
	KEY `person` (`person`),
	KEY `partner` (`partner`),
	CONSTRAINT `fk_cl_person` FOREIGN KEY (`person`) REFERENCES `person` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT `fk_cl_partner` FOREIGN KEY (`partner`) REFERENCES `person` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


