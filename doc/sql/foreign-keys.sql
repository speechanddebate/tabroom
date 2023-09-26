	delete from tourn_site where not exists(
		select tourn.id from tourn where tourn.id = tourn_site.tourn);

	delete from tourn_site where not exists(
		select site.id from site where site.id = tourn_site.site);

	alter table tourn_site
		ADD CONSTRAINT fk_tourn_id FOREIGN KEY (tourn)
		REFERENCES tourn(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table tourn_site
		ADD CONSTRAINT fk_site_id FOREIGN KEY (site)
		REFERENCES site(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;


	delete from permission
		where tourn IS NOT NULL
		and tourn > 0
		and not exists(
			select tourn.id from tourn where tourn.id = permission.tourn
		);

	delete from permission
		where chapter IS NOT NULL
		and chapter > 0
		and not exists(
			select chapter.id from chapter where chapter.id = permission.chapter
		);

	delete from permission
		where circuit IS NOT NULL
		and circuit > 0
		and not exists(
			select circuit.id from circuit where circuit.id = permission.circuit
		);

	delete from permission
		where region IS NOT NULL
		and region > 0
		and not exists(
			select region.id from region where region.id = permission.region
		);

	delete from permission
		where district IS NOT NULL
		and district > 0
		and not exists(
			select district.id from district where district.id = permission.district
		);

	delete from permission
		where category IS NOT NULL
		and category > 0
		and not exists(
			select category.id from category where category.id = permission.category
		);

	delete from permission
		where person IS NOT NULL
		and person > 0
		and not exists(
			select person.id from person where person.id = permission.person
		);

	alter table permission
		ADD CONSTRAINT fk_person_id FOREIGN KEY (person)
		REFERENCES person(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table permission
		ADD CONSTRAINT fk_tourn_id FOREIGN KEY (tourn)
		REFERENCES tourn(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table permission
		ADD CONSTRAINT fk_chapter_id FOREIGN KEY (chapter)
		REFERENCES chapter(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table permission
		ADD CONSTRAINT fk_category_id FOREIGN KEY (category)
		REFERENCES category(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table permission
		ADD CONSTRAINT fk_circuit_id FOREIGN KEY (circuit)
		REFERENCES circuit(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table permission
		ADD CONSTRAINT fk_region_id FOREIGN KEY (region)
		REFERENCES region(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	alter table permission
		ADD CONSTRAINT fk_district_id FOREIGN KEY (district)
		REFERENCES district(id)
		ON DELETE CASCADE
		ON UPDATE CASCADE;

	delete from school_setting where not exists (
		select school.id from school where school.id = school_setting.school);
	delete from person_setting where not exists (
		select person.id from person where person.id = person_setting.person);
	delete from tourn_setting where not exists (
		select tourn.id from tourn where tourn.id = tourn_setting.tourn);
	delete from event_setting where not exists (
		select event.id from event where event.id = event_setting.event);
	delete from student_setting where not exists (
		select student.id from student where student.id = student_setting.student);
	delete from round_setting where not exists (
		select round.id from round where round.id = round_setting.round);
	delete from tiebreak_set_setting where not exists (
		select tiebreak_set.id from tiebreak_set
		where tiebreak_set.id = tiebreak_set_setting.tiebreak_set);
	delete from region_setting where not exists (
		select region.id from region where region.id = region_setting.region);
	delete from panel_setting where not exists (
		select panel.id from panel where panel.id = panel_setting.panel);
	delete from jpool_setting where not exists (
		select jpool.id from jpool where jpool.id = jpool_setting.jpool);
	delete from rpool_setting where not exists (
		select rpool.id from rpool where rpool.id = rpool_setting.rpool);
	delete from entry_setting where not exists (
		select entry.id from entry where entry.id = entry_setting.entry);
	delete from category_setting where not exists (
		select category.id from category where category.id = category_setting.category);
	delete from chapter_setting where not exists (
		select chapter.id from chapter where chapter.id = chapter_setting.chapter);

	delete from round_setting where tag="num_judges" and value="1";

	alter table school_setting ADD CONSTRAINT fk_schoolsetting_tourn FOREIGN KEY (school)
		REFERENCES school(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table person_setting ADD CONSTRAINT fk_personsetting_tourn FOREIGN KEY (person)
		REFERENCES person(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table tourn_setting ADD CONSTRAINT fk_tournsetting_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table event_setting ADD CONSTRAINT fk_eventsetting_tourn FOREIGN KEY (event)
		REFERENCES event(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table student_setting ADD CONSTRAINT fk_studentsetting_tourn FOREIGN KEY (student)
		REFERENCES student(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table round_setting ADD CONSTRAINT fk_roundsetting_tourn FOREIGN KEY (round)
		REFERENCES round(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table tiebreak_set_setting ADD CONSTRAINT fk_tiebreak_setsetting_tourn FOREIGN KEY (tiebreak_set)
		REFERENCES tiebreak_set(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table region_setting ADD CONSTRAINT fk_regionsetting_tourn FOREIGN KEY (region)
		REFERENCES region(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table panel_setting ADD CONSTRAINT fk_panelsetting_tourn FOREIGN KEY (panel)
		REFERENCES panel(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table jpool_setting ADD CONSTRAINT fk_jpoolsetting_tourn FOREIGN KEY (jpool)
		REFERENCES jpool(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table rpool_setting ADD CONSTRAINT fk_rpoolsetting_tourn FOREIGN KEY (rpool)
		REFERENCES rpool(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table entry_setting ADD CONSTRAINT fk_entrysetting_tourn FOREIGN KEY (entry)
		REFERENCES entry(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table category_setting ADD CONSTRAINT fk_categorysetting_tourn FOREIGN KEY (category)
		REFERENCES category(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table chapter_setting ADD CONSTRAINT fk_chaptersetting_tourn FOREIGN KEY (chapter)
		REFERENCES chapter(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from round where not exists( select event.id from event where event.id = round.event);
	delete from panel where not exists( select round.id from round where round.id = panel.round);
	delete from ballot where not exists( select panel.id from panel where panel.id = ballot.panel);
	delete from score where not exists( select ballot.id from ballot where ballot.id = score.ballot);

	alter table score ADD CONSTRAINT fk_score_ballot FOREIGN KEY (ballot)
		REFERENCES ballot(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table ballot ADD CONSTRAINT fk_ballot_panel FOREIGN KEY (panel)
		REFERENCES panel(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table panel ADD CONSTRAINT fk_panel_round FOREIGN KEY (round)
		REFERENCES round(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table round ADD CONSTRAINT fk_round_event FOREIGN KEY (event)
		REFERENCES event(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from event where not exists(
		select category.id from category where category.id = event.category);
	delete from category where not exists(
		select tourn.id from tourn where tourn.id = category.tourn);
	delete from timeslot where not exists(
		select tourn.id from tourn where tourn.id = timeslot.tourn);
	delete from tiebreak_set where not exists (
		select tourn.id from tourn where tourn.id = tiebreak_set.tourn);
	delete from room where not exists (
		select site.id from site where site.id = room.site);
	delete from school where not exists(
		select tourn.id from tourn where tourn.id = school.tourn) ;
	delete from tiebreak where not exists(
		select tiebreak_set.id from tiebreak_set where tiebreak_set.id = tiebreak.tiebreak_set) ;
	delete from sweep_rule where not exists(
		select sweep_set.id from sweep_set where sweep_set.id = sweep_rule.sweep_set) ;
	delete from sweep_event where not exists(
		select sweep_set.id from sweep_set where sweep_set.id = sweep_event.sweep_set) ;

	alter table room ADD CONSTRAINT fk_room_site FOREIGN KEY (site)
		REFERENCES site(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table tiebreak_set ADD CONSTRAINT fk_tiebreak_set_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table category ADD CONSTRAINT fk_category_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table timeslot ADD CONSTRAINT fk_timeslot_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table event ADD CONSTRAINT fk_event_category FOREIGN KEY (category)
		REFERENCES category(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table school ADD CONSTRAINT fk_school_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table tiebreak ADD CONSTRAINT fk_tiebreak_tiebreak_set FOREIGN KEY (tiebreak_set)
		REFERENCES tiebreak_set(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table sweep_rule ADD CONSTRAINT fk_sweep_rule_sweep_set FOREIGN KEY (sweep_set)
		REFERENCES sweep_set(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table sweep_event ADD CONSTRAINT fk_sweep_event_sweep_set FOREIGN KEY (sweep_set)
		REFERENCES sweep_set(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from session where not exists(
		select person.id from person where person.id = session.person) ;
	delete from result_value where not exists(
		select result.id from result where result.id = result_value.result) ;
	delete from result_key where not exists(
		select result_set.id from result_set where result_set.id = result_key.result_set) ;
	delete from result_set where result_set.event > 0 and not exists(
		select event.id from event where event.id = result_set.event);
	delete from result_set where result_set.tourn > 0 and not exists(
		select tourn.id from tourn where tourn.id = result_set.tourn);

	alter table result_value ADD CONSTRAINT fk_result_value_result FOREIGN KEY (result)
		REFERENCES result(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table result_key ADD CONSTRAINT fk_result_key_result_set FOREIGN KEY (result_set)
		REFERENCES result_set(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table session ADD CONSTRAINT fk_session_person FOREIGN KEY (person)
		REFERENCES person(id) ON DELETE CASCADE ON UPDATE CASCADE;

	alter table result_set modify tourn int;
	alter table result_set modify event int;

	update result_set set event = NULL where event = 0;
	update result_set set tourn = NULL where tourn = 0;

	alter table result_set ADD CONSTRAINT fk_result_set_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;
	alter table result_set ADD CONSTRAINT fk_result_set_event FOREIGN KEY (event)
		REFERENCES event(id) ON DELETE CASCADE ON UPDATE CASCADE;

	update site set name="Discord" where id=6934;
	update site set name="Sierra Preparatory Academy" where id = 6146;
	update site set name="Gonzaga University" where id = 1912;
	update site set name="VDR" where id = 7175;
	update site set name="Zoom" where id=4889;

	alter table setting_label ADD CONSTRAINT fk_setting_label FOREIGN KEY (setting)
		REFERENCES setting(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from campus_log where person.id IS NOT NULL and not exists (
		select person.id from person where person.id = campus_log.person
	);

	alter table campus_log ADD CONSTRAINT fk_cl_person FOREIGN KEY (person)
		REFERENCES person(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from campus_log where campus_log.entry IS NOT NULL and not exists (
		select entry.id from entry where entry.id = campus_log.entry
	);

	alter table campus_log ADD CONSTRAINT fk_cl_entry FOREIGN KEY (entry)
		REFERENCES entry(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from campus_log where campus_log.judge IS NOT NULL and not exists (
		select judge.id from judge where judge.id = campus_log.judge
	);

	alter table campus_log ADD CONSTRAINT fk_cl_judge FOREIGN KEY (judge)
		REFERENCES judge(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from campus_log where campus_log.tourn IS NOT NULL and not exists (
		select tourn.id from tourn where tourn.id = campus_log.tourn
	);

	alter table campus_log ADD CONSTRAINT fk_cl_tourn FOREIGN KEY (tourn)
		REFERENCES tourn(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from campus_log where campus_log.panel IS NOT NULL and not exists (
		select panel.id from panel where panel.id = campus_log.panel
	);

	alter table campus_log ADD CONSTRAINT fk_cl_panel FOREIGN KEY (panel)
		REFERENCES panel(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from campus_log where campus_log.school IS NOT NULL and not exists (
		select school.id from school where school.id = campus_log.school
	);

	alter table campus_log ADD CONSTRAINT fk_cl_school FOREIGN KEY (school)
		REFERENCES school(id) ON DELETE CASCADE ON UPDATE CASCADE;

	delete from result where not exists ( select rs.id from result_set rs where result.result_set = rs.id)
	alter table result ADD CONSTRAINT fk_result_rs FOREIGN KEY (result_set) REFERENCES result_set(id) ON UPDATE CASCADE ON DELETE SET DEFAULT;
