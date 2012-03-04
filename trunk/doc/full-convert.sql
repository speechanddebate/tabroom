alter table method add hide_codes bool;
alter table judge_group add track_judge_hires bool;
alter table judge_group add hired_pool int;
alter table judge_group add hired_fee int;
alter table event add qual_cume int;
alter table student_result modify student int null;
alter table student_result add chapter int null;
alter table method add bid_min_number int null;
alter table judge add spare_pool bool;
alter table method add elim_method varchar(63);
alter table ballot add seed int;
update method set elim_method = "snake" where elim_method is null;
create index student_result on student_result(student);
alter table judge_group add special varchar(127);
create index id on student(id);
create index student on comp(student);
create index partner on comp(partner);
create index chapter on school(chapter);
alter table judge_group add free int;
create index school on judge(school);
alter table judge_group add abbr varchar(15);
create index judge on judge_class(judge);
create index judge on judge_class(class);
alter table pool add name varchar(63);
alter table pool drop timeslot;
alter table ballot add rankinround int;
create index timeslot on round (timeslot);
alter table strike drop judge_group int null;
alter table strike drop fine int null;
alter table strike drop name varchar(63) null;

create table bin ( 
	id int auto_increment primary key,
	judge_group int not null,
	fine int,
	name varchar(63),
	start datetime,
	end datetime );

alter table strike add bin int;
alter table judge add hire int;
alter table school add contact_number varchar(15);
alter table judge modify spare_pool bool not null default 0;

create table pool_round (
	id int auto_increment primary key,
	pool int not null,
	round int not null
);

alter table school add contact_name varchar(127);
alter table method add allow_neutral_judges bool;
alter table event add omit_sweeps bool;
alter table method add ask_qualifying_tournament bool;
alter table comp add qualifier varchar(63);
alter table account add saw_judgewarn bool;
alter table method add elim_method_basis varchar(15);
alter table event add no_judge_burden bool not null default 0;
alter table judge_group add collective bool;
alter table pool add prelim bool;
alter table judge add prelim_pool int;
alter table judge_group add track_by_pools bool;
alter table judge_group add default_alt_reduce int;
alter table judge_group add reduce_alt_burden int;
alter table judge_group add alt_max int;
alter table judge modify alt_group int  not null default 0;
alter table panel drop tournament;
alter table panel drop num_regions;
alter table round add ordered bool;
alter table round drop preprep;
alter table panel drop preset;
alter table round add preset bool not null default 0;
alter table chapter add state char(4);
alter table student add phonetic varchar(127);
alter table method add judge_cells bool;
alter table judge add cell varchar(63);
alter table method add track_first_year bool not null default 0;
alter table judge add first_year bool not null default 0;
alter table round add no_first_year bool not null default 0;
alter table event add reg_blockable bool not null default 0;
alter table comp add housing_start datetime;
alter table comp add housing_end datetime;
alter table comp add partner_housing_end datetime;
alter table comp add partner_housing_start datetime;
alter table ballot add chair bool not null default 0;
alter table tournament add chair_ballot_message text;
alter table pool add timeslot int;
alter table pool add standby bool not null default 0;
alter table pool modify prelim bool not null default 0;

create table item (
	id int auto_increment primary key,
	name varchar(63),
	price float,
	description text,
	tournament int,
	timestamp timestamp,
	event int );

create table purchase ( 
	id int auto_increment primary key,
	item int not null,
	quantity int not null,
	timestamp timestamp,
	school int not null
	);

alter table school add concession_paid_amount float;
alter table method add publish_schools bool not null default 0;
alter table round add published bool not null default 0;

alter table method add incremental_school_codes bool not null default 0;
alter table method add first_school_code varchar(15);
alter table method add schemat_school_code bool not null default 0;
alter table method add schemat_display varchar(15)  not null default 0;
alter table method add per_student_fee float not null default 0;
alter table judge add paradigm text;
alter table judge_group add ask_paradigm bool not null default 0;
alter table judge_group add paradigm_explain text;
alter table method add publish_paradigms bool not null default 0;
alter table account_access add contact bool not null default 0;
alter table method add must_pay_dues bool not null default 0;

create table housing (
	id int auto_increment primary key,
	tournament int,
	student int,
	judge int,
	type varchar(7),
	timestamp timestamp,
	waitlist bool not null default 0,
	night date
);

create table housing_slots ( 
	id int auto_increment primary key,
	tournament int,
	night date, 
	slots int
);

alter table method add no_back_to_back bool not null default 0;
alter table event add ballot_type varchar(24);
alter table method add master_printouts varchar(24);
alter table method add ask_two_quals bool not null default 0;
alter table comp add qual2 varchar(63);
alter table comp add qualexp varchar(63);
alter table comp add qual2exp varchar(63);

create table room_pool ( 
	id int auto_increment primary key,
	tournament int,
	event int,
	timeslot int,
	room int
);

alter table comp add registered_at datetime;
alter table comp add dropped_at datetime;
alter table tournament add entry_deadline datetime;
alter table tournament add fine_free_deadline datetime;

update tournament set entry_deadline = reg_end where entry_deadline is null;
update tournament set fine_free_deadline = reg_end where fine_free_deadline is null;

alter table room add poolonly int not null default 0;
alter table room add poor int not null default 0;
alter table pool add percent int not null default 0;
alter table school modify code varchar(15) null;

create table pool_group (
	id int auto_increment primary key,
	pool int,
	judge_group int 
);

alter table round modify published bool null;
alter table round modify preset bool null;
alter table round modify no_first_year bool null;
alter table tournament add judge_deadline datetime;
update tournament set judge_deadline = reg_end where judge_deadline is null;

update event add dbl_flight bool null;

create table sweep(
	id int auto_increment primary key,
	tournament int not null,
	timestamp timestamp,
	place int,
	value float );

alter table method add sweep_only_place_final bool;
alter table event add deadline datetime;
alter table sweep add prelim_cume int; 
alter table method add track_reg_changes bool;
alter table changes add regline varchar(255);
alter table changes add school int;
alter table account add noemail bool;
alter table event add no_count_doubled bool;
create index ballot_panel on ballot(panel);
alter table round add number_judges int;
alter table comp add title varchar(127);
alter table event add ask_for_titles bool;
alter table judge add covers int;
alter table method add drop_worst_elim int;
alter table method add drop_best_elim int;
alter table method add drop_worst_final int;
alter table method add drop_best_final int;
alter table comp add ada int;
alter table comp add notes varchar(128);

create table resultfile (
	id int auto_increment primary key,
	tournament int not null,
	name varchar(128),
	filename varchar(128),
	timestamp timestamp
);

alter table event add results_cache_valid bool;
alter table event add schemats_cache_valid bool;
alter table judge_group add max_pool_burden float;
alter table pool add ask_type bool;
alter table pool_judge add type varchar(128);
alter table event add supp bool;
Alter table account_access add entry bool;
Alter table session add entry bool;
alter table region add cooke_pts int;
alter table region add sweeps int;
alter table changes modify regline varchar(255);
alter table comp add wins int;
alter table comp add losses int;
alter table ballot add countmenot bool;
alter table qual add lim float;
alter table qual add defaul bool;
alter table qual add strike bool;
alter table method add rating_system varchar(15);
alter table method add rating_covers varchar(15);
alter table method add hide_debate_codes bool;
alter table event add identifier varchar(15);

drop table if exists uberjudge;

create table uber (
	id int auto_increment primary key,
	first varchar(128),
	last varchar(128),
	gender char,
	chapter int,
	started int,
	retired bool,
	notes varchar(254),
	timestamp timestamp,
	created datetime,
	last_judged datetime
);

alter table judge add uber int;

alter table room add capacity int;
alter table room modify quality int;
alter table room add inactive bool;
alter table room add notes varchar(127);
alter table method add noshow_judge_fine_elim int;
alter table league add active bool;
alter table league add state char(4);
update league set active=1;
alter table news add sitewide bool;
alter table news add edited_on datetime;
alter table news add pinned bool;
alter table session add league int;
alter table session add tournament int;
alter table session add userkey varchar(127);

drop table league_mem_type;

create table membership (
	id int auto_increment primary key,
	name varchar(128),
	league int,
	approval bool,
	dues float(12), 
	dues_start datetime,
	dues_expire datetime
);

alter table membership drop open;
alter table membership add approval bool;
alter table membership add blurb text;

alter table chapter_league drop league_mem_type;
alter table chapter_league add paid bool;
alter table chapter_league add membership int;
alter table league add last_change int;

alter table league drop track_novices;
alter table league drop track_dues;
alter table league drop travel_info;
alter table league drop direct_access;
alter table league drop coach_credit;

update student set retired = 0 where retired is null;

alter table school add entered datetime;
alter table school add registered_on datetime;
alter table school add disclaimed int;

Alter table session add school int;
alter table event add no_codes bool;
alter table item add deadline datetime;
alter table method add concession_name varchar(127);
alter table judge_group add deadline datetime;
alter table qual add type enum("school", "coach", "comp") default "coach";
update qual set type="coach" where type is null;

alter table judge_group add ratings_need_paradigms bool;
alter table judge_group add strikes_need_paradigms bool;

alter table judge_group add ratings_need_entry bool;
alter table judge_group add strikes_need_entry bool;

alter table judge_group add school_strikes int;
alter table judge_group add comp_strikes int;

alter table judge_group add coach_ratings bool;
alter table judge_group add school_ratings bool;
alter table judge_group add comp_ratings bool;

alter table uber add cell varchar(15);
alter table uber add paradigm text;

update qual set useme = 1;

alter table account add change_pass_key varchar(255);
alter table account add change_timestamp datetime;
alter table account add change_deadline datetime;
alter table tournament add hidden bool;
alter table session add director int;
alter table judge_group add max_burden int;
create index student_chapter on student(chapter);
alter table session add ie_annoy bool;
alter table email add tournament int;

create table rating (
	id int auto_increment primary key,
	tournament int not null,
	school int,
	comp int,
	type enum("school","comp"),
	qual int,
	entered datetime,
	judge int
);

alter table event add field_report bool;
alter table round add listed bool;
alter table round add created datetime;
alter table round add completed datetime;

create table file ( 
    id int auto_increment primary key,
	tournament int not null,
	label varchar(127),
	school int,
	event int,
	name varchar(127),
	uploaded datetime );


alter table judge_group add pub_assigns bool;
alter table pool add publish bool;
alter table ballot modify speakerorder int not null default 0;
alter table judge_group add obligation_before_strikes bool;
alter table judge add novice bool;
alter table method add track_novice_judges bool;
alter table round add no_novice_judges bool;
alter table method add judge_sheet_notice text;
alter table school add noprefs bool;
alter table tournament add freeze datetime;
update tournament set freeze = reg_end where freeze is null;

alter table ballot drop tournament;
alter table ballot drop question;
alter table ballot drop active;
alter table ballot drop doubled;

alter table tournament drop nonleague;
alter table tournament drop location;
alter table panel drop tmp_score;
alter table panel drop checked;

alter table room drop poor;
alter table room drop availstart;
alter table room drop availend;

alter table pool add type enum("prelim", "elim", "standby");
update pool set type="prelim" where prelim=1;
update pool set type="standby" where standby=1;
update pool set type="elim" where standby!=1 and prelim!=1;

rename table constrain to strike;

alter table tournament add web_message text;
alter table account add multiple int;
alter table session add event int;
alter table method add num_judges int;
alter table method add disable_region_strikes bool;
alter table tournament add approved bool;
alter table league add approved bool;
alter table site add approved bool;
update tournament set approved =1 where approved is null;
update league set approved =1 where approved is null;
update site set approved =1 where approved is null;
alter table account add tz varchar(63);
alter table ballot add collected datetime;
alter table ballot add collected_by int not null default 0;
alter table news add tournament int;
alter table news add display_order int;
alter table news add special varchar(15);
update event set field_report = 0 where field_report is null;
alter table judge_group modify special text;
alter table file add posting bool;

alter table account drop cfl_moderator;
alter table account drop diocese;
alter table account drop fax;

alter table tournament add vcorner float;
alter table tournament add hcorner float;
alter table tournament add vlabel float;
alter table tournament add hlabel float;

alter table league add tourn_only bool;
alter table league add full_members bool;

create table no_interest ( 
	id int auto_increment primary key,
	tournament int,
	account int);


alter table tournament change freeze freeze_deadline datetime;
alter table tournament change entry_deadline drop_deadline datetime;
alter table tournament change fine_free_deadline fine_deadline datetime;

alter table league change create_own_account tourn_only;
alter table league add full_members bool;

update league set tourn_only = 1 where tourn_only is null;
update league set full_members = 1 where full_members is null;
alter table league drop create_own_account;

alter table uber add account int; 

alter table tournament add webname varchar(63);

alter table qual add conflict bool;
alter table qual add judge_group int;
alter table qual add min float;
alter table qual add max float;

alter table tiebreak add round int;
alter table tiebreak add drop varchar(15);  
alter table tiebreak add event int;
alter table tiebreak add type int;

alter table panel add nosweep bool;
alter table event add textpost bool;

create table follow_comp ( 
	id int auto_increment primary key,
	comp int,
	cell varchar(16),
	email varchar(127)
);

create table follow_judge ( 
	id int auto_increment primary key,
	judge int,
	cell varchar(16),
	email varchar(127)
);

alter table comp add initials char;
alter table comp add trpc_string varchar(128);
alter table ballot add flight char;
alter table event add live_updates bool;
update event set live_updates = 0 where live_updates is NULL;

alter table qual drop method;
alter table qual add judge_group int;
update qual set max = lim;
alter table qual drop lim;
alter table qual drop defaul;
alter table qual drop useme;
alter table qual modify type enum("mpj","qual");

alter table pool drop type;
alter table pool drop prelim;
alter table pool drop message;
alter table pool drop percent;
alter table pool drop abbr;
alter table pool drop ask_type;
alter table pool modify standby bool;
alter table pool add registrant bool;
alter table pool add standby bool;
alter table pool add publish bool default 0;
alter table pool add burden int;
alter table pool add judge_group int;
alter table pool drop tournament;

alter table event add judge_group int;
alter table event add waitlist_all bool default 0;
alter table event add alumni bool default 0;
alter table event add reg_codes bool default 0;
alter table event add initial_codes bool default 0;
alter table event add bids bool default 0;
alter table event add min_entry int;
alter table event add max_entry int;
update event set min_entry = 1 where team = 1;
update event set max_entry = 1 where team = 1;
update event set min_entry = 2 where team = 2;
update event set max_entry = 2 where team = 2;
update event set min_entry = 3 where team = 3;
update event set max_entry = 8 where team = 3;
update event,class set event.judge_group = class.judge_group where event.class = class.id;

alter table event drop flight;
alter table event drop allow_flight_double;
alter table event drop method;
alter table event drop class_name;
alter table event drop group_name;
alter table event add bid_cume int;
update event set bid_cume = qual_cume;
alter table event drop qual_cume;
alter table event drop dbl_flight;
alter table event drop no_count_doubled;
alter table event drop schemats_cache_valid;
alter table event drop results_cache_valid;
update event set bids=1 where league_event=1;
alter table event drop league_event;
alter table event drop identifier;

alter table round drop score; 
alter table round drop ordered;
alter table round drop no_novice_judges;

alter table room drop poolonly;
alter table room_pool add reserved bool;
alter table room_pool drop timeslot;

alter table account_access add nosetup bool;

alter table tournament change freeze freeze_deadline datetime;
alter table tournament change entry_deadline drop_deadline datetime;
alter table tournament change fine_free_deadline fine_deadline datetime;

alter table league change create_own_account tourn_only;
alter table league add full_members bool;

update league set tourn_only = 1 where tourn_only is null;
update league set full_members = 1 where full_members is null;
alter table league drop create_own_account;

alter table tiebreak add tb_set int;
create table tiebreak_set ( 
	id int auto_increment primary key,
	name varchar(127),
	tournament int,
	timestamp timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP
);

update class,judge_group set class.tournament = judge_group.tournament where class.judge_group = judge_group.id;
alter table class add max int;
alter table method add double_max int;
alter table judge_group add cumulate_mjp bool;

update method set double_max="2" where double_entry="two_events";
update method set double_max="3" where double_entry="three_events";
update method set double_max="4" where double_entry="four_events";

update method set double_entry="max_events" where double_entry="two_events";
update method set double_entry="max_events" where double_entry="three_events";
update method set double_entry="max_events" where double_entry="four_events";

alter table event modify type enum("debate","speech","congress");
alter table judge add obligation int;

create table qual_subset (
	id int auto_increment primary key, 
	name varchar(63),
	judge_group int
);

alter table event add qual_subset int;
alter table rating modify type enum('school','comp','coach');
alter table rating add subset int;

alter table judge_group add conflicts bool;

alter table rating add name char(3);


