/*!40101 SET character_set_client = utf8 */;

alter table tourn_weekend rename to weekend;

CREATE TABLE `weekend` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL,
  `start` datetime NOT NULL,
  `end` datetime NOT NULL,
  `reg_start` datetime NOT NULL,
  `reg_end` datetime NOT NULL,
  `freeze_deadline` datetime NOT NULL,
  `drop_deadline` datetime NOT NULL,
  `judge_deadline` datetime NOT NULL,
  `fine_deadline` datetime NOT NULL,
  `tourn` int(11) not null,
  `site` int(11) not null,
  `city` varchar(127),
  `state` varchar(127),
  `timestamp` timestamp,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

create table weekend_setting (
	id int auto_increment primary key,
	weekend int,
	tag varchar(32),
	value varchar(127),
	value_text text,
	value_date datetime,
	timestamp timestamp,
	created_at datetime,
	setting int
);

create index weekend on weekend_setting(weekend);
