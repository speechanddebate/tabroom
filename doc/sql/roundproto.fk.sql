update round set protocol = NULL where NOT EXISTS (select protocol.id from protocol where protocol.id = round.protocol);
alter table round ADD CONSTRAINT fk_round_protocol FOREIGN KEY (protocol) REFERENCES protocol(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

update round set timeslot = NULL where NOT EXISTS (select timeslot.id from timeslot where timeslot.id = round.timeslot);
alter table round ADD CONSTRAINT fk_round_timeslot FOREIGN KEY (timeslot) REFERENCES timeslot(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

update round set site = NULL where NOT EXISTS (select site.id from site where site.id = round.site);
alter table round ADD CONSTRAINT fk_round_site FOREIGN KEY (site) REFERENCES site(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

update session set su = NULL where NOT EXISTS (select person.id from person where person.id = session.su);
alter table session ADD CONSTRAINT fk_session_su FOREIGN KEY (su) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

update event set tourn = NULL where NOT EXISTS (select tourn.id from tourn where tourn.id = event.tourn);
alter table event ADD CONSTRAINT fk_event_tourn FOREIGN KEY (tourn) REFERENCES tourn(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

alter table judge modify person int;
update judge set person = NULL where NOT EXISTS (select person.id from person where person.id = judge.person);
alter table judge ADD CONSTRAINT fk_judge_person FOREIGN KEY (person) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

alter table chapter_judge modify person int;
update chapter_judge set person = NULL where NOT EXISTS (select person.id from person where person.id = chapter_judge.person);
alter table chapter_judge ADD CONSTRAINT fk_chapter_judge_person FOREIGN KEY (person) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

alter table judge modify school int;
update judge set school = NULL where NOT EXISTS (select school.id from school where school.id = judge.school);
alter table judge ADD CONSTRAINT fk_judge_school FOREIGN KEY (school) REFERENCES school(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

update message set sender = NULL where NOT EXISTS (select person.id from person where person.id = message.sender);
alter table message ADD CONSTRAINT fk_message_sender FOREIGN KEY (sender) REFERENCES person(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;

update message set email = NULL where email = 0;
delete from message where email > 0 and NOT EXISTS (select email.id from email where email.id = message.email);
alter table message ADD CONSTRAINT fk_message_email FOREIGN KEY (email) REFERENCES email(id) ON UPDATE CASCADE ON DELETE CASCADE;
