create table practice (
	id int auto_increment primary key,
	chapter int not null,
	name varchar(127),
	start datetime,
	end datetime,
	timestamp timestamp,
	created_at datetime,
	created_by int not null default 0
);

create index chapter on practice(chapter);
create index created_by on practice(created_by);

alter table practice ADD CONSTRAINT fk_practice_creator FOREIGN KEY (created_by) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;
alter table practice ADD CONSTRAINT fk_practice_chapter FOREIGN KEY (chapter) REFERENCES chapter(id) ON UPDATE CASCADE ON DELETE CASCADE;

create table practice_student (
	id int auto_increment primary key,
	practice int not null,
	student int not null,
	timestamp timestamp,
	created_at datetime,
	created_by int not null default 0
);

create index practice on practice_student(practice);
create index student on practice_student(student);
create index created_by on practice_student(created_by);

alter table practice_student ADD CONSTRAINT fk_pracs_creator FOREIGN KEY (created_by) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;
alter table practice_student ADD CONSTRAINT fk_pracs_student FOREIGN KEY (student) REFERENCES student(id) ON UPDATE CASCADE ON DELETE CASCADE;
alter table practice_student ADD CONSTRAINT fk_pracs_practice FOREIGN KEY (practice) REFERENCES practice(id) ON UPDATE CASCADE ON DELETE CASCADE;
