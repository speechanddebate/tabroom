
alter table tabroom.tourn_change add webpage int;
alter table tabroom.tourn_change add judge_group int;
alter table tabroom.tourn_change add strike int;

alter table tabroom.tourn_change add round int;

alter table tabroom.circuit_membership add region_required bool;
alter table tabroom.circuit_membership add timestamp timestamp;

alter table tabroom.circuit add url varchar(255);

alter table tabroom.concession add school_cap int;

alter table tabroom.account_conflict add type varchar(15);

update tabroom.account_conflict set type = "school" where chapter > 0;
update tabroom.account_conflict set type = "individual" where conflict > 0;

alter table tabroom.email add region int; 

alter table tabroom.file add public bool;
alter table tabroom.file add webpage int;

alter table tabroom.school_fine add tourn int;
alter table tabroom.school_fine add judge int;
alter table tabroom.school_fine add region int;

alter table tabroom.judge_hire add requestor int;

alter table tabroom.login add sha512 char(128);
alter table tabroom.login add spinhash char(64);

alter table tabroom.person add middle varchar(63);
alter table tabroom.person add pronoun varchar(63);
alter table tabroom.person add postal varchar(15);


alter table tabroom.qualifier add qualified_at int;

alter table tabroom.rating add draft bool;

alter table tabroom.region add active bool;


DROP TABLE IF EXISTS `school_contact`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;

CREATE TABLE `school_contact` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `created_at` datetime NOT NULL,
  `updated_at` datetime NOT NULL,
  `school` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


alter table tabroom.school add onsite_by int;

alter table tabroom.ballot_value add speech smallint;

update tabroom.ballot_value, tabroom.ballot 
	set tabroom.ballot_value.speech = tabroom.ballot.speechnumber 
	where tabroom.ballot.id = tabroom.ballot_value.ballot 
	and tabroom.ballot.speechnumber > 0;

alter table tabroom.chapter add address varchar(255);
alter table tabroom.chapter add zip int;
alter table tabroom.chapter add postal varchar(16);

alter table tabroom.chapter_judge add ada bool;
alter table tabroom.chapter_judge add email varchar(128);
alter table tabroom.chapter_judge add created_at datetime;
alter table tabroom.chapter_judge add updated_at datetime;

alter table tabroom.stats add type varchar(15);

alter table tabroom.strike add hidden bool;
alter table tabroom.strike add timeslot int;
alter table tabroom.strike add entered_by int;

alter table tabroom.student add middle varchar(128);

alter table tabroom.tourn add city varchar(128);
alter table tabroom.webpage add creator int;

alter table tabroom.tourn_setting add setting int; 
alter table tabroom.judge_group_setting add setting int; 
alter table tabroom.judge_setting add setting int; 
alter table tabroom.event_setting add setting int; 
alter table tabroom.entry_setting add setting int; 
alter table tabroom.round_setting add setting int; 
alter table tabroom.circuit_setting add setting int; 
alter table tabroom.account_setting add setting int; 
alter table tabroom.jpool_setting add setting int; 
alter table tabroom.rpool_setting add setting int; 
alter table tabroom.tiebreak_setting add setting int; 
alter table tabroom.school_setting add setting int; 

alter table tabroom.tiebreak_setting add value_date datetime;
alter table tabroom.tiebreak_setting add value_text text;

#PERMISSIONS

alter table tabroom.permission add created_at datetime;

create table `nsda_points` (
	id int NOT NULL AUTO_INCREMENT,

	points int NOT NULL DEFAULT 0,
	earned_at datetime,
	venue  varchar(127),
	event_name varchar(63),
	level enum("hs", "college", "middle", "elem"),
	source enum("T", "J", "M", "O"),
	results text,

	person_id int NOT NULL,

	coach int NULL,
	tourn int NULL,
	event int NULL,
	school int NULL,
	student int NULL,
	recorder int NULL,
	nsda_category int NULL, 

	created_at datetime,
	updated_at datetime,

	PRIMARY KEY (`id`),
	KEY `person` (`person`),
	KEY `coach` (`coach`),
	KEY `student` (`student`),
	KEY `school` (`school`),
	KEY `recorder` (`recorder`),
	KEY `tourn` (`tourn`),
	KEY `event` (`event`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `nsda_honors` ( 
	id int NOT NULL AUTO_INCREMENT,

	tag varchar(63) NOT NULL,
	value int NOT NULL DEFAULT 0,
	earned_at datetime,
	venue  varchar(127),
	event_name varchar(63),
	level enum("hs", "college", "middle", "elem"),
	source enum("T", "J", "M", "O"),
	results text,

	tourn int NULL,
	event int NULL,
	coach int NULL,
	person int NULL,
	school int NULL,
	student int NULL,
	recorder int NULL,

	created_at datetime,
	updated_at datetime,

	PRIMARY KEY (`id`),
	KEY `person` (`person`),
	KEY `coach` (`coach`),
	KEY `student` (`student`),
	KEY `recorder` (`recorder`),
	KEY `tourn` (`tourn`),
	KEY `event` (`event`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `nsda_categories` ( 
	id int NOT NULL AUTO_INCREMENT,
	label varchar(127),
	type enum("speech", "debate", "congress"),
	national bool,
	created_at datetime,
	updated_at datetime,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `settings` ( 
	id int NOT NULL AUTO_INCREMENT,

	type varchar(31) NOT NULL,
	tag varchar(31) NOT NULL,
	category varchar(31) NOT NULL,
	value_type enum("text", "string", "bool", "integer", "float", "datetime", "enum"),
	conditions text,

	description text,
	full_description text,

	created_at datetime,
	updated_at datetime,
	PRIMARY KEY (`id`),
	KEY `tag` (`tag`),
	KEY `category` (`category`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `setting_labels` ( 
	id int NOT NULL AUTO_INCREMENT,
	lang char(2),
	label varchar(127),
	guide text,
	options text,
	setting int NOT NULL,
	created_at datetime,
	updated_at datetime,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

