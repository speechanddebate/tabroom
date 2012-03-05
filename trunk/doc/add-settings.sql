
CREATE TABLE `tourn_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `tourn` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `text` text,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;


CREATE TABLE `judge_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `judge` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `text` text,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `chapter_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `notes` varchar(254) DEFAULT NULL,
  `cell` varchar(31) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `account` (`account`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `event_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `event` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `text` text,
    PRIMARY KEY (`id`),
    KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `judge_group_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `judge_group` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `text` text,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `judge_group` (`judge_group`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `circuit_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `circuit` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `text` text,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

alter table	chapter add coaches varchar(255);
alter table tiebreak add tourn int;
create index tourn on tiebreak(tourn);

CREATE TABLE `ballot_student` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `ballot` int(11) DEFAULT NULL,
    `student` int(11) DEFAULT NULL,
    `points` int(11) DEFAULT NULL,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `student` (`student`),
    KEY `ballot` (`ballot`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;


CREATE TABLE `qualifier` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    entry int default null,
    name varchar(63) default null,
    result varchar(127) default null,
    tourn int default null,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `entry` (`entry`),
    KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `follower` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `follower` int(11) DEFAULT NULL,
  `parent` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `account` (`account`),
  KEY `follower` (`follower`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `tourn_circuit` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `tourn` int(11) NOT NULL DEFAULT '0',
    `circuit` int(11) NOT NULL DEFAULT '0',
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `tourn` (`tourn`),
    KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

