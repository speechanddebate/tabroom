
alter table school add registered_by int;

create table stats( 
	id int auto_increment primary key,
	event int not null,
	tag varchar(63), 
	value float,
	taken datetime,
	timestamp timestamp
);

create index event on stats(event);

