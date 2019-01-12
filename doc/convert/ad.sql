create table ad (
	id int auto_increment primary key,
	tag varchar(255),
	filename varchar(127),
	url varchar(255),
	start datetime,
	end datetime,
	approved bool not null default 0,
	person int not null,
	approved_by int not null,
	created_at datetime,
	timestamp timestamp
);

create index person on ad(person);
create index approved_by on ad(approved_by);
