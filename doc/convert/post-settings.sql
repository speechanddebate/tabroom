
alter table bin rename to strike_time;


alter table comp rename to entry;
alter table item rename to concession;
alter table league rename to circuit;

alter table account_access rename to tourn_admin;
alter table league_admin rename to circuit_admin;
alter table chapter_access rename to chapter_admin;
alter table chapter_league rename to chapter_circuit;
alter table purchase rename to concession_purchase;
alter table qual rename to rating_tier;
alter table qual_subset rename to rating_subset;
alter table rating change subset rating_subset int;
alter table roomblock rename to room_strike;
alter table team_member rename to entry_student;

alter table follow_comp rename to follow_entry;
alter table follow_entry change comp entry int;
alter table account_ignore add timestamp timestamp;
alter table rating change comp entry int;
alter table strike change comp entry int;
alter table student_result change comp entry int;
alter table entry_student change comp entry int;

alter table chapter_circuit change league circuit int;
alter table due_payment change league circuit int;
alter table due_payment rename to circuit_dues;
alter table email change league circuit int;
alter table circuit_admin change league circuit int;
alter table news change league circuit int;
alter table region change league circuit int;
alter table site change league circuit int;

alter table concession drop event;
alter table concession change tournament tourn int;

alter table concession_purchase change item concession int;
alter table concession_purchase add placed datetime;
alter table concession_purchase add fulfilled bool;

alter table rating change qual rating_tier int;
alter table rating_tier add rating_subset int;
alter table rating add ordinal int;

alter table round drop no_first_year;
alter table round drop score;
alter table round add motion text;
alter table round add tb_set int;
alter table round drop preset;
alter table round drop number_judges;
alter table round add blasted datetime;
alter table round add online bool;
alter table round add post_results bool;

drop table bill;
drop table sweep;
drop table flight;
drop table interest;
drop table account_interest;
drop table elim_assign;
drop table resultfile;
drop table method;
drop table schedule;
drop table link;
drop table news;
drop table setting;
drop table coach;

alter table uber rename to chapter_judge;
alter table chapter_judge add acct_request int;

alter table tournament rename to tourn;
alter table tournament_site rename to tourn_site;
alter table fine rename to school_fine;
alter table no_interest rename to account_ignore;
create index account on account_ignore(account);
create index tourn on account_ignore(tourn);

alter table account drop password;
alter table account drop active;
alter table account drop hotel;
alter table account drop saw_judgewarn;
alter table account drop change_deadline;
alter table account change change_timestamp password_timestamp datetime;
alter table account drop is_cell;
alter table account drop type;

alter table account add country char(4);
alter table chapter add country char(4);
update account set country="US";
update account set tz="America/New_York" where tz is null;
update chapter set country="US";


alter table ballot drop win;
alter table ballot drop rank;
alter table ballot drop seed;
alter table ballot drop flight;
alter table ballot drop points;
alter table ballot drop position;
alter table ballot drop real_rank;
alter table ballot drop real_points;
alter table ballot drop rankinround;


alter table ballot change comp entry int;
alter table ballot change rank rank tinyint(4);
alter table ballot change real_rank rank int;
alter table ballot change real_points points int;

alter table class rename to event_double;
alter table event_double drop judge_group;
alter table event_double change tournament tourn int;
create index tourn on event_double(tourn);
alter table event_double add min int;
alter table event_double change double_entry setting tinyint;
alter table event change class event_double int;

alter table entry change code code varchar(63);
alter table entry change apda_seed seed varchar(15);
alter table entry change registered_at reg_time datetime;
alter table entry change dropped_at drop_time datetime;

alter table entry add reg_by int;
alter table entry add drop_by int;

alter table entry drop tb0;
alter table entry drop tb1;
alter table entry drop tb2;
alter table entry drop tb3;
alter table entry drop tb4;
alter table entry drop tb5;
alter table entry drop tb6;
alter table entry drop tb7;
alter table entry drop tb8;
alter table entry drop tb9;
alter table entry drop results_bar;
alter table entry drop student;
alter table entry drop partner;
alter table entry drop dq_reason;
alter table entry drop doubled;
alter table entry drop housing;
alter table entry drop partner_housing;
alter table entry drop sweeps_points;
alter table entry drop noshow;
alter table entry drop qualifier;
alter table entry drop qualexp;
alter table entry drop qual2exp;
alter table entry drop qual2;
alter table entry drop wins;
alter table entry drop losses;
alter table entry drop initials;
alter table entry drop trpc_string;
alter table entry drop housing_start;
alter table entry drop housing_end;
alter table entry drop partner_housing_start;
alter table entry drop partner_housing_end;
alter table entry change tournament tourn int;

alter table housing add account int;
alter table housing change tournament tourn int;
alter table housing add school int;

alter table judge drop neutral;
alter table judge drop cfl_tab_first;
alter table judge drop cfl_tab_second;
alter table judge drop cfl_tab_third;
alter table judge drop housing;
alter table judge drop spare_pool;
alter table judge drop score;
alter table judge drop tournament;
alter table judge drop hire; 
alter table judge drop prelim_pool;
alter table judge drop cell;
alter table judge drop first_year;
alter table judge drop paradigm;
alter table judge drop novice;
alter table judge drop trpc_string;
alter table judge change uber chapter_judge int;
alter table judge add account int not null default 0;
update judge set account = 0 where account is null;
alter table judge add acct_request int;
alter table judge add dropped int;
alter table judge add drop_time datetime;
alter table judge add reg_time datetime;
alter table judge add drop_by int;
alter table judge add hired int;

alter table judge_group drop missing_judge_fee;
alter table judge_group drop reduce_alt_burden;
alter table judge_group drop uncovered_entry_fee;
alter table judge_group drop track_by_pools;
alter table judge_group drop alt_max;
alter table judge_group drop min_burden;
alter table judge_group drop max_burden;
alter table judge_group drop default_alt_reduce;
alter table judge_group drop ask_paradigm;
alter table judge_group drop strikes_explain;
alter table judge_group drop conflicts;
alter table judge_group drop paradigm_explain;
alter table judge_group drop fee_missing;
alter table judge_group drop pub_assigns;
alter table judge_group drop dio_min;
alter table judge_group drop ask_alts;
alter table judge_group drop free;
alter table judge_group drop judge_per;
alter table judge_group drop deadline;
alter table judge_group drop collective;
alter table judge_group drop school_strikes;
alter table judge_group drop strike_reg_opens;
alter table judge_group drop strike_reg_closes;
alter table judge_group drop max_pool_burden;
alter table judge_group drop track_judge_hires;
alter table judge_group drop hired_fee;
alter table judge_group drop hired_pool;
alter table judge_group drop cumulate_mjp;
alter table judge_group drop special;
alter table judge_group drop elim_special;
alter table judge_group drop tab_room;
alter table judge_group drop live_updates;
alter table judge_group drop ratings_need_paradigms;
alter table judge_group drop ratings_need_entry;
alter table judge_group drop strikes_need_paradigms;
alter table judge_group drop strikes_need_entry;
alter table judge_group drop obligation_before_strikes;
alter table judge_group drop coach_ratings;
alter table judge_group drop school_ratings;
alter table judge_group drop comp_ratings;
alter table judge_group drop comp_strikes;
alter table judge_group drop group_max;
alter table judge_group change tournament tourn int;


drop table pool_group;
alter table rating change name value varchar(15);
alter table region drop tournament;
alter table region drop director;
alter table room_pool drop tournament;
alter table room_pool change tournament tourn int;



update school set paid_amount=paid_amount+concession_paid_amount;
alter table school change paid_amount paid float;
alter table school drop concession_paid_amount;
alter table school drop disclaimed;
alter table school drop sweeps_points;
alter table school drop score;
alter table school add contact int;
alter table school change entered entered_on datetime;
alter table school add self_register bool;
alter table school add self_reg_deadline datetime;

alter table school_fine drop region;
alter table school_fine drop start;
alter table school add self_register bool;
alter table school add self_reg_deadline datetime;
alter table school_fine drop end;
alter table school_fine change levied levied_on datetime;
alter table school_fine add levied_by int;

alter table event add publish tinyint;
alter table event drop min_entry;
alter table event drop max_entry;
alter table event drop supp;
alter table event drop cap ;
alter table event drop school_cap ;
alter table event drop blurb ;
alter table event drop deadline ;
alter table event drop ballot ;
alter table event drop alumni ;
alter table event drop field_report;
alter table event drop ballot_type ;
alter table event drop allow_judge_own ;
alter table event drop waitlist ;
alter table event drop waitlist_all;
alter table event drop no_codes;
alter table event drop reg_codes;
alter table event drop initial_codes;
alter table event drop bids;
alter table event drop no_judge_burden ;
alter table event drop live_updates;
alter table event drop omit_sweeps;
alter table event drop ask_for_titles;

alter table strike change bin strike_time int;
alter table strike change strike registrant tinyint;
alter table strike drop timeslot; 
alter table strike change tournament tourn int;


alter table tourn drop results;
alter table tourn drop judge_deadline;
alter table tourn drop drop_deadline;
alter table tourn drop freeze_deadline;
alter table tourn drop fine_deadline;
alter table tourn drop judge_policy;
alter table tourn drop invitename;
alter table tourn drop inviteurl;
alter table tourn drop bill_packet;
alter table tourn drop disclaimer;
alter table tourn drop invoice_message;
alter table tourn drop ballot_message;
alter table tourn drop chair_ballot_message;
alter table tourn drop web_message;
alter table tourn drop vlabel;
alter table tourn drop hlabel;
alter table tourn drop vcorner;
alter table tourn drop hcorner;
alter table tourn change league circuit int;
alter table tourn drop method;
alter table tourn drop flighted;
alter table tourn drop director; 

alter table tourn_admin change entry entry_only tinyint;

alter table changes rename to tourn_change;
alter table tourn_change change panel new_panel int;
alter table tourn_change change comp entry int;
alter table tourn_change change moved_from old_panel int;
alter table tourn_change add account int;
alter table tourn_change change regline text varchar(255);

alter table email change tournament tourn int;
alter table email change senton sent_on datetime;
alter table email change blurb content text;

drop table judge_class;
alter table account_ignore change tournament tourn int;
alter table file change tournament tourn int;
alter table housing_slots change tournament tourn int;
alter table judge_hire change tournament tourn int;
alter table rating change tournament tourn int;
alter table rating_tier change tournament tourn int;
alter table room drop tournament;
alter table room_strike change tournament tourn int;
alter table school change tournament tourn int;
alter table school_fine change tournament tourn int;
alter table session change tournament tourn int;
alter table tiebreak_set change tournament tourn int;
alter table timeslot change tournament tourn int;
alter table tourn_admin change tournament tourn int;
alter table tourn_change change tournament tourn int;
alter table tourn_site change tournament tourn int;

alter table student add account int not null default 0;
alter table student add acct_request int;
alter table student add created datetime;
update student set account=0 where account is null;

alter table tiebreak change tiebreaker name varchar(15);
alter table tiebreak add highlow int;
alter table tiebreak drop covers; 
alter table tiebreak drop round;
alter table tiebreak drop event;
alter table tiebreak drop type;
alter table tiebreak drop tourn;
alter table tiebreak drop method;
alter table tourn drop circuit;
alter table tourn_admin change nosetup no_setup tinyint;
alter table tourn_admin add event int;
alter table site drop approved;
alter table session drop ie_annoy;
alter table session drop director;
alter table session change entry entry_only bool;
alter table session change league circuit int;

alter table room_strike add entry int;
alter table room_strike add judge int;
alter table room_strike add start datetime;
alter table room_strike add end datetime;
alter table room add building varchar(64);
alter table rating_tier drop tourn;

alter table rating_tier change type type enum('mpj','qual','coach');
update rating_tier set type="coach" where type="qual";
alter table rating_tier change type type enum('mpj','coach');
alter table rating_subset add timestamp timestamp;

alter table rating add timestamp timestamp;
alter table rating drop value;
drop table pool_round;
alter table pool drop timeslot;
alter table panel drop event;
alter table panel drop nosweep;
alter table housing_slots add timestamp timestamp;
alter table follow_judge add timestamp timestamp;
alter table follow_entry add timestamp timestamp;
alter table follow_account add timestamp timestamp;
alter table file add timestamp timestamp;
alter table file add circuit int;
alter table event change tournament tourn int;
alter table event change type type varchar(15);

alter table event drop code; 
alter table event drop team; 
alter table event drop double_flight; 
alter table event drop double_factor; 
alter table event drop reg_blockable; 
alter table event drop textpost; 
alter table event drop bid_cume; 
alter table event change qual_subset rating_subset int; 
alter table event drop publish; 
alter table event add timestamp timestamp;


alter table membership rename to circuit_membership;
alter table circuit_membership change league circuit int;
alter table circuit_membership change blurb text text;

alter table circuit add country char(4);
alter table circuit change short_name abbr varchar(15);
update circuit set country="US";
update circuit set state="" where state="US";
update circuit set state="VA" where state="Va";
update circuit set name="US National Circuit" where id=6;

  alter table circuit drop url;
  alter table circuit drop public_email;
  alter table circuit drop admin;
  alter table circuit drop dues_to;
  alter table circuit drop dues_amount;
  alter table circuit drop hosted_site;
  alter table circuit drop apda_seeds;
  alter table circuit drop logo_file;
  alter table circuit drop site_theme;
  alter table circuit drop public_results;
  alter table circuit drop header_file;
  alter table circuit drop invoice_message;
  alter table circuit drop track_bids;
  alter table circuit drop last_change;
  alter table circuit drop approved;
  alter table circuit drop tourn_only;
  alter table circuit drop full_members;


rename table account_ignore to tourn_ignore;

alter table account change noemail no_email bool;
alter table account add started_judging date;

alter table account ENGINE=innodb;
alter table ballot ENGINE=innodb;
alter table ballot_value  ENGINE=innodb;
alter table chapter  ENGINE=innodb;
alter table chapter_admin  ENGINE=innodb;
alter table chapter_circuit  ENGINE=innodb;
alter table chapter_judge  ENGINE=innodb;
alter table circuit  ENGINE=innodb;
alter table circuit_admin  ENGINE=innodb;
alter table circuit_dues ENGINE=innodb;
alter table circuit_membership ENGINE=innodb;
alter table circuit_setting  ENGINE=innodb;
alter table concession ENGINE=innodb;
alter table concession_purchase  ENGINE=innodb;
alter table email  ENGINE=innodb;
alter table entry  ENGINE=innodb;
alter table entry_student  ENGINE=innodb;
alter table event  ENGINE=innodb;
alter table event_double ENGINE=innodb;
alter table event_setting  ENGINE=innodb;
alter table file ENGINE=innodb;
alter table follow_account ENGINE=innodb;
alter table follow_entry ENGINE=innodb;
alter table follow_judge ENGINE=innodb;
alter table housing_slots  ENGINE=innodb;
alter table housing  ENGINE=innodb;
alter table judge  ENGINE=innodb;
alter table judge_group  ENGINE=innodb;
alter table judge_group_setting  ENGINE=innodb;
alter table judge_hire ENGINE=innodb;
alter table judge_setting  ENGINE=innodb;
alter table panel  ENGINE=innodb;
alter table pool ENGINE=innodb;
alter table pool_judge ENGINE=innodb;
alter table qualifier  ENGINE=innodb;
alter table rating ENGINE=innodb;
alter table rating_subset  ENGINE=innodb;
alter table rating_tier  ENGINE=innodb;
alter table region ENGINE=innodb;
alter table region_admin ENGINE=innodb;
alter table region_fine  ENGINE=innodb;
alter table room ENGINE=innodb;
alter table room_pool  ENGINE=innodb;
alter table room_strike  ENGINE=innodb;
alter table round  ENGINE=innodb;
alter table school ENGINE=innodb;
alter table school_fine  ENGINE=innodb;
alter table session  ENGINE=innodb;
alter table strike  ENGINE=innodb;
alter table strike_time  ENGINE=innodb;
alter table student ENGINE=innodb;
alter table student_result  ENGINE=innodb;
alter table site ENGINE=innodb;
alter table tourn ENGINE=innodb;
alter table tourn_admin ENGINE=innodb;
alter table tourn_change ENGINE=innodb;
alter table tourn_ignore ENGINE=innodb;
alter table tourn_site ENGINE=innodb;

create index tourn on tourn_change(tourn);
create index new_panel on tourn_change(new_panel);
create index old_panel on tourn_change(old_panel);
create index entry on tourn_change(entry);

create index entry on entry_student(entry);
create index student on entry_student(student);
