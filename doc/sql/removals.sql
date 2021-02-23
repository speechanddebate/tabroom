

alter table ballot drop hangout_admin;
alter table category drop pattern;
alter table change_log change created created_at datetime;
alter table change_log change type tag varchar(63);
alter table ballot modify chair bool after audit;
alter table chapter drop self_prefs;
alter table chapter drop nces;
alter table chapter drop ceeb;
alter table chapter drop coaches;
drop table circuit_membership;
alter table chapter_setting drop created_at;
drop table diocese;
alter table district drop chair;

