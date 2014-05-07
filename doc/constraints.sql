

alter table account add constraint account_email UNIQUE(email);

alter table account_conflict add constraint uk_constraint UNIQUE(account,conflict,chapter,judge);

alter table account_setting add constraint uk_account_setting UNIQUE(account,tag);

delete b2.* from ballot b1, ballot b2 where b2.panel = b1.panel and b2.judge = b1.judge and b2.entry = b1.entry and b2.id > b1.id;

alter table ballot add constraint uk_ballots UNIQUE(judge,entry,panel);

delete bv2.* from ballot_value bv1, ballot_value bv2 where bv2.ballot = bv1.ballot and bv2.tag = bv1.tag and bv2.student = bv1.student and bv2.id > bv1.id;

alter table ballot_value add constraint uk_bv_scores UNIQUE(ballot,student,tag);

delete ca2.* from chapter_admin ca1, chapter_admin ca2 where ca1.chapter = ca2.chapter and ca1.account = ca2.account and ca1.id < ca2.id;

alter table chapter_admin add constraint uk_chapter_admin UNIQUE(chapter,account);

alter table chapter_circuit add constraint uk_chapter_circuit UNIQUE(chapter,circuit);


alter table circuit_admin add constraint uk_circuit_admin UNIQUE(circuit,account);

alter table circuit_setting add constraint uk_circuit_setting UNIQUE(circuit,tag);

delete es2.* from entry_student es1, entry_student es2 where es1.entry = es2.entry and es1.student = es2.student and es1.id < es2.id;

alter table entry_student add constraint uk_entry_student UNIQUE(entry,student);




