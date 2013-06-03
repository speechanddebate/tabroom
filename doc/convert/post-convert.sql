
create table hotel (
	id int auto_increment primary key,
	tourn int,
	name varchar(63),
	multiple int,
	timestamp timestamp
);

alter table school add hotel int not null default 0;

create table sweep_include ( 
	id int auto_increment primary key,
	parent int,
	child int,
	timestamp timestamp
);

create table tiebreak_setting ( 
	id int auto_increment primary key,
	tiebreak_set int,
	tag varchar(31),
	value varchar(127),
	timestamp timestamp
);

create index tiebreak_set on tiebreak_setting(tiebreak_set);
create index event on event_setting(event);
create index judge on judge_setting(judge);
create index judge_group on judge_group_setting(judge_group);
create index tourn on tourn_setting(tourn);

alter table result add rank int;
alter table result add percentile int; 



