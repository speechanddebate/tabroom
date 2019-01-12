
drop table if exists setting; 
drop table if exists setting_header; 
drop table if exists setting_label;

create table `setting_header` ( 
	id int NOT NULL AUTO_INCREMENT,
	tab varchar(31) NOT NULL,
	tag varchar(31) NOT NULL, 
	sort_order SMALLINT,
	timestamp timestamp,
	PRIMARY KEY (`id`),
	UNIQUE KEY `tag_constraint` (`tag`),
	UNIQUE KEY `order_constraint` (`tab`, `sort_order`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `setting` ( 
	id int NOT NULL AUTO_INCREMENT,
	tag varchar(31) NOT NULL,
	sort_order SMALLINT,
	answer_type enum("text", "string", "bool", "integer", "float", "datetime", "enum"),
	answer text,
	conditional_on varchar(31),
	conditional_value varchar(31),
	setting_header int not null,
	timestamp timestamp,
	PRIMARY KEY (`id`),
	UNIQUE KEY `tag` (`tag`),
	KEY `setting_header` (`setting_header`)
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

