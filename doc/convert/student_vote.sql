
create table student_vote (
	id int auto_increment primary key,
	rank int not null default 0,
	panel int,
	entry int,
	voter int,
	entered_by int,
	entered_at datetime,
	timestamp timestamp
);

create index entry on student_vote(entry);
create index voter on student_vote(voter);

ALTER TABLE student_vote ADD CONSTRAINT sv_evp UNIQUE (panel, entry, voter);
