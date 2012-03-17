
CREATE TABLE `tourn_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `tourn` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `value_text` text,
    `value_date` datetime,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `judge_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `judge` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `value_text` text,
    `value_date` datetime,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `event_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `event` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `value_text` text,
    `value_date` datetime,
    PRIMARY KEY (`id`),
    KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `judge_group_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `judge_group` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `value_text` text,
    `value_date` datetime,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `judge_group` (`judge_group`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `circuit_setting` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `circuit` int(11) NOT NULL DEFAULT '0',
    `tag` varchar(31) NOT NULL DEFAULT '',
    `value` varchar(127) NOT NULL DEFAULT '',
    `value_text` text,
    `value_date` datetime,
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

alter table	chapter add coaches varchar(255);
alter table tiebreak add tourn int;
alter table file add result bool;
create index tourn on tiebreak(tourn);

CREATE TABLE `ballot_speaks` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `ballot` int(11) DEFAULT NULL,
    `student` int(11) DEFAULT NULL,
    `entry` int(11) DEFAULT NULL,
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

CREATE TABLE `follow_account` (
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

CREATE TABLE `region_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `region` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;


alter table account add paradigm text;
alter table account add 

CREATE TABLE `tourn_fee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `amount` float DEFAULT NULL,
  `reason` varchar(127) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `region_fine` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `region` int(11) DEFAULT NULL,
  `amount` float DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `levied_on` datetime DEFAULT NULL,
  `levied_by` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

update tiebreak,tournament set tiebreak.tourn = tournament.id where tournament.method = tiebreak.method;

CREATE TABLE `webpage` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `order` int(11) DEFAULT NULL,
  `title` varchar(63) DEFAULT NULL,
  `content` text,
  `last_editor` int(11) DEFAULT NULL,
  `posted_on` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

update tiebreak,tournament set tiebreak.tourn = tournament.id where tournament.method = tiebreak.method;

alter table account  ENGINE=innodb;
alter table ballot ENGINE=innodb;
alter table ballot_speaks  ENGINE=innodb;
alter table chapter  ENGINE=innodb;
alter table chapter_admin  ENGINE=innodb;
alter table chapter_circuit  ENGINE=innodb;
alter table chapter_judge  ENGINE=innodb;
alter table circuit  ENGINE=innodb;
alter table circuit_admin  ENGINE=innodb;
alter table circuit_dues ENGINE=innodb;
alter table circuit_membership ENGINE=innodb;
alter table circuit_setting  ENGINE=innodb;
alter table concession ENGINE=innodb;
alter table concession_purchase  ENGINE=innodb;
alter table email  ENGINE=innodb;
alter table entry  ENGINE=innodb;
alter table entry_qualifier  ENGINE=innodb;
alter table entry_student  ENGINE=innodb;
alter table event  ENGINE=innodb;
alter table event_double ENGINE=innodb;
alter table event_setting  ENGINE=innodb;
alter table file ENGINE=innodb;
alter table follow_account ENGINE=innodb;
alter table follow_entry ENGINE=innodb;
alter table follow_judge ENGINE=innodb;
alter table housing_slots  ENGINE=innodb;
alter table housing_student  ENGINE=innodb;
alter table judge  ENGINE=innodb;
alter table judge_group  ENGINE=innodb;
alter table judge_group_setting  ENGINE=innodb;
alter table judge_hire ENGINE=innodb;
alter table judge_setting  ENGINE=innodb;
alter table panel  ENGINE=innodb;
alter table pool ENGINE=innodb;
alter table pool_judge ENGINE=innodb;
alter table qualifier  ENGINE=innodb;
alter table rating ENGINE=innodb;
alter table rating_subset  ENGINE=innodb;
alter table rating_tier  ENGINE=innodb;
alter table region ENGINE=innodb;
alter table region_admin ENGINE=innodb;
alter table region_fine  ENGINE=innodb;
alter table room ENGINE=innodb;
alter table room_pool  ENGINE=innodb;
alter table room_strike  ENGINE=innodb;
alter table round  ENGINE=innodb;
alter table school ENGINE=innodb;
alter table school_fine  ENGINE=innodb;
alter table session  ENGINE=innodb;
alter table site ENGINE=innodb;
alter table strike ENGINE=innodb;
alter table strike_time  ENGINE=innodb;
alter table student  ENGINE=innodb;
alter table tiebreak ENGINE=innodb;
alter table tiebreak_set ENGINE=innodb;
alter table timeslot ENGINE=innodb;
alter table tourn  ENGINE=innodb;
alter table tourn_admin  ENGINE=innodb;
alter table tourn_change ENGINE=innodb;
alter table tourn_circuit  ENGINE=innodb;
alter table tourn_fee  ENGINE=innodb;
alter table tourn_ignore ENGINE=innodb;
alter table tourn_setting  ENGINE=innodb;
alter table tourn_site ENGINE=innodb;
alter table webpage  ENGINE=innodb;
