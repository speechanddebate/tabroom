
delete a2.* from account a1, account a2 where a2.email = a1.email and a2.id > a1.id;
alter table account add constraint account_email UNIQUE(email);
alter table account add salt varchar(32);

alter table account_conflict add constraint uk_constraint UNIQUE(account,conflict,chapter,judge);
alter table account_setting add constraint uk_account_setting UNIQUE(account,tag);

delete b2.* from ballot b1, ballot b2 where b2.panel = b1.panel and b2.judge = b1.judge and b2.entry = b1.entry and b2.id > b1.id;
alter table ballot add constraint uk_ballots UNIQUE(judge,entry,panel);

delete bv2.* from ballot_value bv1, ballot_value bv2 where bv2.ballot = bv1.ballot and bv2.tag = bv1.tag and bv2.student = bv1.student and bv2.id > bv1.id;
alter table ballot_value add constraint uk_bv_scores UNIQUE(ballot,student,tag);

delete ca2.* from chapter_admin ca1, chapter_admin ca2 where ca1.chapter = ca2.chapter and ca1.account = ca2.account and ca1.id < ca2.id;
alter table chapter_admin add constraint uk_chapter_admin UNIQUE(chapter,account);

delete cc2.* from chapter_circuit cc1, chapter_circuit cc2 where cc1.chapter = cc2.chapter and cc1.circuit = cc2.circuit and cc1.id < cc2.id;
alter table chapter_circuit add constraint uk_chapter_circuit UNIQUE(chapter,circuit);

alter table circuit_admin add constraint uk_circuit_admin UNIQUE(circuit,account);

alter table circuit_setting add constraint uk_circuit_setting UNIQUE(circuit,tag);

delete es2.* from entry_student es1, entry_student es2 where es1.entry = es2.entry and es1.student = es2.student and es1.id < es2.id;

alter table entry_student add constraint uk_entry_student UNIQUE(entry,student);

delete es2.* from event_setting es1, event_setting es2 where es1.event = es2.event and es1.tag = es2.tag and es1.id < es2.id;
alter table event_setting add constraint uk_event_setting UNIQUE(event,tag);

delete jgs2.* from judge_group_setting jgs1, judge_group_setting jgs2 where jgs1.judge_group = jgs2.judge_group and jgs1.tag = jgs2.tag and jgs1.id < jgs2.id;
alter table judge_group_setting add constraint uk_judge_group_setting UNIQUE(judge_group,tag);

delete js2.* from judge_setting js1, judge_setting js2 where js1.judge = js2.judge and js1.tag = js2.tag and js1.id < js2.id;
alter table judge_setting add constraint uk_judge_setting UNIQUE(judge,tag);

alter table rating add sheet int;
#delete rt2.* from rating rt1, rating rt2 where rt1.judge = rt2.judge and rt1.entry = rt2.entry and rt1.school = rt2.school and rt1.id < rt2.id;
#alter table rating add constraint uk_rating UNIQUE(judge,entry,school,sheet);

delete ra2.* from region_admin ra1, region_admin ra2 where ra1.region = ra2.region and ra1.account = ra2.account and ra1.id < ra2.id;
alter table region_admin add constraint uk_region_admin UNIQUE(region,account);

update panel, room room1, room room2 set panel.room = room1.id where panel.room = room2.id and room1.site = room2.site and room1.name = room2.name and room1.id < room2.id;
delete room2.* from room room1, room room2 where room1.site = room2.site and room1.name = room2.name and room1.id < room2.id;
alter table room add constraint uk_room UNIQUE(site,name);

update panel, round round1, round round2 set panel.round = round1.id where panel.round = round2.id and round1.event = round2.event and round1.name = round2.name and round1.id < round2.id;
delete round2.* from round round1, round round2 where round1.event = round2.event and round1.name = round2.name and round1.id < round2.id;
alter table round add constraint uk_round UNIQUE(name,event);

update entry, school school1, school school2 set entry.school = school1.id where entry.school = school2.id and school1.tourn = school2.tourn 
	and school1.chapter = school2.chapter and school1.id < school2.id;
update judge, school school1, school school2 set judge.school = school1.id where judge.school = school2.id and school1.tourn = school2.tourn 
	and school1.chapter = school2.chapter and school1.id < school2.id;
delete school2.* from school school1, school school2 where school1.tourn = school2.tourn and school1.chapter = school2.chapter and school1.id < school2.id;
alter table school add constraint uk_school UNIQUE(chapter,tourn);

delete ta2.* from tourn_admin ta1, tourn_admin ta2 where ta1.tourn = ta2.tourn and ta1.account = ta2.account and ta1.id < ta2.id;
alter table tourn_admin add constraint uk_tourn_admin UNIQUE(tourn,account);

delete tc2.* from tourn_circuit tc1, tourn_circuit tc2 where tc1.tourn = tc2.tourn and tc1.circuit = tc2.circuit and tc1.id < tc2.id;
alter table tourn_circuit add constraint uk_tourn_circuit UNIQUE(tourn,circuit);

delete ts2.* from tourn_setting ts1, tourn_setting ts2 where ts1.tourn = ts2.tourn and ts1.tag = ts2.tag and ts1.id < ts2.id;
alter table tourn_setting add constraint uk_tourn_setting UNIQUE(tourn,tag);

