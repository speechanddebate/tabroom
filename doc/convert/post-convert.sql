
alter table student add birthdate date;
alter table student add school_sid varchar(63);
alter table student add race varchar(63);
alter table round add note varchar(255);

alter table room add ada bool not null default 0;
alter table entry add flight int not null default 0;

CREATE TABLE `account_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) NOT NULL DEFAULT '0',
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `account` (`account`)
) ENGINE=InnoDB AUTO_INCREMENT=166 DEFAULT CHARSET=latin1;

alter table chapter add naudl bool not null default 0;
alter table chapter add ipeds int not null default 0;
alter table chapter add nces int not null default 0;

create table room_group ( 
	id int auto_increment primary key,
	name varchar(31),
	tourn int not null,
	timestamp timestamp
);

create table room_group_round ( 
	id int auto_increment primary key,
	room_group int not null,
	round int not null,
	timestamp timestamp,
	key `round` (`round`)
);

create table room_group_room ( 
	id int auto_increment primary key,
	room_group int not null,
	room int not null,
	timestamp timestamp,
	key `room` (`room`)
);
	
