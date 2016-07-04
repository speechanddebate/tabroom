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

create table `setting` ( 
	id int NOT NULL AUTO_INCREMENT,

	tag varchar(31) NOT NULL,

	type varchar(31) NOT NULL,
	subtype varchar(31) NOT NULL,

	value_type enum("text", "string", "bool", "integer", "float", "datetime", "enum"),

	conditions text,
	timestamp timestamp,

	PRIMARY KEY (`id`),
	UNIQUE KEY `tag` (`tag`),
	KEY `category` (`type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `setting_label` ( 
	id int NOT NULL AUTO_INCREMENT,
	lang char(2),
	label varchar(127),
	guide text,
	options text,
	setting int NOT NULL,
	timestamp timestamp,
	PRIMARY KEY (`id`),
	KEY `setting` (`setting`),
	UNIQUE KEY `lang_constraint` (`lang`, `setting`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

