
alter table score drop tiebreak;
alter table score drop cat_id;

alter table ballot drop constraint uk_ballots;

alter table ballot drop speechnumber;
alter table ballot drop cat_id;

alter table panel drop score;
alter table panel drop cat_id;
alter table panel drop confirmed;
alter table panel drop label;

drop table diocese;

alter table region drop quota;
alter table region drop arch;
alter table region drop cooke_pts;
alter table region drop sweeps;

alter table login drop nsda_login_id;
alter table login drop source;
alter table login drop spinhash;

alter table login drop ualt_id;
alter table person drop ualt_id;
alter table student drop ualt_id;
alter table chapter_judge drop ualt_id;

delete from judge_setting where tag = "ualt_id";
delete from student_setting where tag = "ualt_id";

alter table ballot drop collected;
alter table ballot drop collected_by;
alter table ballot drop seed;
alter table ballot drop pullup;

update entry_setting set tag="autoqual" where tag="auto_qual";
update entry_setting set tag="registered_seed" where tag="regsitered_seed";
