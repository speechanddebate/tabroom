
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
    `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
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
create index tourn on tiebreak(tourn);

CREATE TABLE `ballot_value` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `tag` varchar(31) NOT NULL DEFAULT '',
    `ballot` int(11) DEFAULT NULL,
    `student` int(11) DEFAULT NULL,
    `tiebreak` int(11) DEFAULT NULL,
    `value` float DEFAULT NULL,
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
  `request` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `account` (`account`),
  KEY `follower` (`follower`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `follow_tourn` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `follower` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`),
  KEY `follower` (`follower`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `tourn_circuit` (
    `id` int(11) NOT NULL AUTO_INCREMENT,
    `tourn` int(11) NOT NULL DEFAULT '0',
    `circuit` int(11) NOT NULL DEFAULT '0',
  	`approved` tinyint(1) NOT NULL DEFAULT '0',
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

alter table account add started date;
alter table account add provider text;
alter table account add gender char;
alter table account add paradigm text;

CREATE TABLE `tourn_fee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `amount` float DEFAULT NULL,
  `reason` varchar(127) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`),
   KEY `tourn` (`tourn`)
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

update tiebreak,tournament set tiebreak.tourn = tournament.id where tournament.method = tiebreak.method;

CREATE TABLE `webpage` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `page_order` int(11) DEFAULT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '0',
  `sitewide` tinyint(1) NOT NULL DEFAULT '0',
  `title` varchar(63) DEFAULT NULL,
  `content` text,
  `last_editor` int(11) DEFAULT NULL,
  `posted_on` datetime DEFAULT NULL,
  `special` varchar(15) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`),
   KEY `tourn` (`tourn`),
   KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `result` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `entry` int(11) DEFAULT NULL,
  `sweeps` int(11) DEFAULT NULL,
  `bid` tinyint(1) NOT NULL DEFAULT '0',
  `label` varchar(63) DEFAULT NULL,
  `seed` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`),
   KEY `entry` (`entry`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `result_value` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `result` int(11) DEFAULT NULL,
  `label` varchar(63) DEFAULT NULL,
  `value` float DEFAULT NULL,
  `priority` int(7) NOT NULL DEFAULT '0',
  `sort_desc` tinyint(1) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`),
   KEY `result` (`result`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

update tiebreak,tournament set tiebreak.tourn = tournament.id where tournament.method = tiebreak.method;

alter table file add result int;
alter table room_pool add timestamp timestamp;

alter table tournament add tz varchar(31);
update tournament,league 
	set tournament.tz = league.timezone 
	where tournament.league = league.id;

alter table tournament add state varchar(7);
alter table tournament add country varchar(7);
update tournament set country = "US";

CREATE TABLE `account_ignore` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`),
   KEY `account` (`account`),
   KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
