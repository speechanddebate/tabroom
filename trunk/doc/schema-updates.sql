
#create table sweep_set (
#	id int auto_increment primary key,
#	tourn int,
#	name varchar(31),
#	timestamp timestamp
#);

#create index sweep_set on sweep_rule(sweep_set);
#alter table judge add tab_rating int;

create table account_conflict ( 
	id int auto_increment primary key,
	account int not null,
	conflict int not null default 0,
	chapter int not null default 0,
	added_by int not null default 0,
	timestamp timestamp
);

create index chapter on account_conflict(chapter);
create index account on account_conflict(account);
create index conflict on account_conflict(conflict);
create index added_by on account_conflict(added_by);


alter table result add student int;

create table result_set  (
	id int auto_increment primary key,
	tourn int not null,
	event int not null,
	label varchar(255),
	timestamp timestamp
);

alter table result add result_set int not null;
alter table result drop label;
alter table result drop sweeps;
alter table result drop bid;
alter table result drop seed;
alter table result_value drop label;
alter table result_value add tag varchar(15);
alter table result_value add no_sort bool not null default 0;
alter table result add school int;



