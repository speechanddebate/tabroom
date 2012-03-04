
#Conversions for the database.

alter table judge_group add live_updates bool;

alter table follow_comp add domain varchar(63);
alter table follow_judge add domain varchar(63);

alter table round add score int;

update tiebreak set tiebreaker="ranks" where tiebreaker="cumulative";

alter table comp drop dq_reason;
alter table comp drop wins;
alter table comp drop doubled;
alter table comp drop losses;
alter table comp drop trpc_string;
alter table comp drop initials;
alter table comp modify code varchar(15);
alter table comp drop housing;
alter table comp drop partner_housing;
alter table comp drop housing_start;
alter table comp drop housing_end;
alter table comp drop partner_housing_start;
alter table comp drop partner_housing_end;
alter table comp drop noshow;

alter table setting add modified int;

alter table judge_group drop collective;
alter table judge_group drop paradigm_explain;
alter table judge_group drop strikes_explain;
alter table judge_group drop group_max;
alter table judge_group drop ratings_need_entry;
alter table judge_group drop strikes_need_entry;
alter table judge_group drop cumulate_mjp;
alter table judge_group drop max_pool_burden;
alter table judge_group drop default_alt_reduce;
alter table judge_group drop reduce_alt_burden;
alter table judge_group add ordinals bool;


alter table event drop double_factor;
alter table event drop initial_codes;
alter table event drop reg_blockable;

alter table account drop type;
alter table account drop saw_judgewarn;
alter table account drop change_deadline;

alter table tournament add currency char;
alter table round drop score;

alter table panel add flight char;

alter table tiebreak drop round;
alter table tiebreak drop event;
alter table tiebreak drop type;
alter table tiebreak drop method;
alter table tiebreak drop covers;




