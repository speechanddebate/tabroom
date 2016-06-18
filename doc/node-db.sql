

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

