create table bid_set (
	id int auto_increment primary key,
	name varchar(63),
	description varchar(255),
	label varchar(15),
	basis enum("school", "entry", "individual"),
	annual boolean,
	circuit int not null default 0
	timestamp timestamp
	UNIQUE KEY `uk_bid_duplicate` (`circuit`,`name`),
	UNIQUE KEY `uk_bid_duplicate` (`circuit`,`description`),
	KEY `circuit` (`circuit`)
);

create table bid (
	id int auto_increment primary key,
	value int not null default 0,
	entry int not null default 0,
	school int not null default 0,
	student int not null default 0,
	bid_set int not null default 0,
	UNIQUE KEY `uk_bid_duplicate` (`entry`,`school`,`student`,`bid_set`),
	KEY `entry` (`entry`),
	KEY `school` (`school`),
	KEY `student` (`student`)
);

alter table sweep_rule rename value points;
alter table sweep_rule modify place place varchar(15);
alter table sweep_rule modify count round_type varchar(15);
update sweep_rule set round_type = count_round where count_round is not null and round_type = "specific";
alter table sweep_rule drop count_round;
alter table sweep_rule add scope varchar(15) not null default 0 after place;
alter table sweep_rule add bid_set int not null default 0 after sweep_set;



