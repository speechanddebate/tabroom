
drop database if exists tabapi;
create database tabapi;
use tabapi;

create view tabapi.ballot
	(id, side, bye, forfeit, chair, speakerorder, speechnumber, seed, pullup, tv, audit, collected, judge_started,
		entry_id, judge_id, panel_id, collected_by_id, entered_by_id, audited_by_id, hangout_admin_id, timestamp)
as select
	id, side, bye, forfeit, chair, speakerorder, speechnumber, seed, pullup, tv, audit, collected, judge_started,
		entry, judge, panel, collected_by, entered_by, audited_by, hangout_admin, timestamp
from tabroom.ballot;

create view tabapi.change_log
	(id, type, description, 
		person_id, tourn_id, event_id, school_id, entry_id, judge_id, 
		old_panel_id, new_panel_id, fine_id, timestamp)
as select
	id, type, description, 
		person, tourn, event, school, entry, judge, 
		old_panel, new_panel, fine, timestamp
from tabroom.change_log;

create view tabapi.circuit_membership
	(id, name, circuit_id, timestamp)
as select
	id, name, circuit, timestamp
from tabroom.circuit_membership;

create view tabapi.circuit
	(id, name, abbr, active, state, country, tz, webname, timestamp)
as select
	id, name, abbr, active, state, country, tz, webname, timestamp
from tabroom.circuit;

create view tabapi.category
	(id, name, abbr, tourn_id, timestamp)
as select
	id, name, abbr, tourn, timestamp
from tabroom.category;

create view tabapi.chapter
	(id, name, address, city, state, zip, postal, country, coaches, self_prefs, level, nsda, naudl, ipeds, nces, ceeb, timestamp)
as select
	id, name, address, city, state, zip, postal, country, coaches, self_prefs, level, nsda, naudl, ipeds, nces, ceeb, timestamp
from tabroom.chapter;

create view tabapi.chapter_circuit
	(id, code, full_member, circuit_id, chapter_id, region_id, circuit_membership_id, timestamp)
as select
	id, code, full_member, circuit, chapter, region, circuit_membership, timestamp
from tabroom.chapter_circuit;

create view tabapi.chapter_judge
	(id, first, middle, last, ada, retired, phone, email, diet, notes, notes_timestamp, gender, chapter_id, person_id, person_request_id, timestamp)
as select
	id, first, middle, last, ada, retired, phone, email, diet, notes, notes_timestamp, gender, chapter, person, person_request, timestamp
from tabroom.chapter_judge;

create view tabapi.concession_purchase
	(id, quantity, placed, fulfilled, concession_id, school_id, timestamp)
as select
	id, quantity, placed, fulfilled, concession, school, timestamp
from tabroom.concession_purchase;

create view tabapi.concession
	(id, name, price, description, cap, school_cap, deadline, tourn_id, timestamp)
as select
	id, name, price, description, cap, school_cap, deadline, tourn, timestamp
from tabroom.concession;

create view tabapi.conflict
	(id, type, person_id, conflicted_id, chapter_id, added_by_id, timestamp) 
as select
	id, type, person, conflicted, chapter, added_by, timestamp
from tabroom.conflict;

create view tabapi.pattern
	(id, name, type, max, exclude_id, timestamp)
as select
	id, name, type, max, exclude, timestamp
from tabroom.pattern;

create view tabapi.email
	(id, subject, content, sent_to, sent_at, sender_id, tourn_id, circuit_id, timestamp)
as select
	id, subject, content, sent_to, sent_at, sender, tourn, circuit, timestamp
from tabroom.email;

create view tabapi.entry
	(id, code, name, active, ada, tba, seed, 
		dropped, waitlisted, dq, unconfirmed, 
		event_id, school_id, tourn_id, registered_by_id, timestamp)
as select
	id, code, name, active, ada, tba, seed, 
		dropped, waitlist, dq, unconfirmed, 
		event, school, tourn, registered_by, timestamp
from tabroom.entry;

create view tabapi.entry_student
	(entry_id, student_id, timestamp)
as select
	entry, student, timestamp
from tabroom.entry_student;

create view tabapi.event
	(id, name, type, abbr, fee, tourn_id, category_id, pattern_id, rating_subset_id, timestamp)
as select
	id, name, type, abbr, fee, tourn, category, pattern, rating_subset, timestamp
from tabroom.event;

create view tabapi.file
	(id, label, filename, published, uploaded_at, circuit_id, tourn_id, event_id, webpage_id, school_id, result_set_id, timestamp)
as select
	id, label, filename, published, uploaded, circuit, tourn, event, webpage, school, result, timestamp
from tabroom.file;

create view tabapi.fine
	(id, reason, amount, payment, deleted, deleted_at, deleted_by_id, levied_at, levied_by_id, 
		tourn_id, school_id, region_id, judge_id, timestamp)
as select
	id, reason, amount, payment, deleted, deleted_at, deleted_by, levied_at, levied_by, tourn, school, region, judge, timestamp
from tabroom.fine;

create view tabapi.follower
	(id, type, cell, domain, email, follower_id, tourn_id, judge_id, entry_id, school_id, person_id, timestamp)
as select
	id, type, cell, domain, email, follower, tourn, judge, entry, school, person, timestamp
from tabroom.follower;

create view tabapi.hotel
	(id, name, fee_multiplier, tourn_id, timestamp)
as select
	id, name, multiple, tourn, timestamp
from tabroom.hotel;

create view tabapi.housing
	(id, type, night, tba, waitlist, requested, requestor_id, tourn_id, student_id, judge_id, school_id, timestamp) 
as select
	id, type, night, tba, waitlist, requested, requestor, tourn, student, judge, school, timestamp
from tabroom.housing;

create view tabapi.housing_slots
	(id, night, slots, tourn_id, timestamp)
as select
	id, night, slots, tourn, timestamp
from tabroom.housing_slots;

create view tabapi.jpool_judge
	(jpool_id, judge_id, timestamp)
as select
	jpool, judge, timestamp
from tabroom.jpool_judge;

create view tabapi.jpool_round
			(round_id, jpool_id, timestamp)
as select
			round, jpool, timestamp
from tabroom.jpool_round;

create view tabapi.jpool
	(id, name, category_id, site_id, timestamp)
as select
	id, name, category, site, timestamp
from tabroom.jpool;

create view tabapi.judge_hire
	(id, entries_requested, entries_accepted, rounds_requested, rounds_accepted, requested_at, 
		tourn_id, school_id, judge_id, category_id, timestamp)
as select
	id, entries_requested, entries_accepted, rounds_requested, rounds_accepted, requested_at, tourn, school, judge, category, timestamp
from tabroom.judge_hire;

create view tabapi.judge
	(id, code, first, middle, last, active, ada, obligation, hired, 
		school_id, category_id, alt_category_id, covers_id, chapter_judge_id, person_id, person_request_id, timestamp)
as select
	id, code, first, middle, last, active, ada, obligation, hired, 
		school, category, alt_category, covers, chapter_judge, person, person_request, timestamp
from tabroom.judge;

create view tabapi.login
	(id, username, accesses, last_access, password, sha512, spinhash, pass_timestamp, pass_changekey, pass_change_expires, source, 
	person_id, nsda_login_id, ualt_id, timestamp)
as select
	id, username, accesses, last_access, password, sha512, spinhash, pass_timestamp, pass_changekey, pass_change_expires, source,
	person, nsda_login_id, ualt_id, timestamp
from tabroom.login;

create view tabapi.person
	(id, email, first, middle, last, gender, pronoun, no_email, street, city, state, zip, postal, country, tz, phone, provider, site_admin, diversity, googleplus, ualt_id, timestamp)
as select
	id, email, first, middle, last, gender, pronoun, no_email, street, city, state, zip, postal, country, tz, phone, provider, site_admin, diversity, googleplus, ualt_id, timestamp
from tabroom.person;

create view tabapi.qualifier
	(id, name, result, entry_id, tourn_id, qualified_tourn_id, timestamp)
as select
	id, name, result, entry, tourn, qualifier_tourn, timestamp
from tabroom.qualifier;

create view tabapi.rating_subset
	(id, name, category_id, timestamp)
as select
	id, name, category, timestamp
from tabroom.rating_subset;

create view tabapi.rating_tier
	(id, type, name, description, strike, conflict, min, max, default_tier, rating_subset_id, category_id, timestamp)
as select
	id, type, name, description, strike, conflict, min, max, start, rating_subset, category, timestamp
from tabroom.rating_tier;

create view tabapi.rating
	(id, type, draft, entered, ordinal, percentile, tourn_id, school_id, entry_id, rating_tier_id, judge_id, rating_subset_id, sheet_id, timestamp)
as select
	id, type, draft, entered, ordinal, percentile, tourn, school, entry, rating_tier, judge, rating_subset, sheet, timestamp
from tabroom.rating;

create view tabapi.region
	(id, name, code, quota, archdiocese, cooke_points, sweeps, circuit_id, tourn_id, timestamp)
as select
	id, name, code, quota, arch, cooke_pts, sweeps, circuit, tourn, timestamp
from tabroom.region;

create view tabapi.diocese
	(id, name, code, state, quota, active, archdiocese, cooke_award_points, timestamp)
as select
	id, name, code, state, quota, active, archdiocese, cooke_award_points, timestamp
from tabroom.diocese;

create view tabapi.district
	(id, name, code, location, chair, timestamp)
as select
	id, name, code, location, chair, timestamp
from tabroom.district;

create view tabapi.region_fine
	(id, reason, amount, levied_at, levied_by_id, region_id, tourn_id, timestamp)
as select
	id, reason, amount, levied_on, levied_by, region, tourn, timestamp
from tabroom.region_fine;

create view tabapi.result_set
	(id, label, bracket, published, generated, tourn_id, event_id, timestamp)
as select
	id, label, bracket, published, generated, tourn, event, timestamp
from tabroom.result_set;

create view tabapi.result_value
	(id, tag, value, priority, long_tag, no_sort, sort_descending, description, result_id, timestamp)
as select
	id, tag, value, priority, long_tag, no_sort, sort_desc, long_tag, result, timestamp
from tabroom.result_value;

create view tabapi.result
	(id, rank, percentile, honor, honor_site, result_set_id, entry_id, student_id, school_id, round_id, timestamp)
as select
	id, rank, percentile, honor, honor_site, result_set, entry, student, school, round, timestamp
from tabroom.result;

create view tabapi.room_strike
	(id, type, start, end, room_id, event_id, tourn_id, entry_id, judge_id, timestamp)
as select
	id, type, start, end, room, event, tourn, entry, judge, timestamp
from tabroom.room_strike;

create view tabapi.room
	(id, building, name, quality, capacity, inactive, ada, notes, site_id, timestamp)
as select
	id, building, name, quality, capacity, inactive, ada, notes, site, timestamp
from tabroom.room;

create view tabapi.round
	(id, type, number, label, flighted, published, post_results, start_time, event_id, timeslot_id, site_id, tiebreak_set_id, timestamp)
as select
	id, type, name, label, flighted, published, post_results, start_time, event, timeslot, site, tiebreak_set, timestamp
from tabroom.round;

create view tabapi.rpool_room
	(rpool_id, room_id, timestamp)
as select
	rpool, room, timestamp
from tabroom.rpool_room;

create view tabapi.rpool_round
	(round_id, rpool_id, timestamp)
as select
	round, rpool, timestamp
from tabroom.rpool_round;

create view tabapi.rpool
	(id, name, tourn_id, timestamp)
as select
	id, name, tourn, timestamp
from tabroom.rpool;

create view tabapi.school
	(id, name, code, onsite, tourn_id, chapter_id, region_id, timestamp)
as select
	id, name, code, onsite, tourn, chapter, region, timestamp
from tabroom.school;

create view tabapi.score
	(id, tag, value, content, speech, position, ballot_id, student_id, timestamp)
as select
	id, tag, value, content, speech, position, ballot, student, timestamp
from tabroom.score;

create view tabapi.panel
	(id, letter, flight, bye, started, confirmed, bracket, room_id, round_id, timestamp)
as select
	id, letter, flight, bye, started, confirmed, bracket, room, round, timestamp
from tabroom.panel;

create view tabapi.site
	(id, name, directions, dropoff, host_id, circuit_id, timestamp)
as select
	id, name, directions, dropoff, host, circuit, timestamp
from tabroom.site;

create view tabapi.stats
	(id, type, tag, value, event_id, timestamp)
as select
	id, type, tag, value, event, timestamp
from tabroom.stats;

create view tabapi.strike_timeslot
	(id, name, fine, start, end, category_id, timestamp)
as select
	id, fine, name, start, end, category, timestamp
from tabroom.strike_timeslot;

create view tabapi.strike
	(id, type, start, end, registrant, conflictee, tourn_id, judge_id, event_id, entry_id, school_id, region_id, strike_timeslot_id, timestamp)
as select
	id, type, start, end, registrant, conflictee, tourn, judge, event, entry, school, region, strike_timeslot, timestamp
from tabroom.strike;

create view tabapi.student
	(id, first, middle, last, phonetic, grad_year, novice, retired, gender, diet, birthdate, school_sid, race, 
		ualt_id, chapter_id, person_id, person_request_id, timestamp)
as select
	id, first, middle, last, phonetic, grad_year, novice, retired, gender, diet, birthdate, school_sid, race, 
		ualt_id, chapter, person, person_request, timestamp
from tabroom.student;

create view tabapi.sweep_event
	(sweep_set_id, event_id, timestamp)
as select
	sweep_set, event, timestamp
from tabroom.sweep_event;

create view tabapi.sweep_include
	(id, child_id, parent_id, timestamp)
as select
	id, child, parent, timestamp
from tabroom.sweep_include;

create view tabapi.sweep_rule
	(id, tag, value, place, count, sweep_set_id, timestamp)
as select
	id, tag, value, place, count, sweep_set, timestamp
from tabroom.sweep_rule;

create view tabapi.sweep_set
	(id, name, tourn_id, timestamp)
as select
	id, name, tourn, timestamp
from tabroom.sweep_set;

create view tabapi.tiebreak_set
	(id, name, tourn_id, timestamp)
as select
	id, name, tourn, timestamp
from tabroom.tiebreak_set;

create view tabapi.tiebreak
	(id, name, count, multiplier, priority, highlow, highlow_count, tiebreak_set_id, timestamp)
as select
	id, name, count, multiplier, priority, highlow, highlow_count, tiebreak_set, timestamp
from tabroom.tiebreak;

create view tabapi.timeslot
	(id, name, start, end, tourn_id, timestamp)
as select
	id, name, start, end, tourn, timestamp
from tabroom.timeslot;

create view tabapi.tourn_circuit
	(id, approved, tourn_id, circuit_id, timestamp)
as select
	id, approved, tourn, circuit, timestamp
from tabroom.tourn_circuit;

create view tabapi.tourn_fee
	(id, amount, reason, start, end, tourn_id, timestamp)
as select
	id, amount, reason, start, end, tourn, timestamp
from tabroom.tourn_fee;

create view tabapi.tourn_ignore
	(person_id, tourn_id, timestamp)
as select
	person, tourn, timestamp
from tabroom.tourn_ignore;

create view tabapi.tourn_site
	(tourn_id, site_id, timestamp)
as select
	tourn, site, timestamp
from tabroom.tourn_site;

create view tabapi.tourn
	(id, name, city, state, country, tz, webname, hidden, start, end, reg_start, reg_end, timestamp)
as select
	id, name, start, end, reg_start, reg_end, hidden, webname, tz, city, state, country, timestamp
from tabroom.tourn;

create view tabapi.webpage
	(id, title, content, published, sitewide, special, page_order, tourn_id, last_editor_id, parent_id, timestamp)
as select
	id, title, content, published, sitewide, special, page_order, tourn, last_editor, parent, timestamp
from tabroom.webpage;

create view tabapi.setting
	(id, type, subtype, tag, value_type, conditions, timestamp)
as select 
	id, type, subtype, tag, value_type, conditions, timestamp
from tabroom.setting;

create view tabapi.setting_label 
	(id, lang, label, guide, options, setting, timestamp)
as select 
	id, lang, label, guide, options, setting, timestamp
from tabroom.setting_label;

create view tabapi.circuit_setting
	(id, tag, value, value_text, value_date, circuit_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, circuit, setting, timestamp
from tabroom.circuit_setting;

create view tabapi.tourn_setting 
	(id, tag, value, value_text, value_date, tourn_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, tourn, setting, timestamp
from tabroom.tourn_setting;

create view tabapi.category_setting 
	(id, tag, value, value_text, value_date, category_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, category, setting, timestamp
from tabroom.category_setting;

create view tabapi.event_setting 
	(id, tag, value, value_text, value_date, event_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, event, setting, timestamp
from tabroom.event_setting;

create view tabapi.entry_setting 
	(id, tag, value, value_text, value_date, entry_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, entry, setting, timestamp
from tabroom.entry_setting;

create view tabapi.jpool_setting 
	(id, tag, value, value_text, value_date, jpool_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, jpool, setting, timestamp
from tabroom.jpool_setting;

create view tabapi.rpool_setting 
	(id, tag, value, value_text, value_date, rpool_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, rpool, setting, timestamp
from tabroom.rpool_setting;

create view tabapi.tiebreak_set_setting 
	(id, tag, value, value_text, value_date, tiebreak_set_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, tiebreak_set, setting, timestamp
from tabroom.tiebreak_set_setting;

create view tabapi.judge_setting 
	(id, tag, value, value_text, value_date, judge_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, judge, setting, timestamp
from tabroom.judge_setting;

create view tabapi.round_setting 
	(id, tag, value, value_text, value_date, round_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, round, setting, timestamp
from tabroom.round_setting;

create view tabapi.person_setting 
	(id, tag, value, value_text, value_date, person_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, person, setting, timestamp
from tabroom.person_setting;

create view tabapi.school_setting 
	(id, tag, value, value_text, value_date, school_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, school, setting, timestamp
from tabroom.school_setting;

create view tabapi.region_setting 
	(id, tag, value, value_text, value_date, region_id, setting_id, timestamp)
as select 
	id, tag, value, value_text, value_date, region, setting, timestamp
from tabroom.region_setting;

#PERMISSIONS

create view tabapi.permission
	(id, person_id, tourn_id, region_id, chapter_id, circuit_id, category_id, tag, timestamp)
	as select 
	id, person, tourn, region, chapter, circuit, category, tag, timestamp
from tabroom.permission;

create view tabapi.session
	(id, userkey, ip, su_id, person_id, tourn_id, event_id, category_id, timestamp)
as select 
	id, userkey, ip, su, person, tourn, event, category, timestamp
from tabroom.session;



