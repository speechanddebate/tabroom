delete from entry_setting where not exists (select entry.id from entry where entry.id = entry_setting.entry);
optimize table entry_setting;
delete from school_setting where not exists (select school.id from school where school.id = school_setting.school);
optimize table school_setting;
delete from entry_student where not exists (select entry.id from entry where entry.id = entry_student.entry);
optimize table entry_student;

delete from rating where not exists (select judge.id from judge where rating.judge = judge.id);
optimize table rating;

delete from rating where not exists (select entry.id from entry where rating.entry = entry.id);
optimize table rating;

delete from person_setting where not exists (select person.id from person where person.id = person_setting.person);
optimize table person_setting;

delete from email where 1=1
	and not exists (select tourn.id from tourn where tourn.id = email.tourn)
	and not exists (select circuit.id from circuit where circuit.id = email.circuit);

optimize table email;

delete from campus_log where 1=1
	and not exists (select person.id from person where person.id = campus_log.person)

delete from campus_log where 1=1
	and campus_log.tourn > 0
	and not exists (select tourn.id from tourn where tourn.id = campus_log.tourn)

optimize table campus_log;

delete from session where 1=1
	and not exists (select person.id from person where person.id = session.person);

optimize table session;

delete from event where not exists (select tourn.id from tourn where tourn.id = event.tourn);
optimize table event;

delete from event_setting where not exists (select event.id from event where event.id = event_setting.event);
optimize table event_setting;

delete from round where not exists (select event.id from event where event.id = round.event);
optimize table round;

delete from round_setting where not exists (select entry.id from entry where entry.id = entry_setting.entry);
optimize table round_setting;

delete from webpage where 1=1
	and webpage.tourn > 0
	and NOT EXISTS (select tourn.id from tourn where tourn.id = webpage.tourn);

optimize table webpage;

delete from judge where 1=1
	and not exists ( select category.id from category where category.id = judge.category);
optimize table judge;

delete from judge_setting where not exists (select judge.id from judge where judge.id = judge_setting.judge);
optimize table judge_setting;

alter table rating add constraint entry_judge UNIQUE(judge,entry,type,school);
alter table rating drop constraint entry_judge;

