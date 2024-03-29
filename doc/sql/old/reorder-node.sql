
alter table ballot change noshow forfeit bool;

update tiebreak_setting set tag="forfeits_never_break" where tag="noshows_never_break";

alter table ballot add judge_started datetime; 
alter table ballot change account entered_by int;

rename table ballot_value to score;

rename table tourn_change to change_log;

rename table account_setting to person_setting; 
alter table person_setting change account person int;

rename table account_conflict to conflict; 
alter table conflict change conflict conflicted int;

alter table student change acct_request person_request int;
alter table judge change acct_request person_request int;
alter table chapter_judge change acct_request person_request int;

alter table conflict change account person int;
alter table permission change account person int;
alter table tourn_ignore change account person int;
alter table student change account person int;
alter table judge change account person int;
alter table follower change account person int;
alter table chapter_judge change account person int;
alter table housing change account person int;
alter table chapter add ceeb varchar(15);

alter table permission change tag tag varchar(15) after id;

drop table account;
drop table account_ignore;

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

ALTER TABLE tourn_ignore DROP INDEX account;
create INDEX person on tourn_ignore(person);

rename table judge_group to category; 
rename table judge_group_setting to category_setting; 

alter table judge change judge_group category int;
alter table event change judge_group category int;
alter table judge_hire change judge_group category int;
alter table permission change judge_group category int;
alter table rating_subset change judge_group category int;
alter table rating_tier change judge_group category int;
alter table strike_time change judge_group category int;
alter table judge change alt_group alt_category int;
alter table jpool change judge_group category int;

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


alter table chapter_circuit drop active; 
alter table chapter_circuit drop paid; 
alter table chapter_circuit drop created_at; 
alter table chapter_circuit change membership circuit_membership int; 

alter table chapter_judge drop paradigm;
alter table chapter_judge drop identity;

alter table concession add school_cap int;


alter table ballot drop created_at;

alter table ballot change side side bool not null default 0 after id;
alter table ballot change speakerorder speakerorder int after side;
alter table ballot change speechnumber speechnumber int after speakerorder;
alter table ballot change chair chair bool not null default 0 after speechnumber;
alter table ballot change bye bye bool not null default 0 after chair;
alter table ballot change forfeit forfeit bool not null default 0 after bye;
alter table ballot change seed seed int after forfeit;
alter table ballot change pullup pullup bool not null default 0 after seed;
alter table ballot change tv tv bool not null default 0 after pullup;
alter table ballot change audit audit bool not null default 0 after tv;
alter table ballot change judge_started judge_started datetime after audit;
alter table ballot change collected collected datetime after judge_started;
alter table ballot change collected_by collected_by int after collected;
alter table ballot change entered_by entered_by int after collected_by;
alter table ballot change audited_by audited_by int after entered_by;
alter table ballot change cat_id cat_id int after hangout_admin; 
alter table ballot change timestamp timestamp timestamp after hangout_admin; 

alter table change_log change account person int;
alter table change_log change timestamp timestamp timestamp after school; 
alter table change_log change text description text after type; 
alter table change_log change new_panel new_panel int after fine; 
alter table change_log change person person int after description; 
alter table change_log drop created_at; 

alter table circuit drop created_at;











alter table circuit change name name varchar(63) after id;
alter table circuit change timestamp timestamp timestamp after webname;

alter table circuit_membership drop created_at;
alter table circuit_membership add timestamp timestamp;

alter table concession drop created_at;
alter table concession change tourn tourn int after school_cap;
alter table concession change price price decimal(8,2) after name;
alter table concession change timestamp timestamp timestamp after tourn;

alter table concession_purchase drop created_at;
alter table concession_purchase change school school int after fulfilled;
alter table concession_purchase change concession concession int after school;
alter table concession_purchase change timestamp timestamp timestamp after concession;

alter table conflict add type varchar(15) after id;
update tabroom.conflict set type = "chapter" where chapter > 0;
update tabroom.conflict set type = "individual" where conflicted > 0;

alter table conflict drop judge; 
alter table conflict drop created_at; 

alter table conflict change added_by added_by int after chapter;
alter table conflict change timestamp timestamp timestamp after added_by;

rename table event_double to pattern;
alter table pattern drop created_at;
alter table pattern drop min;
alter table pattern change name name varchar(31) after id;
alter table pattern change setting type tinyint after name;
alter table pattern change max max tinyint after type;
alter table pattern change exclude exclude int after max;
alter table pattern change tourn tourn int after exclude;

alter table pattern change timestamp timestamp timestamp after tourn;

alter table email change subject subject varchar(127) after id;
alter table email change content content text after subject;
alter table email change sent_to sent_to varchar(127) after content;
alter table email change sent_on sent_at datetime after sent_to;
alter table email change tourn tourn int after sender;
alter table email drop created_at;
alter table email change timestamp timestamp timestamp after tourn;

alter table entry change code code varchar(63) after id;
alter table entry change name name varchar(127) after code;
alter table entry add active bool not null default 0 after name;

alter table entry change ada ada bool not null default 0 after dq;
alter table entry change tba tba bool not null default 0 after ada;
alter table entry drop self_reg_by;
alter table entry change seed seed varchar(15) after tba;

alter table entry change dropped dropped bool not null default 0 after seed;
alter table entry change waitlist waitlist bool not null default 0 after dropped;
alter table entry add unconfirmed bool not null default 0 after waitlist;
alter table entry change dq dq bool not null default 0 after unconfirmed;

alter table entry add registered_by int after event;
alter table entry change created_at created_at datetime after registered_by;

alter table event change name name varchar(63) after id;
alter table event change abbr abbr varchar(15) after name;
alter table event change type type varchar(15) after abbr;
alter table event change fee fee decimal(8,2) after type;
alter table event change category category int after tourn;
alter table event change event_double pattern int after category;
alter table event drop created_at;
alter table event change timestamp timestamp timestamp after rating_subset;


alter table file add webpage int;
alter table file drop created_at;
alter table file change label label varchar(127) after id;
alter table file change name filename varchar(127) after label;
alter table file change posting posting bool not null default 0 after filename;
alter table file change result result bool not null default 0 after posting;
alter table file change published published bool not null default 0 after posting;
alter table file change uploaded uploaded datetime after published;
alter table file change timestamp timestamp timestamp after webpage;

rename table school_fine to fine; 
alter table fine change reason reason varchar(63) after id;
alter table fine change amount amount decimal(8,2) after reason;
alter table fine change payment payment bool not null default 0 after amount;

alter table fine change levied_on levied_at datetime after tourn;
alter table fine change levied_by levied_by int after levied_at; 

alter table fine change deleted deleted bool not null default 0 after levied_by;
alter table fine change deleted_at deleted_at datetime after deleted; 
alter table fine change deleted_by deleted_by int after deleted_at; 


alter table fine change tourn tourn int after levied_by;
alter table fine change school school int after tourn;
alter table fine change region region int after school;
alter table fine change judge judge int after region;

alter table fine change timestamp timestamp timestamp after judge;
alter table fine drop created_at;

alter table region_fine change tourn tourn int after levied_by;
alter table region_fine change region region int after tourn;
alter table region_fine add school int after region;

alter table follower drop created_at;
alter table follower drop updated_at;
alter table follower change type type varchar(8) after id;
alter table follower change follower follower int after email;

alter table hotel change tourn tourn int after multiple;
alter table hotel drop created_at;


alter table housing change type type varchar(7) after id;
alter table housing change night night date after type;
alter table housing change waitlist waitlist bool not null default 0 after night;
alter table housing change tba tba bool not null default 0 after waitlist;
alter table housing change requested requested datetime after tba;
alter table housing change person requestor int after requested;
alter table housing change timestamp timestamp timestamp after school;
alter table housing drop created_at;

alter table housing_slots change slots slots smallint;
alter table housing_slots change tourn tourn int after slots;
alter table housing_slots drop created_at; 

alter table jpool_judge drop created_at; 
alter table jpool_judge drop type;

alter table jpool_round drop created_at; 

alter table jpool change name name varchar(63) after id;
alter table jpool change category category int after name;
alter table jpool drop tourn;
alter table jpool drop created_at; 

alter table judge_hire change covers entries_requested smallint after id; 
alter table judge_hire change accepted entries_accepted smallint after entries_requested; 
alter table judge_hire change rounds rounds_requested smallint after entries_accepted; 
alter table judge_hire change rounds_accepted rounds_accepted smallint after rounds_requested; 
alter table judge_hire drop created_at; 
alter table judge_hire change request_made requested_at datetime after rounds_accepted;
alter table judge_hire add requestor int after requested_at;

alter table judge change code code smallint after id; 
alter table judge change first first varchar(63) after code;
alter table judge add middle varchar(63) after first;
alter table judge change last last varchar(63) after middle;
alter table judge change active active bool not null default 0 after last; 
alter table judge change ada ada bool not null default 0 after active;
alter table judge change obligation obligation smallint after ada; 
alter table judge change hired hired smallint after obligation; 
alter table judge drop created_at;

alter table judge change timestamp timestamp timestamp after person_request;

alter table login drop name;
alter table login drop salt;
alter table login drop created_at;

alter table login change password password varchar(63) after username;
alter table login change sha512 sha512 varchar(128) after password;
alter table login change spinhash spinhash varchar(128) after sha512;

alter table login change pass_timestamp pass_timestamp datetime after last_access;
alter table login change person person int after source;
alter table login change timestamp timestamp timestamp after ualt_id;

rename table nsda_event_categories to nsda_event_category;
alter table nsda_event_category change name name varchar(31) after id;
alter table nsda_event_category change type type enum("s", "d", "c") after name;
alter table nsda_event_category change nsda_id code smallint after type;
alter table nsda_event_category change nat_category national bool not null default 0 after code;

update person set state="MA" where state="Mass";
update person set state="MA" where state="Massachuset";
update person set state="NY" where state="New York";
update person set state="WA" where state="Washington";
update person set state="UT" where state="ut";
update person set state="Ohio" where state="OH";

alter table person change middle middle varchar(63) after first; 
alter table person drop alt_phone; 
alter table person drop multiple;
alter table person drop flags;
alter table person drop created_at;
alter table person drop started_judging;
alter table person change state state char(4) after city;
alter table person add postal varchar(15) after zip;
alter table person change tz tz varchar(63) after country;
alter table person change phone phone varchar(31) after tz;
alter table person change provider provider varchar(63) after phone;

alter table person change gender gender char after last;
alter table person change pronoun pronoun varchar(63) after gender;
alter table person change no_email no_email bool not null default 0 after pronoun;
alter table person change ualt_id ualt_id int after googleplus;
alter table person change timestamp timestamp timestamp after ualt_id;

alter table qualifier change name name varchar(63) after id;
alter table qualifier change result result varchar(127) after name;
alter table qualifier add qualifier_tourn int after tourn;
alter table qualifier drop created_at; 

alter table rating_subset drop created_at;

alter table rating_tier change type type enum('coach', 'mpj') after id;
alter table rating_tier change name name varchar(15) after type;
alter table rating_tier change description description varchar(255) after name;
alter table rating_tier change strike strike bool not null default 0 after description;
alter table rating_tier change conflict conflict bool not null default 0 after strike;
alter table rating_tier change min min decimal(8,2) after conflict;
alter table rating_tier change max max decimal(8,2) after min;
alter table rating_tier change start start bool not null default 0 after max;
alter table rating_tier drop created_at;
alter table rating_tier drop tourn;
alter table rating_tier change timestamp timestamp timestamp after rating_subset;

delete from rating where timestamp < "2015-07-01 00:00:00";

alter table rating change type type enum('school', 'entry', 'coach') after id;
alter table rating add draft bool;
alter table rating change draft draft bool not null default 0 after type;
alter table rating change entered entered datetime after draft;
alter table rating change ordinal ordinal smallint after entered;
alter table rating change percentile percentile decimal(8,2) after ordinal;
alter table rating drop created_at; 


alter table region drop diocese; 
alter table region change quota quota tinyint after code;
alter table region change circuit circuit int after sweeps;
alter table region change timestamp timestamp timestamp after tourn;

alter table result_set drop created_at; 

alter table result_set change label label varchar(255) after id;
alter table result_set change bracket bracket bool not null default 0 after label;
alter table result_set change published published bool not null default 0 after bracket;
alter table result_set change generated generated datetime after published;

alter table result_value change tag tag varchar(15) after id;
alter table result_value change long_tag description varchar(255) after tag;
alter table result_value change value value varchar(255) after tag;
alter table result_value change priority priority smallint after value;
alter table result_value change no_sort no_sort bool not null default 0 after long_tag;
alter table result_value change sort_desc sort_desc bool not null default 0 after no_sort;
alter table result_value change result result int after sort_desc;
alter table result_value drop created_at; 

alter table result drop created_at;

alter table result change rank rank int after id;
alter table result change percentile percentile int after rank;
alter table result change honor honor varchar(255) after percentile;
alter table result change honor_site honor_site varchar(63) after honor; 
alter table result change result_set result_set int after honor_site;
alter table result change timestamp timestamp timestamp after round;

alter table room_strike drop created_at;
alter table room_strike change type type varchar(15) after id;
alter table room_strike change start start datetime after type;
alter table room_strike change end end datetime after start;
alter table room_strike change room room datetime after end;
alter table room_strike change timestamp timestamp timestamp after judge;

alter table room change building building varchar(31) after id;
alter table room change name name varchar(31) after building;
alter table room change quality quality smallint after name;
alter table room change capacity capacity smallint after quality;
alter table room change inactive inactive bool not null default 0 after capacity;
alter table room change ada ada bool not null default 0 after inactive;
alter table room change notes notes varchar(63) after ada; 
alter table room change timestamp timestamp timestamp after site;

alter table room drop created_at; 

alter table round drop created_at;
alter table round drop pool;
alter table round drop online;

alter table round change type type varchar(15) after id;
alter table round change name name smallint after type;
alter table round change label label varchar(31) after name;
alter table round change flighted flighted tinyint after label;
alter table round change created created datetime after flighted;
alter table round change start_time start_time datetime after created;
alter table round change published published tinyint after flighted;
alter table round change post_results post_results tinyint after published;
alter table round change timestamp timestamp timestamp after tb_set;
alter table round change tb_set tiebreak_set int after site; 

alter table round_setting drop type;

alter table rpool_room drop created_at; 
alter table rpool_round drop created_at; 
alter table rpool drop created_at; 

alter table school drop contact; 
alter table school drop registered_on; 
alter table school drop registered_by; 
alter table school drop entered_on; 

alter table school change name name varchar(127) after id; 
alter table school change code code varchar(15) after name; 
alter table school change registered onsite bool not null default 0 after code;
alter table school change timestamp timestamp timestamp after region;

alter table score change value value decimal(8,2) after tag;
alter table score drop created_at;
alter table score change content content text after value;
alter table score add speech tinyint after content; 
alter table score change position position tinyint after speech; 
alter table score change tiebreak tiebreak tinyint after timestamp;

alter table chapter add address varchar(255) after name;
alter table chapter change city city varchar(63) after address;
alter table chapter change state state char(4) after city;
alter table chapter add zip smallint after state;
alter table chapter add postal varchar(15) after zip;
alter table chapter change country country char(4) after postal;
alter table chapter change coaches coaches varchar(255) after country;
alter table chapter change self_prefs self_prefs bool not null default 0 after coaches;
alter table chapter change level level varchar(15) after self_prefs;
alter table chapter change nsda nsda int after level;
alter table chapter change naudl naudl bool not null default 0 after nsda;
alter table chapter change ipeds ipeds varchar(15) after naudl;
alter table chapter change nces nces varchar(15) after ipeds;
alter table chapter change ceeb ceeb varchar(15) after nces;

alter table chapter drop created_at;
alter table chapter drop district_id;

alter table chapter_circuit change code code varchar(15) after id;
alter table chapter_circuit change full_member full_member bool not null default 0 after code;
alter table chapter_circuit change timestamp timestamp timestamp after circuit_membership;

alter table chapter_judge drop started;
alter table chapter_judge drop created;
alter table chapter_judge drop last_judged;
alter table chapter_judge drop created_at;

alter table chapter_judge change first first varchar(127) after id;
alter table chapter_judge add middle varchar(63) after first;
alter table chapter_judge change last last varchar(127) after middle;

alter table chapter_judge add ada bool not null default 0 after last;
alter table chapter_judge change retired retired bool not null default 0 after ada;
alter table chapter_judge change cell phone varchar(31) after retired;
alter table chapter_judge add email varchar(127) after phone;
alter table chapter_judge change diet diet varchar(255) after email;
alter table chapter_judge change notes notes varchar(255) after diet;
alter table chapter_judge change notes_timestamp notes_timestamp datetime after notes;
alter table chapter_judge change gender gender char after notes_timestamp;
alter table chapter_judge change timestamp timestamp timestamp after person_request;

alter table panel drop created_at;
alter table panel drop type;

alter table panel change letter letter varchar(3) after id;
alter table panel change flight flight varchar(3) after letter;
alter table panel change bye bye bool not null default 0 after flight;
alter table panel change started started datetime after bye;
alter table panel change confirmed confirmed datetime after started;
alter table panel change bracket bracket smallint after confirmed;
alter table panel change room room int after bracket;
alter table panel change round round int after room;

alter table panel change g_event g_event varchar(255) after round;
alter table panel change room_ext_id room_ext_id varchar(255) after g_event;
alter table panel change invites_sent invites_sent bool not null default 0 after room_ext_id;
alter table panel change timestamp timestamp timestamp after invites_sent;
alter table panel change cat_id cat_id int after timestamp;
alter table panel change score score int not null default 0 after timestamp;

alter table site change dropoff dropoff varchar(255) after directions;
alter table site change host host int after dropoff;
alter table site drop created_at;



alter table stats add type varchar(15) after id;
alter table stats change tag tag varchar(31) after type;
alter table stats change value value decimal(8,2) after tag;
alter table stats drop taken;
alter table stats drop created_at;

rename table strike_time to strike_timeslot;
alter table strike_timeslot change name name varchar(31) after id;
alter table strike_timeslot change fine fine smallint after name;
alter table strike_timeslot change start start datetime after fine;
alter table strike_timeslot change end end datetime after start;
alter table strike_timeslot change judge_group category int after end;
alter table strike_timeslot drop created_at;


alter table strike change type type varchar(31) after id;
alter table strike change start start datetime after type;
alter table strike change end end datetime after start;

alter table strike change dioregion dioregion int after region;

alter table strike change strike_time strike_timeslot int after region;
alter table strike change created_at created_at datetime after strike_timeslot;

alter table strike change registrant registrant bool not null default 0 after end;
alter table strike change conflictee conflictee bool not null default 0 after registrant;

alter table strike add timeslot int after region;
alter table strike add entered_by int after strike_timeslot;

alter table student change middle middle varchar(63) after first;

alter table student drop created_at;
alter table student drop created;
alter table student change phonetic phonetic varchar(63) after last;

alter table student change grad_year grad_year smallint after phonetic;
alter table student change novice novice bool not null default 0 after grad_year;
alter table student change retired retired bool not null default 0 after novice;
alter table student change gender gender char after retired;
alter table student change diet diet varchar(31) after gender;
alter table student change birthdate birthdate date after diet; 
alter table student change school_sid school_sid varchar(63) after birthdate;
alter table student change race race varchar(31) after school_sid;
alter table student change ualt_id ualt_id int after race;
alter table student change timestamp timestamp timestamp after person_request;

alter table sweep_event drop created_at; 
alter table sweep_include drop created_at; 
alter table sweep_rule drop created_at; 

alter table sweep_rule change tag tag varchar(31) after id;
alter table sweep_rule change value value varchar(63) after tag;
alter table sweep_rule change place place smallint after value;
alter table sweep_rule change count count varchar(15) after place;

alter table sweep_set drop created_at;
alter table sweep_set drop place;
alter table sweep_set change name name varchar(31) after id;

alter table tiebreak_set drop created_at;
alter table tiebreak_set drop type;
alter table tiebreak_set drop elim;

alter table tiebreak drop created_at;
alter table tiebreak change multiplier multiplier smallint after count;
alter table tiebreak change priority priority smallint after multiplier;
alter table tiebreak change highlow highlow tinyint after priority;
alter table tiebreak change highlow_count highlow_count tinyint after highlow;
alter table tiebreak change tb_set tiebreak_set int after highlow_count;

alter table timeslot drop created_at; 
alter table timeslot change name name varchar(63) after id;
alter table timeslot change tourn tourn int after end;

alter table tourn_circuit drop created_at;
alter table tourn_circuit change approved approved bool not null default 0 after id;

alter table tourn_fee drop created_at;
alter table tourn_fee change tourn tourn int after end;
alter table tourn_fee change amount amount decimal(8,2) after tourn;
alter table tourn_fee change reason reason varchar(63) after amount;

alter table tourn_ignore drop created_at;
alter table tourn_site drop created_at;



alter table tourn drop approved;
alter table tourn drop googleplus;
alter table tourn drop foreign_site;
alter table tourn drop foreign_id;
alter table tourn drop created_at;
alter table tourn drop created_by;

alter table tourn change city city varchar(31) after name;
alter table tourn change state state char(4) after city;
alter table tourn change country country char(4) after state;
alter table tourn change tz tz varchar(31) after country;
alter table tourn change webname webname varchar(64) after tz;

alter table tourn change hidden hidden bool not null default 0 after webname;
alter table tourn change timestamp timestamp timestamp after hidden;

delete from webpage where help = 1;

alter table webpage drop created_at;
alter table webpage drop posted_on;
alter table webpage drop circuit;
alter table webpage drop help;
alter table webpage change title title varchar(63) after id;
alter table webpage change content content text after title;
alter table webpage change active published bool not null default 0 after content;
alter table webpage change sitewide sitewide bool not null default 0 after published;
alter table webpage change special special varchar(15) after sitewide;
alter table webpage change page_order page_order smallint after special;


alter table tabroom.tourn_setting drop created_at; 
alter table tabroom.category_setting drop created_at; 
alter table tabroom.judge_setting drop created_at; 
alter table tabroom.event_setting drop created_at; 
alter table tabroom.entry_setting drop created_at; 
alter table tabroom.round_setting drop created_at; 
alter table tabroom.circuit_setting drop created_at; 
alter table tabroom.person_setting drop created_at; 
alter table tabroom.jpool_setting drop created_at; 
alter table tabroom.rpool_setting drop created_at; 
alter table tabroom.tiebreak_setting drop created_at; 
alter table tabroom.school_setting drop created_at; 

alter table tabroom.tourn_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.category_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.judge_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.event_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.entry_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.round_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.circuit_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.person_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.jpool_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.rpool_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.tiebreak_setting change timestamp timestamp timestamp after setting; 
alter table tabroom.school_setting change timestamp timestamp timestamp after setting; 

alter table tabroom.tiebreak_setting add value_date datetime after value;
alter table tabroom.tiebreak_setting add value_text text after value_date;

rename table tiebreak_setting to tiebreak_set_setting; 

drop table room_pool;
drop table follow_tourn;
drop table circuit_dues;

delete from session where timestamp < "2016-06-01 00:00:00";
create index userkey on session(userkey);

alter table tabroom.session drop authkey;

alter table tabroom.session change userkey userkey varchar(127) after id;
alter table tabroom.session change ip ip varchar(63) after userkey;
alter table tabroom.session change person person int after ip;
alter table tabroom.session change su su int after ip;
alter table tabroom.session change judge_group category int after event;
alter table tabroom.session change timestamp timestamp timestamp after category;

drop table setting;
drop table setting_label;

create table `setting` ( 
	id int NOT NULL AUTO_INCREMENT,
	type varchar(31) NOT NULL,
	subtype varchar(31) NOT NULL,
	tag varchar(31) NOT NULL,
	value_type enum("text", "string", "bool", "integer", "decimal", "datetime", "enum"),
	conditions text,
	timestamp timestamp,
	PRIMARY KEY (`id`),
	KEY `tag` (`tag`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `setting_label` ( 
	id int NOT NULL AUTO_INCREMENT,
	lang char(2),
	label varchar(127),
	guide text,
	options text,
	setting int NOT NULL,
	timestamp timestamp,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `district` ( 
	id int NOT NULL AUTO_INCREMENT,
	name varchar(63),
	code varchar(15),
	location varchar(63),
	chair int,
	timestamp timestamp,
	PRIMARY KEY (`id`),
	UNIQUE KEY `code` (`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `diocese` ( 
	id int NOT NULL AUTO_INCREMENT,
	name varchar(63),
	code varchar(15),
	state char(4),
	quota tinyint,
	active bool not null default 0,
	archdiocese bool not null default 0,
	cooke_award_points smallint,
	timestamp timestamp,
	PRIMARY KEY (`id`),
	UNIQUE KEY `codes` (`active`, `code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

	
