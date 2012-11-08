
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

