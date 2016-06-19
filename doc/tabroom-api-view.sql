
drop database if exists tabroom_api;
create database tabroom_api;
use tabroom_api;

create view tabroom_api.ballot
	(id, side, bye, forfeit, chair, speakerorder, speechnumber, seed, pullup, tv, audit, collected, judge_started,
		entry_id, judge_id, panel_id, collected_by_id, entered_by_id, hangout_admin )
as select
	id, side, bye, forfeit, chair, speakerorder, speechnumber, seed, pullup, tv, audit, collected, judge_started,
		entry, judge, panel, collected_by, entered_by, hangout_admin
from tabroom.ballot;

create view tabroom_api.change_log
	(id, type, description, 
		person_id, tourn_id, category_id, event_id, webpage_id, school_id, entry_id, judge_id, 
		strike_id, round_id, old_panel_id, new_panel_id, fine_id)
as select
	id, type, description, 
		person, tourn, category, event, webpage, school, entry, judge, 
		strike, round, old_panel, new_panel, fine
from tabroom.change_log;

create view tabroom_api.circuit_membership
	(id, name, circuit_id)
as select
	id, name, circuit
from tabroom.circuit_membership;

create view tabroom_api.circuit
	(id, name, abbr, active, state, country, tz, webname)
as select
	id, name, abbr, active, state, country, tz, webname
from tabroom.circuit;

create view tabroom_api.category
	(id, name, abbr, tourn_id)
as select
	id, name, abbr, tourn
from tabroom.category;

create view tabroom_api.concession_purchase
	(id, quantity, placed, fulfilled, concession_id, school_id)
as select
	id, quantity, placed, fulfilled, concession, school
from tabroom.concession_purchase;

create view tabroom_api.concession
	(id, name, price, description, cap, school_cap, deadline, tourn_id)
as select
	id, name, price, description, cap, school_cap, deadline, tourn
from tabroom.concession;

create view tabroom_api.conflict
	(id, type, person_id, conflicted_id, chapter_id, added_by_id) 
as select
	id, type, person, conflicted, chapter, added_by
from tabroom.conflict;

create view tabroom_api.event_double
	(id, name, type, max, exclude_id)
as select
	id, name, type, max, exclude
from tabroom.event_double;

create view tabroom_api.email
	(id, subject, content, sent_to, sent_at, sender_id, tourn_id, circuit_id)
as select
	id, subject, content, sent_to, sent_at, sender, tourn, circuit
from tabroom.email;

create view tabroom_api.entry
	(id, code, name, dropped, waitlisted, dq, unconfirmed, ada, tba, seed, event_id, school_id, tourn_id, registered_by_id)
as select
	id, code, name, dropped, waitlist, dq, unconfirmed, ada, tba, seed,  event, school, tourn, registered_by
from tabroom.entry;

create view tabroom_api.entry_student
	(entry_id, student_id)
as select
	entry, student
from tabroom.entry_student;

create view tabroom_api.event
	(id, name, type, abbr, fee, tourn_id, category_id, event_double_id, rating_subset_id)
as select
	id, name, type, abbr, fee, tourn, category, event_double, rating_subset
from tabroom.event;

create view tabroom_api.file
	(id, label, filename, published, uploaded_at, circuit_id, tourn_id, event_id, webpage_id, school_id, result_set_id)
as select
	id, label, filename, published, uploaded, circuit, tourn, event, webpage, school, result
from tabroom.file;

create view tabroom_api.school_fine
	(id, reason, amount, payment, school_id, deleted, deleted_at, deleted_by_id, levied_at, levied_by_id)
as select
	id, reason, amount, payment, school, deleted, deleted_at, deleted_by, levied_at, levied_by
from tabroom.school_fine;

/// BREAK POINT HERE

create view tabroom_api.follower
	(id, cell, email, domain, tourn_id, judge_id, entry_id, school_id, person_id)
as select
	id, cell, email, domain, tourn, judge, entry, school, account
from tabroom.follower;

create view tabroom_api.hotel
	(id, name, fee_multiplier, tourn_id)
as select
	id, name, multiple, tourn
from tabroom.hotel;

create view tabroom_api.housing_request
	(id, night, type, tba, waitlist, student_id, judge_id, school_id, requestor_id)
as select
	id, night, type, tba, waitlist, student, judge, school, account
from tabroom.housing;

create view tabroom_api.housing_slots
	(id, night, slots, tourn_id)
as select
	id, night, slots, tourn
from tabroom.housing_slots;

create view tabroom_api.jpool_judge
	(jpool_id, judge_id)
as select
	jpool, judge
from tabroom.jpool_judge;

create view tabroom_api.jpool_round
			(round_id, jpool_id)
as select
			round, jpool
from tabroom.jpool_round;

create view tabroom_api.jpool
	(id, name, category_id, site_id)
as select
	id, name, category, site
from tabroom.jpool;

alter table tabroom.judge_hire add requestor int;

create view tabroom_api.judge_hire
	(id, requested_at, entries_requested, entries_accepted, rounds_requested, rounds_accepted, school_id, tourn_id, judge_id, requestor_id)
as select
	id, request_made, covers, accepted, rounds, rounds_accepted, school, tourn, judge, requestor
from tabroom.judge_hire ;

create view tabroom_api.judge
	(id, first, last, code, active, ada, obligation_rounds, hired_rounds, chapter_judge_id, category_id, school_id, person_id, alt_category_id, covers_category_id)
as select
	id, first, last, code, active, ada, obligation, hired, chapter_judge, category, school, account, alt_category, covers
from tabroom.judge;

alter table tabroom.login add sha512 char(128);
alter table tabroom.login add spinhash char(64);

create view tabroom_api.login
	(id, username, password, sha512, spinhash, name, accesses, last_access, pass_changekey, pass_pass_change_expires, source, ualt_id, person_id)
as select
	id, username, password, sha512, spinhash, name, accesses, last_access, pass_changekey, pass_pass_change_expires, source, ualt_id, person
from tabroom.login;

alter table tabroom.person add middle varchar(63);
alter table tabroom.person add pronoun varchar(63);
alter table tabroom.person add postal varchar(15);

create view tabroom_api.person
	(id, email, first, middle, last, phone, alt_phone, provider, street, city, state, zip, postal, country, gender, pronouns, ualt_id, site_admin, no_email, multiple, tz, diversity, flags)
as select
	id, email, first, middle, last, phone, alt_phone, provider, street, city, state, zip, postal, country, gender, pronoun, ualt_id, site_admin, no_email, multiple, tz, diversity, flags, timestamp
from tabroom.person;

create view tabroom_api.jpool_judge
	(judge_id, jpool_id)
as select
			judge, jpool
from tabroom.jpool_judge;

alter table tabroom.qualifier add qualified_at int;

create view tabroom_api.qualifier
	(id, tag, value, entry_id, tourn_id, qualified_at_id)
as select
	id, name, result, entry, tourn, qualified_at
from tabroom.qualifier;

create view tabroom_api.rating_subset
	(id, name, category_id)
as select
	id, name, category
from tabroom.rating_subset;

create view tabroom_api.rating_tier
	(id, name, type, description, strike, conflict, min, max, default_tier, rating_subset_id, category_id)
as select
	id, name, type, description, strike, conflict, min, max, start, rating_subset, category
from tabroom.rating_tier;

alter table tabroom.rating add draft bool;

create view tabroom_api.rating
	(id, ordinal, percentile, type, draft, sheet, school_id, entry_id, judge_id, rating_tier_id, rating_subset_id)
as select
	id, ordinal, percentile, type, draft, sheet, school, entry, judge, rating_tier, rating_subset
from tabroom.rating;

alter table tabroom.region add active bool;

create view tabroom_api.region
	(id, name, code, active, ncfl_quota, ncfl_archdiocese, ncfl_cooke, ncfl_sweeps, circuit_id, tourn_id)
as select
	id, name, code, active, quota, arch, cooke_pts, sweeps, circuit, tourn
from tabroom.region;

create view tabroom_api.result_set
	(id, label, bracket, published, tourn_id, event_id)
as select
	id, label, bracket, published, tourn, event
from tabroom.result_set;

create view tabroom_api.result_value
	(id, tag, value, priority, no_sort, sort_descending, description, result_id)
as select
	id, tag, value, priority, no_sort, sort_desc, long_tag, result
from tabroom.result_value;

create view tabroom_api.result
	(id, rank, percentile, honor, honor_site, student_id, entry_id, school_id, round_id, result_set_id)
as select
	id, rank, percentile, honor, honor_site, student, entry, school, round, result_set
from tabroom.result;

create view tabroom_api.room_strike
	(id, type, start, end, room_id, event_id, tourn_id, judge_id, entry_id)
as select
	id, type, start, end, room, event, tourn, judge, entry
from tabroom.room_strike;

create view tabroom_api.room
	(id, name, building, quality, capacity, inactive, ada, site_id)
as select
	id, name, building, quality, capacity, inactive, ada, site
from tabroom.room;

create view tabroom_api.round
	(id, number, name, type, published, post_results, flighted, start_time, site_id, timeslot_id, event_id, tiebreak_set_id)
as select
	id, name, label, type, published, post_results, flighted, start_time, site, timeslot, event, tb_set
from tabroom.round;

create view tabroom_api.rpool_room
	(rpool_id, room_id)
as select
	rpool, room
from tabroom.rpool_room;

create view tabroom_api.rpool_round
	(round_id, rpool_id)
as select
			round, rpool
from tabroom.rpool_round;

create view tabroom_api.rpool
	(id, name, tourn_id)
as select
	id, name, tourn
from tabroom.rpool;

use tabroom;
DROP TABLE IF EXISTS `chapter_contact`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_contact` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create view tabroom_api.school
	(id, name, code, onsite, onsite_at, onsite_by_id, registered_by_id, chapter_id, tourn_id)
as select
	id, name, code, registered, registered_on, onsite_by, registered_by, entered_on, chapter, tourn
from tabroom.chapter;

alter table tabroom.score add speech smallint;

update tabroom.score, tabroom.ballot 
	set tabroom.score.speech = tabroom.ballot.speechnumber 
	where tabroom.ballot.id = tabroom.score.ballot 
	and tabroom.ballot.speechnumber > 0;

create view tabroom_api.score
	(id, tag, value, content, position, speech, student_id, ballot_id)
as select
	id, tag, value, content, position, speech, student, ballot
from tabroom.score;

create view tabroom_api.panel
	(id, letter, flight, bye, started, confirmed, bracket, room_id, round_id)
as select
	id, letter, flight, bye, started, confirmed, bracket, room, round
from tabroom.panel;

create view tabroom_api.site
	(id, name, directions, host_id, circuit_id)
as select
	id, name, directions, host, circuit
from tabroom.site;

create view tabroom_api.chapter_circuit
	(id, region_id, chapter_id, circuit_id, circuit_membership_id)
as select
	id, region, chapter, circuit, membership
from tabroom.chapter_circuit;

alter table tabroom.chapter add address varchar(255);
alter table tabroom.chapter add zip int;
alter table tabroom.chapter add postal varchar(16);

create view tabroom_api.chapter
	(id, name, address, city, state, zip, postal, country, coaches, level, naudl, ipeds, nces, nsda, ceeb)
as select
	id, name, address, city, state, zip, postal, country, coaches, level, naudl, ipeds, nces, nsda, ceeb, timestamp
from tabroom.chapter;

alter table tabroom.chapter_judge add ada bool;
alter table tabroom.chapter_judge add email varchar(128);

create view tabroom_api.chapter_judge
	(id, first, last, gender, retired, cell, email, diet, ada, notes, notes_chapter_id, person_id, request_person_id)
as select
	id, first, last, gender, retired, cell, email, diet, ada, notes, notes_chapter, account, acct_request, 
from tabroom.chapter_judge;

alter table tabroom.stats add type varchar(15);

create view tabroom_api.stats
	(id, type, tag, value, event_id)
as select
	id, type, tag, value, taken, event
from tabroom.stats;

create view tabroom_api.strike_timeslots
	(id, name, start, end, fine, category_id)
as select
	id, name, start, end, fine, category
from tabroom.strike_time;

alter table tabroom.strike add hidden bool;
alter table tabroom.strike add timeslot int;
alter table tabroom.strike add entered_by int;

create view tabroom_api.strikes
	(id, type, start, end, hidden, region_id, timeslot_id, school_id, entry_id, judge_id, entered_by_id, strike_timeslot_id)
as select
	id, type, start, end, hidden, region, timeslot, chapter, entry, judge, entered_by, strike_time
from tabroom.strike;

alter table tabroom.student add middle varchar(128);

create view tabroom_api.students
	(id, first, middle, last, grad_year, novice, retired, gender, phonetic, ualt_id, chapter_id, person_id)
as select
	id, first, middle, last, grad_year, novice, retired, gender, phonetic, ualt_id, chapter, account
from tabroom.student;

create view tabroom_api.sweep_events
	(sweep_set_id, event_id)
as select
	sweep_set, event
from tabroom.sweep_event;

create view tabroom_api.sweep_include
	(id, child_id, parent_id)
as select
	id, child, parent
from tabroom.sweep_include;

create view tabroom_api.sweep_rules
	(id, tag, value, place, count, sweep_set_id)
as select
	id, tag, value, place, count, sweep_set
from tabroom.sweep_rule;

create view tabroom_api.sweep_sets
	(id, name)
as select
	id, name, timestamp
from tabroom.sweep_set;

create view tabroom_api.tiebreak_sets
	(id, name, tourn_id)
as select
	id, name, tourn
from tabroom.tiebreak_set;

create view tabroom_api.tiebreaks
	(id, name, count, multiplier, priority, highlow, highlow_count, tiebreak_set_id)
as select
	id, name, count, multiplier, priority, highlow, highlow_count, tb_set
from tabroom.tiebreak;

create view tabroom_api.timeslots
	(id, name, start, end, tourn_id)
as select
	id, name, start, end, tourn
from tabroom.timeslot;

create view tabroom_api.tourn_circuits
	(tourn_id, circuit_id)
as select
			tourn, circuit
from tabroom.tourn_circuit;

create view tabroom_api.tourn_fees
	(id, amount, reason, start, end, tourn_id)
as select
	id, amount, reason, start, end, tourn
from tabroom.tourn_fee;

create view tabroom_api.tourn_ignore
	(person_id, tourn_id)
as select
			account, tourn
from tabroom.tourn_ignore;

create view tabroom_api.tourn_sites
	(tourn_id, site_id)
as select
			tourn, site
from tabroom.tourn_site;

alter table tabroom.tourn add city varchar(128);

create view tabroom_api.tourns
	(id, name, start, end, reg_start, reg_end, hidden, webname, tz, city, state, country)
as select
	id, name, start, end, reg_start, reg_end, hidden, webname, tz, city, state, country, timestamp
from tabroom.tourn;

alter table tabroom.webpage add creator int;

create view tabroom_api.webpages
	(id, title, content, published, sitewide, page_order, creator_id, editor_id, tourn_id, circuit_id)
as select
	id, title, content, active, sitewide, page_order, creator, last_editor, tourn, circuit
from tabroom.webpage;

alter table tabroom.tourn_setting add setting int; 
alter table tabroom.category_setting add setting int; 
alter table tabroom.judge_setting add setting int; 
alter table tabroom.event_setting add setting int; 
alter table tabroom.entry_setting add setting int; 
alter table tabroom.round_setting add setting int; 
alter table tabroom.circuit_setting add setting int; 
alter table tabroom.account_setting add setting int; 
alter table tabroom.jpool_setting add setting int; 
alter table tabroom.rpool_setting add setting int; 
alter table tabroom.tiebreak_setting add setting int; 
alter table tabroom.chapter_setting add setting int; 

create view tabroom_api.circuit_settings 
	(id, tag, value, value_text, value_date, circuit_id, setting_id )
as select 
	id, tag, value, value_text, value_date, circuit, setting
from tabroom.circuit_setting ;

create view tabroom_api.tourn_settings 
	(id, tag, value, value_text, value_date, tourn_id, setting_id )
as select 
	id, tag, value, value_text, value_date, tourn, setting
from tabroom.tourn_setting ;

create view tabroom_api.category_settings 
	(id, tag, value, value_text, value_date, category_id, setting_id )
as select 
	id, tag, value, value_text, value_date, category, setting
from tabroom.category_setting ;

create view tabroom_api.event_settings 
	(id, tag, value, value_text, value_date, event_id, setting_id )
as select 
	id, tag, value, value_text, value_date, event, setting
from tabroom.event_setting ;

create view tabroom_api.entry_settings 
	(id, tag, value, value_text, value_date, entry_id, setting_id )
as select 
	id, tag, value, value_text, value_date, entry, setting
from tabroom.entry_setting;

create view tabroom_api.jpool_settings 
	(id, tag, value, value_text, value_date, jpool_id, setting_id )
as select 
	id, tag, value, value_text, value_date, jpool, setting
from tabroom.jpool_setting ;

create view tabroom_api.rpool_settings 
	(id, tag, value, value_text, value_date, rpool_id, setting_id)
as select 
	id, tag, value, value_text, value_date, rpool, setting
from tabroom.rpool_setting ;

alter table tabroom.tiebreak_setting add value_date datetime;
alter table tabroom.tiebreak_setting add value_text text;

create view tabroom_api.tiebreak_set_settings 
	(id, tag, value, value_text, value_date, tiebreak_set_id, setting_id)
as select 
	id, tag, value, value_text, value_date, tiebreak_set, setting
from tabroom.tiebreak_setting ;

create view tabroom_api.judge_settings 
	(id, tag, value, value_text, value_date, judge_id, setting_id)
as select 
	id, tag, value, value_text, value_date, judge, setting
from tabroom.judge_setting ;

create view tabroom_api.round_settings 
	(id, tag, value, value_text, value_date, round_id, setting_id)
as select 
	id, tag, value, value_text, value_date, round, setting
from tabroom.round_setting ;

create view tabroom_api.person_settings 
	(id, tag, value, value_text, value_date, person_id, setting_id)
as select 
	id, tag, value, value_text, value_date, account, setting
from tabroom.account_setting ;

create view tabroom_api.school_settings 
	(id, tag, value, value_text, value_date, school_id, setting_id)
as select 
	id, tag, value, value_text, value_date, chapter, setting
from tabroom.chapter_setting ;

#PERMISSIONS

alter table tabroom.permission add created_at datetime;
create view tabroom_api.permissions
	(id, person_id, tourn_id, region_id, chapter_id, circuit_id, category_id, tag)
	as select 
	id, account, tourn, region, chapter, circuit, category, tag, timestamp
from tabroom.permission;

create table `nsda_points` (
	id int NOT NULL AUTO_INCREMENT,
	points int NOT NULL DEFAULT 0,
	venue  varchar(127),
	event_name varchar(63),
	level enum("hs", "college", "middle", "elem"),
	source enum("T", "J", "M", "O"),
	results text,

	person_id int NOT NULL,

	coach_id int NULL,
	tourn_id int NULL,
	event_id int NULL,
	chapter_id int NULL,
	student_id int NULL,
	recorder_id int NULL,
	nsda_category_id int NULL, 

	PRIMARY KEY (`id`),
	KEY `person_id` (`person_id`),
	KEY `coach_id` (`coach_id`),
	KEY `student_id` (`student_id`),
	KEY `chapter_id` (`chapter_id`),
	KEY `recorder_id` (`recorder_id`),
	KEY `tourn_id` (`tourn_id`),
	KEY `event_id` (`event_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `nsda_honors` ( 
	id int NOT NULL AUTO_INCREMENT,

	tag varchar(63) NOT NULL,
	value int NOT NULL DEFAULT 0,
	venue  varchar(127),
	event_name varchar(63),
	level enum("hs", "college", "middle", "elem"),
	source enum("T", "J", "M", "O"),
	results text,

	tourn_id int NULL,
	event_id int NULL,
	coach_id int NULL,
	person_id int NULL,
	chapter_id int NULL,
	student_id int NULL,
	recorder_id int NULL,

	PRIMARY KEY (`id`),
	KEY `person_id` (`person_id`),
	KEY `coach_id` (`coach_id`),
	KEY `student_id` (`student_id`),
	KEY `recorder_id` (`recorder_id`),
	KEY `tourn_id` (`tourn_id`),
	KEY `event_id` (`event_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `nsda_categories` ( 
	id int NOT NULL AUTO_INCREMENT,
	label varchar(127),
	type enum("speech", "debate", "congress"),
	national bool,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `settings` ( 
	id int NOT NULL AUTO_INCREMENT,

	type varchar(31) NOT NULL,
	tag varchar(31) NOT NULL,
	category varchar(31) NOT NULL,
	value_type enum("text", "string", "bool", "integer", "float", "datetime", "enum"),
	conditions text,

	description text,
	full_description text,

	PRIMARY KEY (`id`),
	KEY `tag` (`tag`),
	KEY `category` (`category`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `setting_labels` ( 
	id int NOT NULL AUTO_INCREMENT,
	lang char(2),
	label varchar(127),
	guide text,
	options text,
	setting_id int NOT NULL,
	PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

