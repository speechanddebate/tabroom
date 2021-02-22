

alter table ballot change noshow forfeit bool;

update tiebreak_setting set tag="forfeits_never_break" where tag="noshows_never_break";

alter table ballot add judge_started datetime; 
alter table ballot change account entered_by int;

rename table ballot_value to score;

rename table tourn_change to change_log;

rename table account_setting to person_setting; 
alter table person_setting change account person int;

rename table account_conflict to conflict; 
rename table person_conflict to conflict; 
alter table conflict change account person int;
alter table conflict change conflict conflicted int;

alter table change_log change text description text; 
alter table change_log change account person int;

alter table student change acct_request person_request int;

alter table student change acct_request person_request int;
alter table judge change acct_request person_request int;
alter table chapter_judge change acct_request person_request int;

alter table permission change account person int;
alter table tourn_ignore change account person int;
alter table student change account person int;
alter table judge change account person int;
alter table follower change account person int;
alter table chapter_judge change account person int;
alter table housing change account person int;
alter table chapter add ceeb varchar(15);

drop table account;
drop table account_ignore;
drop table school_contact;

ALTER TABLE chapter_judge DROP INDEX account;
create INDEX person on chapter_judge(person);

ALTER TABLE conflict DROP INDEX account;
create INDEX person on conflict(person);

ALTER TABLE judge DROP INDEX account;
create INDEX person on judge(person);

ALTER TABLE permission DROP INDEX account;
create INDEX person on permission(person);

ALTER TABLE person_setting DROP INDEX account;
create INDEX person on person_setting(person);

ALTER TABLE student DROP INDEX account;
create INDEX person on student(person);

ALTER TABLE judge DROP INDEX account;
create INDEX person on judge(person);

ALTER TABLE tourn_ignore DROP INDEX account;
create INDEX person on tourn_ignore(person);

rename table judge_group to category; 
rename table judge_group_setting to category_setting; 

alter table judge change judge_group category int;
alter table event change judge_group category int;
alter table change_log change judge_group category int;
alter table jpool change judge_group category int;
alter table category change judge_group category int;
alter table judge_hire change judge_group category int;
alter table permission change judge_group category int;
alter table rating_subset change judge_group category int;
alter table rating_tier change judge_group category int;
alter table session change judge_group category int;
alter table strike_time change judge_group category int;
alter table event change judge_group category int;
alter table judge change alt_group alt_category int;

alter table category_setting change judge_group category int;

ALTER TABLE judge DROP INDEX judge_group;
create INDEX category on judge(category);

ALTER TABLE permission DROP INDEX judge_group;
create INDEX category on permission(category);

ALTER TABLE category_setting DROP INDEX judge_group;
create INDEX category on category_setting(category);

ALTER TABLE event DROP INDEX judge_group;
create INDEX category on event(category);

ALTER TABLE jpool DROP INDEX judge_group;
create INDEX category on jpool(category);

ALTER TABLE permission DROP INDEX judge_group;
create INDEX category on permission(category);

alter table circuit_membership change approval approval_required bool;
alter table circuit_membership change region region_required bool;
alter table circuit_membership drop created_at;

alter table chapter_circuit drop active; 
alter table chapter_circuit drop paid; 
alter table chapter_circuit drop created_at; 
alter table chapter_circuit change membership circuit_membership int; 

alter table chapter_judge drop paradigm;
alter table chapter_judge drop identity;

alter table concession add school_cap int;
