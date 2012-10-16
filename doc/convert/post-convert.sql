
create table sweep_set (
	id int auto_increment primary key,
	tourn int,
	name varchar(31),
	timestamp timestamp
);

create table sweep_event ( 
	id int auto_increment primary key,
	sweep_set int,
	event int,
	timestamp timestamp
);

create table sweep_rule ( 
	id int auto_increment primary key,
	sweep_set int,
	tag varchar(31),
	value varchar(63),
	timestamp timestamp
);

create index sweep_set on sweep_rule(sweep_set);
create index sweep_set on sweep_event(sweep_set);
create index event on sweep_event(event);
create index tourn on sweep_set(tourn);

create index entry on rating(entry);
create index judge on rating(judge);

#settings:
#sweep_max_events
#sweep_max_sets
#sweep_max_per_event
#sweep_max_overall
#sweep_max_per_set
#sweep_wildcards

create table follow_school ( 
	id int auto_increment primary key,
	school int,
	event int,
	follower int,
	timestamp timestamp
);

create index school on follow_school(school);
create index event on follow_school(event);
create index follower on follow_school(follower);

alter table chapter_admin add prefs bool;
alter table event_double add exclude int;

alter table judge add tab_rating int;

