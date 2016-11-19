
delete from tiebreak where tiebreak_set is null;
delete from score where tag="comment";

alter table permission add district int after region;
alter table district add level int after chair;
alter table district add realm varchar(7) after level;

alter table chapter add district int after nsda; 

create table chapter_setting (
	id int auto_increment primary key,
	chapter int,
	tag varchar(32),
	value varchar(127),
	value_text text,
	value_date datetime,
	timestamp timestamp,
	created_at datetime,
	setting int
);

create index chapter on chapter_setting(chapter);

create table student_setting (
	id int auto_increment primary key,
	student int,
	tag varchar(32),
	value varchar(127),
	value_text text,
	value_date datetime,
	timestamp timestamp,
	created_at datetime,
	setting int
);

create index student on student_setting(student);
create index site on tourn_site(site);
create index tourn on tourn_site(tourn);

alter table tiebreak add child int after name;

update tiebreak set name="opp_ranks" where name="competition";
update tiebreak set name="judgevar" where name="judge_var";
update tiebreak set count="previous" where count="last elim";

update event set type="debate" where type="other";
update event set type="debate" where type="roundrobin";
update event set type="debate" where type="lincoln-douglas";

update event set type="congress" where type="" and name like "%ongress%";
update event set type="debate" where type="" and name like "%LD%";
update event set type="debate" where type="" and name like "%olicy%";
update event set type="pf" where type="" and name like "%orum%";
update event set type="policy" where type="" and name like "%CX%";
update event set type="speech" where type="";

update round set type="prelim" where type="preset";
