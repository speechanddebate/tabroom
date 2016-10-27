
alter table permission add district int after region;

alter table district add level int after chair;
alter table district add realm varchar(7) after level;

alter table chapter add district int after nsda; 

create table chapter_setting (
	id int auto_increment primary key,
	chapter int,
	tag varchar(32),
	value varchar(127),
	value_text text,
	value_date datetime,
	timestamp timestamp,
	created_at datetime,
	setting int
);

create index chapter on chapter_setting(chapter);


create table student_setting (
	id int auto_increment primary key,
	student int,
	tag varchar(32),
	value varchar(127),
	value_text text,
	value_date datetime,
	timestamp timestamp,
	created_at datetime,
	setting int
);

create index student on student_setting(student);
create index site on tourn_site(site);
create index tourn on tourn_site(tourn);

