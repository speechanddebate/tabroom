create table practice (
	id int auto_increment primary key,
	chapter int not null,
	tag varchar(127),
	start datetime,
	end datetime,
	timestamp timestamp,
	created_at datetime,
	created_by int not null default 0
);

create index chapter on practice(chapter);
create index created_by on practice(created_by);
create index created_by on practice(created_by)

create table practice_student (
	id int auto_increment primary key,
	practice int not null,
	student int not null,
	tag varchar(127),
	start datetime,
	end datetime,
	timestamp timestamp,
	created_at datetime,
	created_by int not null default 0
);

create index practice on practice_student(practice);
create index student on practice_student(student);
create index created_by on practice_student(created_by);
