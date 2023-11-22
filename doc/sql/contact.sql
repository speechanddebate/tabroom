create table contact (
	id int auto_increment primary key,
	school int not null,
	person int not null,
	tag varchar(15),
	created_at datetime,
	timestamp timestamp,
	created_by int not null default 0
);

create index school on contact(school);
create index person on contact(person);

alter table contact ADD CONSTRAINT fk_contact_creator FOREIGN KEY (created_by) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;
alter table contact ADD CONSTRAINT fk_contact_school FOREIGN KEY (school) REFERENCES school(id) ON UPDATE CASCADE ON DELETE CASCADE;
alter table contact ADD CONSTRAINT fk_contact_person FOREIGN KEY (person) REFERENCES person(id) ON UPDATE CASCADE ON DELETE CASCADE;
