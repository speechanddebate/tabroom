
delete e2.* from event_setting e1, event_setting e2
	where e1.event = e2.event
	and e1.tag = e2.tag
	and e1.id < e2.id;

alter table event_setting add constraint event_tag UNIQUE (event, tag);

delete e2.* from entry_setting e1, entry_setting e2
	where e1.entry = e2.entry
	and e1.tag = e2.tag
	and e1.id < e2.id;

alter table entry_setting add constraint entry_tag UNIQUE (entry, tag);

delete e2.* from tourn_setting e1, tourn_setting e2
	where e1.tourn = e2.tourn
	and e1.tag = e2.tag
	and e1.id < e2.id;

alter table tourn_setting add constraint tourn_tag UNIQUE (tourn, tag);

delete e2.* from panel_setting e1, panel_setting e2
	where e1.panel = e2.panel
	and e1.tag = e2.tag
	and e1.id < e2.id;

alter table panel_setting add constraint panel_tag UNIQUE (panel, tag);

delete e2.* from round_setting e1, round_setting e2
	where e1.round = e2.round
	and e1.tag = e2.tag
	and e1.id < e2.id;

alter table round_setting add constraint round_tag UNIQUE (round, tag);

delete e2.* from school_setting e1, school_setting e2
	where e1.school = e2.school
	and e1.tag = e2.tag
	and e1.id < e2.id;

alter table school_setting add constraint school_tag UNIQUE (school, tag);

delete e2.* from student_setting e1, student_setting e2
    where e1.student = e2.student
    and e1.tag = e2.tag
    and e1.id < e2.id;

alter table student_setting add constraint student_tag UNIQUE (student, tag);

delete e2.* from tiebreak_set_setting e1, tiebreak_set_setting e2
    where e1.tiebreak_set = e2.tiebreak_set
    and e1.tag = e2.tag
    and e1.id < e2.id;

alter table tiebreak_set_setting add constraint tiebreak_set_tag UNIQUE (tiebreak_set, tag);

delete e2.* from person_setting e1, person_setting e2
    where e1.person = e2.person
    and e1.tag = e2.tag
    and e1.id < e2.id;

alter table person_setting add constraint person_tag UNIQUE (person, tag);

delete e2.* from region_setting e1, region_setting e2
    where e1.region = e2.region
    and e1.tag = e2.tag
    and e1.id < e2.id;

alter table region_setting add constraint region_tag UNIQUE (region, tag);

