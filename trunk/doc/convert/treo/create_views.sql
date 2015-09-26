
drop database treo;
create database treo;
use treo;

drop database if exists treo;
create database treo;

create view treo.ballots 
	(id, side, bye, forfeit, chair, speaker_order, speech_number, collected_at, created_at, updated_at, entry_id, judge_id, section_id, collected_by_id, entered_by_id, audited_by_id )
as select
	id, side, bye, noshow, chair, speakerorder, speechnumber, collected, created_at, timestamp, entry, judge, panel, collected_by, account, audited_by
from tabroom.ballot;

alter table tabroom.tourn_change add webpage int;
alter table tabroom.tourn_change add judge_group int;
alter table tabroom.tourn_change add strike int;

create view treo.change_logs
	(id, type, description, created_at, updated_at, tourn_id, class_id, event_id, webpage_id, squad_id, entry_id, judge_id, strike_id, round_id, section_id, new_section_id)
as select
	id, type, text, created_at, timestamp, tourn, judge_group, event, webpage, school, entry, judge, strike, round, panel, new_panel
from tabroom.tourn_change;

create view treo.circuit_dues
	(id, amount, paid_on, created_at, updated_at, circuit_id, school_id)
as select
	id, amount, paid_on, created_at, timestamp, circuit, chapter
from tabroom.circuit_dues;

alter table tabroom.circuit_membership add region_required bool;
alter table tabroom.circuit_membership add timestamp timestamp;
create view treo.circuit_memberships
	(id, name, approval_required, region_required, dues_required, dues_amount, dues_fiscal_year, description, created_at, updated_at, circuit_id)
as select
	id, name, approval, region_required, dues, dues, dues_start, text, created_at, timestamp, circuit
from tabroom.circuit_membership;

create view treo.circuits
	(id, name, abbr, active, state, country, tz, webname, created_at, updated_at)
as select
	id, name, abbr, active, state, country, tz, webname, created_at, timestamp
from tabroom.circuit;

create view treo.classes
	(id, name, abbr, created_at, updated_at, tourn_id)
as select
	id, name, abbr, created_at, timestamp, tourn
from tabroom.judge_group;

create view treo.concession_purchases
	(id, quantity, fulfilled, created_at, updated_at, concession_id, squad_id)
as select
	id, quantity, fulfilled, created_at, timestamp, concession, school
from tabroom.concession_purchase;

alter table tabroom.concession add school_cap int;

create view treo.concessions
	(id, name, price, description, cap, squad_cap, deadline, created_at, updated_at, tourn_id)
as select
	id, name, price, description, cap, school_cap, deadline, created_at, timestamp, tourn
from tabroom.concession;

create view treo.conflicts
	(id, created_at, updated_at, person_id, target_person_id, creator_id, target_school_id)
as select
	id, created_at, timestamp, account, conflict, added_by, chapter
from tabroom.account_conflict;

create view treo.double_entry_sets
	(id, name, type, max, created_at, updated_at, mutex_id)
as select
	id, name, setting, max, created_at, timestamp, exclude
from tabroom.event_double;

create view treo.emails
	(id, subject, content, recipients, created_at, updated_at, sender_id, tourn_id, circuit_id)
as select
	id, subject, content, sent_to, created_at, timestamp, sender, tourn, circuit
from tabroom.email;

create view treo.entries
	(id, code, name, dropped, waitlisted, dq, register_seed, pairing_seed, ada, registered_by, tba, created_at, updated_at, event_id, squad_id, tourn_id)
as select
	id, code, name, dropped, waitlist, dq, seed, pair_seed, ada, reg_by, tba, created_at, timestamp, event, school, tourn
from tabroom.entry;

create view treo.entry_students
	(created_at, updated_at, entry_id, student_id)
as select
			created_at, timestamp, entry, student
from tabroom.entry_student;

create view treo.events
	(id, name, type, abbr, fee, created_at, updated_at, tourn_id, class_id, double_entry_set_id)
as select
	id, name, type, abbr, fee, created_at, timestamp, tourn, judge_group, event_double
from tabroom.event;

alter table tabroom.file add public bool;
alter table tabroom.file add webpage int;

create view treo.files
	(id, label, filename, public, created_at, updated_at, circuit_id, tourn_id, event_id, webpage_id, squad_id, result_set_id)
as select
	id, label, name, public, uploaded, timestamp, circuit, tourn, event, webpage, school, result
from tabroom.file;

alter table tabroom.school_fine add tourn int;
alter table tabroom.school_fine add judge int;
alter table tabroom.school_fine add region int;

create view treo.fines
	(id, amount, reason, created_at, updated_at, region_id, tourn_id, squad_id, judge_id, levied_by_id)
as select
	id, amount, reason, levied_on, timestamp, region, tourn, school, judge, levied_by
from tabroom.school_fine;

create view treo.followers
	(id, cell, email, domain, created_at, updated_at, tourn_id, judge_id, entry_id, squad_id, person_id)
as select
	id, cell, email, domain, created_at, timestamp, tourn, judge, entry, school, account
from tabroom.follower;

create view treo.hotels
	(id, name, fee_multiplier, created_at, updated_at, tourn_id)
as select
	id, name, multiple, created_at, timestamp, tourn
from tabroom.hotel;

create view treo.housing_requests
	(id, night, type, tba, waitlist, created_at, updated_at, student_id, judge_id, squad_id, requestor_id)
as select
	id, night, type, tba, waitlist, created_at, timestamp, student, judge, school, account
from tabroom.housing;


create view treo.housing_slots
	(id, night, slots, created_at, updated_at, tourn_id)
as select
	id, night, slots, created_at, timestamp, tourn
from tabroom.housing_slots;

create view treo.jpool_judges
	(created_at, updated_at, jpool_id, judge_id)
as select
	created_at, timestamp, jpool, judge
from tabroom.jpool_judge;

create view treo.jpool_rounds
			(created_at, updated_at, round_id, jpool_id)
as select
			created_at, timestamp, round, jpool
from tabroom.jpool_round;

create view treo.jpools
	(id, name, created_at, updated_at, class_id, site_id)
as select
	id, name, created_at, timestamp, judge_group, site
from tabroom.jpool;

create view treo.judge_hires
	(id, requested_at, entries_requested, entries_accepted, rounds_requested, rounds_accepted, created_at, updated_at, squad_id, tourn_id, judge_id, requestor_id)
as select
	id, request_made, entries_requested, entries_accepted, rounds_requested, rounds_accepted, created_at, timestamp, school, tourn_id, judge_id, requestor_id
from tabroom.judge_hire ;

create view treo.judges
	(id, first, last, code, active, ada, obligation_rounds, hired_rounds, created_at, updated_at, school_judge_id, class_id, squad_id, person_id, alternate_class_id, covers_class_id)
as select
	id, first, last, code, active, ada, obligation, hired, reg_time, timestamp, chapter_judge, judge_group, school, account, alt_group, covers
from tabroom.judge;

alter table login add sha512 char(128);
create view treo.logins
	(id, username, password, sha512, name, accesses, last_access, pass_changekey, pass_timestamp, pass_change_expires, source, ualt_id, created_at, updated_at, person_id)
as select
	id, username, password, sha512, name, accesses, last_access, pass_changekey, pass_timestamp, pass_change_expires, source, ualt_id, created_at, timestamp, person_id
from tabroom.login;

alter table person add middle varchar(63);
create view treo.people
	(id, email, first, middle, last, phone, alt_phone, provider, street, city, state, zip, country, gender, ualt_id, site_admin, no_email, multiple, tz, diversity, flags, created_at, updated_at)
as select
	id, email, first, middle, last, phone, alt_phone, provider, street, city, state, zip, country, gender, ualt_id, site_admin, no_email, multiple, tz, diversity, flags, created_at, timestamp
from tabroom.person;

create view treo.jpool_judges
	(created_at, updated_at, judge_id, jpool_id)
as select
			created_at, timestamp, judge, jpool
from tabroom.jpool_judge;

alter table tabroom.qualifier add qualified_at int;

create view treo.qualifiers
	(id, tag, value, created_at, updated_at, entry_id, tourn_id, qualified_at_id)
as select
	id, name, result, created_at, timestamp, entry, tourn, qualified_at
from tabroom.qualifier;

create view treo.rating_subsets
	(id, name, created_at, updated_at, class_id)
as select
	id, name, created_at, timestamp, judge_group
from tabroom.rating_subset;

create view treo.rating_tiers
	(id, name, type, description, strike, conflict, min, max, default_tier, created_at, updated_at, rating_subset_id, class_id)
as select
	id, name, type, description, strike, conflict, min, max, start, created_at, timestamp, rating_subset, judge_group
from tabroom.rating_tier;

alter table tabroom.rating add draft bool;

create view treo.ratings
	(id, ordinal, percentile, type, created_at, updated_at, squad_id, entry_id, judge_id, rating_tier_id, rating_subset_id, draft, sheet)
as select
	id, ordinal, percentile, type, created_at, timestamp, school, entry, judge, rating_tier, rating_subset, draft, sheet
from tabroom.rating;

alter table region add active bool;

create view treo.regions
	(id, name, code, active, ncfl_quota, ncfl_archdiocese, ncfl_cooke, ncfl_sweeps, created_at, updated_at, circuit_id, tourn_id)
as select
	id, name, code, active, quota, arch, cooke_pts, sweeps, created_at, timestamp, circuit, tourn
from tabroom.region;

create view treo.result_sets
	(id, label, bracket, published, created_at, updated_at, tourn_id, event_id)
as select
	id, label, bracket, published, created_at, timestamp, tourn, event
from tabroom.result_set;

create view treo.result_values
	(id, tag, value, priority, no_sort, sort_descending, description, created_at, updated_at, result_id)
as select
	id, tag, value, priority, no_sort, sort_desc, long_tag, created_at, timestamp, result
from tabroom.result_value;

create view treo.results
	(id, rank, percentile, honor, honor_site, created_at, updated_at, student_id, entry_id, squad_id, result_set_id)
as select
	id, rank, percentile, honor, honor_site, created_at, timestamp, student, entry, school, round, result_set
from tabroom.result;

create view treo.room_strikes
	(id, type, start, end, created_at, updated_at, room_id, event_id, tourn_id, judge_id, entry_id)
as select
	id, type, start, end, created_at, timestamp, room, event, tourn, judge, entry
from tabroom.room_strike;

create view treo.rooms
	(id, name, building, quality, capacity, inactive, ada, created_at, updated_at, site_id)
as select
	id, name, building, quality, capacity, inactive, ada, created_at, timestamp, site
from tabroom.room;

create view treo.rounds
	(id, number, name, type, published, post_results, flighted, start_time, created_at, updated_at, site_id, timeslot_id, event_id, tiebreak_set_id)
as select
	id, name, label, type, published, post_results, flighted, start_time, created_at, timestamp, site, timeslot, event, tiebreak_set
from tabroom.round;

create view treo.rpool_rooms
	(created_at, updated_at, rpool_id, room_id)
as select
	created_at, timestamp, rpool, room
from tabroom.rpool_room;

create view treo.rpool_rounds
	(created_at, updated_at, round_id, rpool_id)
as select
			created_at, timestamp, round, rpool
from tabroom.rpool_round;

create view treo.rpools
	(id, name, created_at, updated_at, tourn_id)
as select
	id, name, created_at, timestamp, tourn
from tabroom.rpool;

DROP TABLE IF EXISTS `school_contact`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school_contact` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `created_at` datetime NOT NULL,
  `updated_at` datetime NOT NULL,
  `school` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create view treo.squad_contacts
	(created_at, updated_at, person_id, squad_id)
as select
	created_at, timestamp, account, school
from tabroom.school_contact;

alter table tabroom.school add onsite_by int;

create view treo.squads
	(id, name, code, paid, onsite, onsite_at, onsite_by_id, registered_by_id, created_at, updated_at, school_id, tourn_id)
as select
	id, name, code, paid, registered, registered_on, onsite_by, registered_by, entered_on, timestamp, chapter, tourn
from tabroom.school;

alter table tabroom.ballot_value add speech smallint;
update tabroom.ballot_value, tabroom.ballot set tabroom.ballot_value.speech = tabroom.ballot.speechnumber 
	where tabroom.ballot.id = tabroom.ballot_value.tabroom.ballot and tabroom.ballot.speechnumber > 0;

create view treo.scores
	(id, tag, value, content, position, speech, created_at, updated_at, student_id, ballot_id)
as select
	id, tag, value, content, position, speech, created_at, timestamp, student, ballot
from tabroom.ballot_value;

create view treo.sections
	(id, letter, flight, bye, started, confirmed, bracket, created_at, updated_at, room_id, round_id)
as select
	id, letter, flight, bye, started, confirmed, bracket, created_at, timestamp, room, round
from tabroom.panel;

create view treo.sites
	(id, name, directions, created_at, updated_at, host_id, circuit_id)
as select
	id, name, directions, created_at, timestamp, host, circuit
from tabroom.site;

create view treo.school_circuits
	(id, created_at, updated_at, region_id, school_id, circuit_id, circuit_membership_id)
as select
	id, created_at, timestamp, region, chapter, circuit, membership
from tabroom.chapter_circuit;

create view treo.schools
	(id, name, city, state, country, coaches, level, naudl, ipeds, nces, nsda, created_at, updated_at)
as select
	id, name, city, state, country, coaches, level, naudl, ipeds, nces, nsda, created_at, timestamp
from tabroom.chapter;

alter table tabroom.chapter_judge add ada bool;
alter table tabroom.chapter_judge add email varchar(128);

create view treo.school_judges
	(id, first, last, gender, retired, cell, email, diet, ada, notes, notes_timestamp, school_id, person_id, request_person_id)
as select
	id, first, last, gender, retired, cell, email, diet, ada, notes, notes_timestamp, chapter, account, acct_request
from tabroom.chapter_judge;

create view treo.stats
	(id, tag, value, created_at, updated_at, event_id)
as select
	id, tag, value, created_at, timestamp, event
from tabroom.stats;

create view treo.strike_timeslots
	(id, name, start, end, fine, created_at, updated_at, class_id)
as select
	id, name, start, end, fine, created_at, timestamp, judge_group
from tabroom.strike_time;

alter table tabroom.strike add hidden bool;

create view treo.strikes
	(id, type, start, end, hidden, created_at, updated_at, region_id, timeslot_id, squad_id, entry_id, judge_id, entered_by_id, strike_timeslot_id)
as select
	id, type, start, end, hidden, created_at, timestamp, region, timeslot, school, entry, judge, entered_by, strike_timeslot
from tabroom.strike;

create view treo.students
	(id, first, last, grad_year, novice, retired, gender, phonetic, ualt_id, created_at, updated_at, school_id, person_id)
as select
	id, first, last, grad_year, novice, retired, gender, phonetic, ualt_id, created_at, timestamp, chapter, person
from tabroom.student;

create view treo.sweep_events
	(created_at, updated_at, sweep_set_id, event_id)
as select
	created_at, timestamp, sweep_set, event
from tabroom.sweep_event;

create view treo.sweep_include
	(id, created_at, updated_at, child_id, parent_id)
as select
	id, created_at, timestamp, child, parent
from tabroom.sweep_include;

create view treo.sweep_rules
	(id, tag, value, place, created_at, updated_at)
as select
	id, tag, value, place, created_at, timestamp
from tabroom.sweep_rule;

create view treo.sweep_sets
	(id, name, created_at, updated_at)
as select
	id, name, created_at, timestamp
from tabroom.sweep_set;

create view treo.tiebreak_sets
	(id, name, created_at, updated_at, tourn_id)
as select
	id, name, created_at, timestamp, tourn
from tabroom.tiebreak_set;

create view treo.tiebreaks
	(id, name, count, multiplier, priority, highlow, highlow_count, created_at, updated_at, tiebreak_set_id)
as select
	id, name, count, multiplier, priority, highlow, highlow_count, created_at, timestamp, tb_set
from tabroom.tiebreak;

create view treo.timeslots
	(id, name, start, end, created_at, updated_at, tourn_id)
as select
	id, name, start, end, created_at, timestamp, tourn
from tabroom.timeslot;

create view treo.tourn_circuits
	(created_at, updated_at, tourn_id, circuit_id)
as select
			created_at, timestamp, tourn, circuit
from tabroom.tourn_circuit;

create view treo.tourn_fees
	(id, amount, reason, start, end, created_at, updated_at, tourn_id)
as select
	id, amount, reason, start, end, created_at, timestamp, tourn
from tabroom.tourn_fee;

create view treo.tourn_ignore
	(created_at, updated_at, person_id, tourn_id)
as select
			created_at, timestamp, account, tourn
from tabroom.tourn_ignore;

create view treo.tourn_sites
	(created_at, updated_at, tourn_id, site_id)
as select
			created_at, timestamp, tourn, site
from tabroom.tourn_site;

alter table tabroom.tourn add city varchar(128);

create view treo.tourns
	(id, name, start, end, reg_start, reg_end, hidden, webname, tz, city, state, country, created_at, updated_at)
as select
	id, name, start, end, reg_start, reg_end, hidden, webname, tz, city, state, country, created_at, timestamp
from tabroom.tourn;

alter table tabroom.webpage add creator int;

create view treo.webpages
	(id, title, content, active, sitewide, page_order, created_at, updated_at, creator_id, editor_id, tourn_id, circuit_id)
as select
	id, title, content, active, sitewide, page_order, created_at, timestamp, creator, last_editor, tourn, circuit
from tabroom.webpage;

create view treo.circuit_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, circuit_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, circuit 
from tabroom.circuit_setting ;

create view treo.tourn_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, tourn_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, tourn
from tabroom.tourn_setting ;

create view treo.class_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, class_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, judge_group
from tabroom.judge_group_setting ;

create view treo.event_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, event_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, event
from tabroom.event_setting ;

create view treo.entry_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, entry_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, entry
from tabroom.entry_setting;

create view treo.jpool_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, jpool_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, jpool
from tabroom.jpool_setting ;

create view treo.rpool_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, rpool_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, rpool
from tabroom.rpool_setting ;

create view treo.tiebreak_set_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, tiebreak_set_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, tiebreak_set
from tabroom.tiebreak_setting ;

create view treo.judge_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, judge_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, judge
from tabroom.judge_setting ;

create view treo.round_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, round_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, round
from tabroom.round_setting ;

create view treo.person_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, person_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, account
from tabroom.account_setting ;

create view treo.squad_settings 
	(id, tag, value, value_text, value_date, created_at, updated_at, squad_id )
as select 
	id, tag, value, value_text, value_date, created_at, timestamp, school 
from tabroom.school_setting ;

#PERMISSIONS

create view treo.permission
	(created_at, updated_at, person_id, circuit_id, school_id, tourn_id
	as select 
			created_at, timestamp, person, circuit
from tabroom.circuit_admin;


