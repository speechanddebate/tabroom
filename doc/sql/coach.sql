create table coach (
	id int auto_increment primary key,
	person int not null,
	entry int not null default 0,
	student int not null default 0,
	nsda int not null default 0,
	created_at datetime,
	created_by int not null default 0,
	timestamp timestamp
);

create index entry on coach(entry);
create index student on coach(student);
create index person on coach(person);

alter table coach ADD CONSTRAINT fk_coach_creator FOREIGN KEY (created_by) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;
alter table coach ADD CONSTRAINT fk_coach_student FOREIGN KEY (student) REFERENCES student(id) ON UPDATE CASCADE ON DELETE CASCADE;
alter table coach ADD CONSTRAINT fk_coach_entry FOREIGN KEY (entry) REFERENCES entry(id) ON UPDATE CASCADE ON DELETE CASCADE;
alter table coach ADD CONSTRAINT fk_coach_person FOREIGN KEY (person) REFERENCES person(id) ON UPDATE CASCADE ON DELETE CASCADE;


