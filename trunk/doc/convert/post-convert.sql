
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
	parent_set int,
	child_set int,
	timestamp timestamp
);



