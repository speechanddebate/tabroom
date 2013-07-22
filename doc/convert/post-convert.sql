
create table hotel (
	id int auto_increment primary key,
	tourn int,
	name varchar(63),
	multiple int,
	timestamp timestamp
);

alter table school add hotel int not null default 0;

create table sweep_include ( 
	id int auto_increment primary key,
	parent int,
	child int,
	timestamp timestamp
);

create table tiebreak_setting ( 
	id int auto_increment primary key,
	tiebreak_set int,
	tag varchar(31),
	value varchar(127),
	timestamp timestamp
);

create index tiebreak_set on tiebreak_setting(tiebreak_set);
create index event on event_setting(event);
create index judge on judge_setting(judge);
create index judge_group on judge_group_setting(judge_group);
create index tourn on tourn_setting(tourn);

alter table result add rank int;
alter table result add percentile int; 

alter table ballot drop topic;
alter table ballot drop countmenot;
alter table ballot add seed int;
alter table ballot add pullup bool;

alter table session drop school;
alter table session drop limited;
alter table session drop circuit;
alter table session drop entry_only;
alter table session add judge_group int;

delete from tourn_setting where tag like "sweep_%";
delete from tourn_setting where tag like "drop_%";
delete from tourn_setting where tag="ask_experience";
delete from tourn_setting where tag="chair_ballot_message";
delete from tourn_setting where tag="default_qualification";
delete from tourn_setting where tag="elim_method_basis";
delete from tourn_setting where tag="judge_quality_system";
delete from tourn_setting where tag="master_printouts";
delete from tourn_setting where tag="mfl_flex_finals";
delete from tourn_setting where tag="num_judges";
delete from tourn_setting where tag="publish_paradigms";
delete from tourn_setting where tag like "points_%";

delete from tourn_setting where tag="schemat_display";
delete from tourn_setting where tag="schemat_school_code";
delete from tourn_setting where tag="truncate_ranks_to";
delete from tourn_setting where tag="truncate_to_smallest";
delete from tourn_setting where tag="mfl_time_violation";
delete from tourn_setting where tag="noshows_never_break";
delete from tourn_setting where tag="results";

