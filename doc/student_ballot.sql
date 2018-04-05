create table student_ballot (
	id int auto_increment primary key,
	tag varchar(15),
	panel int not null default 0,
	entry int not null default 0,
	voter int not null default 0,
	value int,
	entered_by int,
	timestamp timestamp
);

create index panel on student_ballot(panel);
create index entry on student_ballot(entry);
create index voter on student_ballot(voter);

