
 alter table onetab.ballots  add created_at datetime;
 alter table onetab.calendars add created_at datetime;
 alter table onetab.change_logs add created_at datetime;
 alter table onetab.circuit_admins add created_at datetime;
 alter table onetab.circuit_dues add created_at datetime;
 alter table onetab.circuit_memberships add created_at datetime;
 alter table onetab.circuits add created_at datetime;
 alter table onetab.classes add created_at datetime;
 alter table onetab.concession_purchases add created_at datetime;
 alter table onetab.concessions add created_at datetime;
 alter table onetab.conflicts add created_at datetime;
 alter table onetab.double_entry_sets add created_at datetime;
 alter table onetab.emails add created_at datetime;
 alter table onetab.entries add created_at datetime;
 alter table onetab.entry_students add created_at datetime;
 alter table onetab.events add created_at datetime;
 alter table onetab.files add created_at datetime;
 alter table onetab.fines add created_at datetime;
 alter table onetab.followers add created_at datetime;
 alter table onetab.hotels add created_at datetime;
 alter table onetab.housing_requests add created_at datetime;
 alter table onetab.housing_slots add created_at datetime;
 alter table onetab.jpool_judges add created_at datetime;
 alter table onetab.jpool_rounds add created_at datetime;
 alter table onetab.jpools add created_at datetime;
 alter table onetab.judge_hires add created_at datetime;
 alter table onetab.judges add created_at datetime;
 alter table onetab.logins add created_at datetime;
 alter table onetab.people add created_at datetime;
 alter table onetab.pool_judges add created_at datetime;
 alter table onetab.pool_rooms add created_at datetime;
 alter table onetab.qualifiers add created_at datetime;
 alter table onetab.rating_sheets add created_at datetime;
 alter table onetab.rating_subsets add created_at datetime;
 alter table onetab.rating_tiers add created_at datetime;
 alter table onetab.ratings add created_at datetime;
 alter table onetab.region_admins add created_at datetime;
 alter table onetab.regions add created_at datetime;
 alter table onetab.result_sets add created_at datetime;
 alter table onetab.result_values add created_at datetime;
 alter table onetab.results add created_at datetime;
 alter table onetab.room_strikes add created_at datetime;
 alter table onetab.rooms add created_at datetime;
 alter table onetab.rounds add created_at datetime;
 alter table onetab.rpool_rooms add created_at datetime;
 alter table onetab.rpool_rounds add created_at datetime;
 alter table onetab.rpools add created_at datetime;
 alter table onetab.school_contacts add created_at datetime;
 alter table onetab.schools add created_at datetime;
 alter table onetab.scores add created_at datetime;
 alter table onetab.sections add created_at datetime;
 alter table onetab.settings add created_at datetime;
 alter table onetab.sites add created_at datetime;
 alter table onetab.squad_admins add created_at datetime;
 alter table onetab.squad_circuits add created_at datetime;
 alter table onetab.squad_judges add created_at datetime;
 alter table onetab.squads add created_at datetime;
 alter table onetab.stats add created_at datetime;
 alter table onetab.strike_timeslots add created_at datetime;
 alter table onetab.strikes add created_at datetime;
 alter table onetab.students add created_at datetime;
 alter table onetab.sweep_events add created_at datetime;
 alter table onetab.sweep_include add created_at datetime;
 alter table onetab.sweep_rules add created_at datetime;
 alter table onetab.sweep_sets add created_at datetime;
 alter table onetab.tiebreak_sets add created_at datetime;
 alter table onetab.tiebreaks add created_at datetime;
 alter table onetab.timeslots add created_at datetime;
 alter table onetab.tourn_admins add created_at datetime;
 alter table onetab.tourn_circuits add created_at datetime;
 alter table onetab.tourn_fees add created_at datetime;
 alter table onetab.tourn_ignore add created_at datetime;
 alter table onetab.tourn_sites add created_at datetime;
 alter table onetab.tourns add created_at datetime;
 alter table onetab.webpages add created_at datetime;


alter table tabroom.ballot add audited_by int;

 onetab.ballots 
	'id', 'side', 'bye', 'forfeit', 'chair', 'speaker_order', 'speech_number', 'collected_time', 'created_at', 'updated_at', 'entry_id', 'judge_id', 'section_id', 'collected_by_id', 'entered_by_id', 'audited_by_id'
	as select 'id', 'side', 'bye', 'forfeit', 'chair', 'speaker_order', 'speech_number', 'collected_time', 'timestamp', 'timestamp', 'entry', 'judge', 'panel', 'collected_by', 'account', 'audited_by'
	from tabroom.ballot;

# onetab.calendars
#	'id', 'name', 'start', 'end', 'reg_start', 'reg_end', 'webname', 'location', 'city', 'state', 'country', 'tz', 'contact', 'url', 'source', 'hidden', 'created_at', 'updated_at', 'tourn_id', 'person_id'
#	as select 'id', 'name', 'start', 'end', 'reg_start', 'reg_end', 'webname', 'location', 'city', 'state', 'country', 'tz', 'contact', 'url', 'source', 'hidden', 'created_at', 'updated_at', 'tourn_id', 'person_id'
	from tabroom. ;

alter table tabroom.tourn_change add webpage int;

 onetab.change_logs
	'id', 'type', 'description', 'created_at', 'updated_at', 'tourn_id', 'class_id', 'event_id', 'webpage_id', 'school_id', 'entry_id', 'judge_id', 'strike_id', 'round_id', 'section_id', 'new_section_id'
	as select 'id', 'type', 'description', 'timestamp', 'timestamp', 'tourn', 'class', 'event', 'webpage', 'school', 'entry', 'judge', 'strike', 'round', 'panel', 'new_panel'
	from tabroom.tourn_change;

 onetab.circuit_admins
	'created_at', 'updated_at', 'person_id', 'circuit_id'
	as select 'created_at', 'timestamp', 'person', 'circuit'
	from tabroom.circuit_admin;

 onetab.circuit_dues
	'id', 'amount', 'paid_on', 'created_at', 'updated_at', 'circuit_id', 'squad_id'
	as select 'id', 'amount', 'paid_on', 'created_at', 'timestamp', 'circuit', 'squad'
	from tabroom.circuit_dues;

alter table tabroom.circuit_memberships add region_required bool;

 onetab.circuit_memberships
	'id', 'name', 'approval_required', 'region_required', 'dues_required', 'dues_amount', 'dues_fiscal_year', 'description', 'created_at', 'updated_at', 'circuit_id'
	as select 'id', 'name', 'approval', 'region_required', 'dues', 'dues', 'dues_start', 'text', 'created_at', 'timestamp', 'circuit'
	from tabroom.circuit_membership;

 onetab.circuits
	'id', 'name', 'abbr', 'active', 'state', 'country', 'tz', 'website', 'webname', 'created_at', 'updated_at'
	as select 'id', 'name', 'abbr', 'active', 'state', 'country', 'tz', 'website', 'webname', 'created_at', 'timestamp'
	from tabroom.circuit;

 onetab.classes
	'id', 'name', 'abbr', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'name', 'abbr', 'created_at', 'timestamp', 'tourn'
	from tabroom.judge_group;

 onetab.concession_purchases
	'id', 'quantity', 'fulfilled', 'created_at', 'updated_at', 'concession_id', 'school_id'
	as select 'id', 'quantity', 'fulfilled', 'created_at', 'timestamp', 'concession', 'school'
	from tabroom.concession_purchase;

 onetab.concessions
	'id', 'name', 'price', 'description', 'cap', 'school_cap', 'deadline', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'name', 'price', 'description', 'cap', 'school_cap', 'deadline', 'created_at', 'timestamp', 'tourn'
	from tabroom.concession;

 onetab.conflicts
	'id', 'name', 'created_at', 'updated_at', 'person_id', 'target_person_id', 'creator_id', 'target_squad_id'
	as select 'id', 'name', 'created_at', 'timestamp', 'account', 'conflict', 'added_by', 'chapter'
	from tabroom.account_conflict;

 onetab.double_entry_sets
	'id', 'name', 'type', 'max', 'created_at', 'updated_at', 'mutex_id'
	as select 'id', 'name', 'setting', 'max', 'created_at', 'timestamp', 'exclude'
	from tabroom.event_double;

 onetab.emails
	'id', 'subject', 'content', 'recipients', 'created_at', 'updated_at', 'sender_id', 'tourn_id', 'circuit_id'
	as select 'id', 'subject', 'content', 'sent_to', 'created_at', 'timestamp', 'sender', 'tourn', 'circuit'
	from tabroom.email;

 onetab.entries
	'id', 'code', 'name', 'dropped', 'waitlisted', 'dq', 'register_seed', 'pairing_seed', 'ada', 'registered_by', 'tba', 'created_at', 'updated_at', 'event_id', 'school_id'
	as select 'id', 'code', 'name', 'dropped', 'waitlisted', 'dq', 'seed', 'pair_seed', 'ada', 'registered_by', 'tba', 'created_at', 'timestamp', 'event', 'school'
	from tabroom.entry;

 onetab.entry_students
	'created_at', 'updated_at', 'entry_id', 'student_id'
	as select 'created_at', 'timestamp', 'entry', 'student'
	from tabroom.entry_student;

 onetab.events
	'id', 'name', 'type', 'abbr', 'fee', 'created_at', 'updated_at', 'tourn_id', 'class_id', 'double_entry_set_id'
	as select 'id', 'name', 'type', 'abbr', 'fee', 'created_at', 'timestamp', 'tourn', 'judge_group', 'event_double'
	from tabroom.event;

alter table tabroom.file add public bool;

 onetab.files
	'id', 'label', 'filename', 'public', 'created_at', 'updated_at', 'circuit_id', 'tourn_id', 'event_id', 'webpage_id', 'school_id', 'result_set_id'
	as select 'id', 'label', 'name', 'public', 'uploaded', 'timestamp', 'circuit', 'tourn', 'event', 'webpage', 'school', 'result'
	from tabroom.file;

alter table school_fine add tourn int;
alter table school_fine add judge int;
alter table school_fine add region int;

onetab.fines
	'id', 'amount', 'reason', 'created_at', 'updated_at', 'region_id', 'tourn_id', 'school_id', 'judge_id', 'levied_by_id'
	as select 'id', 'amount', 'reason', 'levied_on', 'timestamp', 'region', 'tourn', 'school', 'judge', 'levied_by'
	from tabroom.fine;


 onetab.followers
	'id', 'cell', 'email', 'domain', 'created_at', 'updated_at', 'tourn_id', 'judge_id', 'entry_id', 'school_id', 'person_id'
	as select 'id', 'cell', 'email', 'domain', 'created_at', 'timestamp', 'tourn', 'judge_id', 'entry_id', 'school_id', 'person_id'
	from tabroom. ;


 onetab.hotels
	'id', 'name', 'fee_multiplier', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'name', 'fee_multiplier', 'created_at', 'timestamp', 'tourn'
	from tabroom. ;


 onetab.housing_requests
	'id', 'night', 'type', 'tba', 'waitlist', 'created_at', 'updated_at', 'student_id', 'judge_id', 'school_id', 'requestor_id'
	as select 'id', 'night', 'type', 'tba', 'waitlist', 'created_at', 'timestamp', 'student', 'judge_id', 'school_id', 'requestor_id'
	from tabroom. ;


 onetab.housing_slots
	'id', 'night', 'slots', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'night', 'slots', 'created_at', 'timestamp', 'tourn'
	from tabroom. ;


 onetab.jpool_judges
	'created_at', 'updated_at', 'jpool_id', 'judge_id'
	as select 'created_at', 'timestamp', 'jpool', 'judge_id'
	from tabroom. ;


 onetab.jpool_rounds
	'created_at', 'updated_at', 'round_id', 'jpool_id'
	as select 'created_at', 'timestamp', 'round', 'jpool_id'
	from tabroom. ;


 onetab.jpools
	'id', 'name', 'created_at', 'updated_at', 'class_id', 'site_id'
	as select 'id', 'name', 'created_at', 'timestamp', 'class', 'site_id'
	from tabroom. ;


 onetab.judge_hires
	'id', 'requested_at', 'entries_requested', 'entries_accepted', 'rounds_requested', 'rounds_accepted', 'created_at', 'updated_at', 'school_id', 'tourn_id', 'judge_id', 'requestor_id'
	as select 'id', 'requested_at', 'entries_requested', 'entries_accepted', 'rounds_requested', 'rounds_accepted', 'created_at', 'timestamp', 'school', 'tourn_id', 'judge_id', 'requestor_id'
	from tabroom. ;


 onetab.judges
	'id', 'first', 'last', 'code', 'active', 'ada', 'tab_rating', 'obligation_rounds', 'hired_rounds', 'created_at', 'updated_at', 'squad_judge_id', 'class_id', 'school_id', 'person_id', 'alternate_class_id'
	as select 'id', 'first', 'last', 'code', 'active', 'ada', 'tab_rating', 'obligation_rounds', 'hired_rounds', 'created_at', 'timestamp', 'squad_judge', 'class_id', 'school_id', 'person_id', 'alternate_class_id'
	from tabroom. ;


 onetab.logins
	'id', 'username', 'password', 'sha512', 'name', 'accesses', 'last_access', 'pass_changekey', 'pass_timestamp', 'pass_change_expires', 'source', 'ualt_id', 'created_at', 'updated_at', 'person_id'
	as select 'id', 'username', 'password', 'sha512', 'name', 'accesses', 'last_access', 'pass_changekey', 'pass_timestamp', 'pass_change_expires', 'source', 'ualt', 'created_at', 'timestamp', 'person_id'
	from tabroom. ;


 onetab.people
	'id', 'email', 'first', 'middle', 'last', 'phone', 'alt_phone', 'provider', 'street', 'city', 'state', 'zip', 'country', 'gender', 'ualt_id', 'site_admin', 'no_email', 'multiple', 'tz', 'diversity', 'flags', 'created_at', 'updated_at'
	as select 'id', 'email', 'first', 'middle', 'last', 'phone', 'alt_phone', 'provider', 'street', 'city', 'state', 'zip', 'country', 'gender', 'ualt', 'site_admin', 'no_email', 'multiple', 'tz', 'diversity', 'flags', 'created_at', 'timestamp'
	from tabroom. ;


 onetab.pool_judges
	'created_at', 'updated_at', 'judge_id', 'jpool_id'
	as select 'created_at', 'timestamp', 'judge', 'jpool_id'
	from tabroom. ;


 onetab.pool_rooms
	'created_at', 'updated_at', 'room_id', 'rpool_id'
	as select 'created_at', 'timestamp', 'room', 'rpool_id'
	from tabroom. ;


 onetab.qualifiers
	'id', 'tag', 'value', 'created_at', 'updated_at', 'event_id', 'entry_id', 'tourn_id', 'qualified_at_id'
	as select 'id', 'tag', 'value', 'created_at', 'timestamp', 'event', 'entry_id', 'tourn_id', 'qualified_at_id'
	from tabroom. ;


 onetab.rating_sheets
	'id', 'name', 'description', 'created_at', 'updated_at'
	as select 'id', 'name', 'description', 'created_at', 'timestamp'
	from tabroom. ;


 onetab.rating_subsets
	'id', 'name', 'created_at', 'updated_at', 'class_id'
	as select 'id', 'name', 'created_at', 'timestamp', 'class'
	from tabroom. ;


 onetab.rating_tiers
	'id', 'name', 'type', 'description', 'strike', 'conflict', 'min', 'max', 'default_tier', 'created_at', 'updated_at', 'rating_subset_id', 'class_id', 'rating_sheet_id'
	as select 'id', 'name', 'type', 'description', 'strike', 'conflict', 'min', 'max', 'default_tier', 'created_at', 'timestamp', 'rating_subset', 'class_id', 'rating_sheet_id'
	from tabroom. ;


 onetab.ratings
	'id', 'ordinal', 'percentile', 'type', 'created_at', 'updated_at', 'school_id', 'entry_id', 'judge_id', 'rating_tier_id', 'rating_subset_id', 'rating_sheet_id'
	as select 'id', 'ordinal', 'percentile', 'type', 'created_at', 'timestamp', 'school', 'entry_id', 'judge_id', 'rating_tier_id', 'rating_subset_id', 'rating_sheet_id'
	from tabroom. ;


 onetab.region_admins
	'created_at', 'updated_at', 'person_id', 'region_id'
	as select 'created_at', 'timestamp', 'person', 'region_id'
	from tabroom. ;


 onetab.regions
	'id', 'name', 'code', 'active', 'ncfl_quota', 'ncfl_archdiocese', 'ncfl_cooke', 'ncfl_sweeps', 'created_at', 'updated_at', 'circuit_id', 'tourn_id'
	as select 'id', 'name', 'code', 'active', 'ncfl_quota', 'ncfl_archdiocese', 'ncfl_cooke', 'ncfl_sweeps', 'created_at', 'timestamp', 'circuit', 'tourn_id'
	from tabroom. ;


 onetab.result_sets
	'id', 'label', 'bracket', 'published', 'created_at', 'updated_at', 'tourn_id', 'event_id', 'person_id'
	as select 'id', 'label', 'bracket', 'published', 'created_at', 'timestamp', 'tourn', 'event_id', 'person_id'
	from tabroom. ;


 onetab.result_values
	'id', 'tag', 'value', 'priority', 'sort', 'description', 'created_at', 'updated_at', 'result_id'
	as select 'id', 'tag', 'value', 'priority', 'sort', 'description', 'created_at', 'timestamp', 'result'
	from tabroom. ;


 onetab.results
	'id', 'honor', 'honor_site', 'rank', 'percentile', 'created_at', 'updated_at', 'student_id', 'entry_id', 'school_id', 'result_set_id'
	as select 'id', 'honor', 'honor_site', 'rank', 'percentile', 'created_at', 'timestamp', 'student', 'entry', 'school', 'result_set'
	from tabroom. ;


 onetab.room_strikes
	'id', 'type', 'start', 'end', 'created_at', 'updated_at', 'room_id'
	as select 'id', 'type', 'start', 'end', 'created_at', 'timestamp', 'room'
	from tabroom. ;


 onetab.rooms
	'id', 'name', 'building', 'quality', 'capacity', 'active', 'ada', 'created_at', 'updated_at', 'site_id'
	as select 'id', 'name', 'building', 'quality', 'capacity', 'active', 'ada', 'created_at', 'timestamp', 'site'
	from tabroom. ;


 onetab.rounds
	'id', 'number', 'name', 'type', 'published', 'post_results', 'flighted', 'start_time', 'created_at', 'updated_at', 'site_id', 'timeslot_id', 'event_id', 'tiebreak_set_id'
	as select 'id', 'number', 'name', 'type', 'published', 'post_results', 'flighted', 'start_time', 'created_at', 'timestamp', 'site', 'timeslot', 'event', 'tiebreak_set'
	from tabroom. ;


 onetab.rpool_rooms
	'created_at', 'updated_at', 'rpool_id', 'room_id'
	as select 'created_at', 'timestamp', 'rpool', 'room'
	from tabroom. ;


 onetab.rpool_rounds
	'created_at', 'updated_at', 'round_id', 'rpool_id'
	as select 'created_at', 'timestamp', 'round', 'rpool'
	from tabroom. ;


 onetab.rpools
	'id', 'name', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'name', 'created_at', 'timestamp', 'tourn'
	from tabroom. ;


 onetab.school_contacts
	'created_at', 'updated_at', 'person_id', 'school_id'
	as select 'created_at', 'timestamp', 'person', 'school'
	from tabroom. ;


 onetab.schools
	'id', 'name', 'code', 'paid', 'registered', 'contact_person_id', 'contact_number', 'contact_name', 'contact_email', 'created_at', 'updated_at', 'squad_id', 'tourn_id'
	as select 'id', 'name', 'code', 'paid', 'registered', 'contact_person', 'contact_number', 'contact_name', 'contact_email', 'created_at', 'timestamp', 'squad', 'tourn'
	from tabroom. ;


 onetab.scores
	'id', 'tag', 'value', 'content', 'position', 'speech', 'created_at', 'updated_at', 'student_id', 'ballot_id'
	as select 'id', 'tag', 'value', 'content', 'position', 'speech', 'created_at', 'timestamp', 'student', 'ballot'
	from tabroom. ;


 onetab.sections
	'id', 'letter', 'flight', 'bye', 'started', 'bracket', 'created_at', 'updated_at', 'room_id', 'round_id'
	as select 'id', 'letter', 'flight', 'bye', 'started', 'bracket', 'created_at', 'timestamp', 'room', 'round'
	from tabroom. ;


 onetab.settings
	'id', 'type', 'tag', 'value', 'value_text', 'value_date', 'created_at', 'updated_at', 'circuit_id', 'tourn_id', 'class_id', 'event_id', 'jpool_id', 'tiebreak_set_id', 'judge_id', 'round_id', 'person_id', 'school_id'
	as select 'id', 'type', 'tag', 'value', 'value_text', 'value_date', 'created_at', 'timestamp', 'circuit', 'tourn', 'class', 'event', 'jpool', 'tiebreak_set', 'judge', 'round', 'person', 'school'
	from tabroom. ;


 onetab.sites
	'id', 'name', 'directions', 'created_at', 'updated_at', 'host_id', 'circuit_id'
	as select 'id', 'name', 'directions', 'created_at', 'timestamp', 'host', 'circuit'
	from tabroom. ;


 onetab.squad_admins
	'created_at', 'updated_at', 'person_id', 'squad_id'
	as select 'created_at', 'timestamp', 'person', 'squad'
	from tabroom. ;


 onetab.squad_circuits
	'id', 'name', 'created_at', 'updated_at', 'region_id', 'squad_id', 'circuit_id', 'circuit_membership_id'
	as select 'id', 'name', 'created_at', 'timestamp', 'region', 'squad', 'circuit', 'circuit_membership'
	from tabroom. ;


 onetab.squad_judges
	'id', 'first', 'last', 'gender', 'retired', 'cell', 'ada', 'diet', 'notes', 'notes_timestamp', 'created_at', 'updated_at', 'squad_id', 'person_id', 'person_request_id'
	as select 'id', 'first', 'last', 'gender', 'retired', 'cell', 'ada', 'diet', 'notes', 'notes_timestamp', 'created_at', 'timestamp', 'squad', 'person', 'person_request'
	from tabroom. ;


 onetab.squads
	'id', 'name', 'city', 'state', 'country', 'coaches', 'district_id', 'level', 'naudl', 'ipeds', 'nces', 'nsda', 'created_at', 'updated_at'
	as select 'id', 'name', 'city', 'state', 'country', 'coaches', 'district', 'level', 'naudl', 'ipeds', 'nces', 'nsda', 'created_at', 'timestamp'
	from tabroom. ;


 onetab.stats
	'id', 'type', 'tag', 'value', 'created_at', 'updated_at', 'event_id'
	as select 'id', 'type', 'tag', 'value', 'created_at', 'timestamp', 'event'
	from tabroom. ;


 onetab.strike_timeslots
	'id', 'name', 'start', 'end', 'fine', 'created_at', 'updated_at', 'class_id'
	as select 'id', 'name', 'start', 'end', 'fine', 'created_at', 'timestamp', 'class'
	from tabroom. ;


 onetab.strikes
	'id', 'type', 'start', 'end', 'hidden', 'created_at', 'updated_at', 'region_id', 'timeslot_id', 'school_id', 'entry_id', 'judge_id', 'entered_by_id', 'strike_timeslot_id'
	as select 'id', 'type', 'start', 'end', 'hidden', 'created_at', 'timestamp', 'region', 'timeslot', 'school', 'entry', 'judge', 'entered_by', 'strike_timeslot'
	from tabroom. ;


 onetab.students
	'id', 'first', 'last', 'grad_year', 'novice', 'retired', 'gender', 'phonetic', 'ualt_id', 'created_at', 'updated_at', 'squad_id', 'person_id'
	as select 'id', 'first', 'last', 'grad_year', 'novice', 'retired', 'gender', 'phonetic', 'ualt', 'created_at', 'timestamp', 'squad', 'person'
	from tabroom. ;


 onetab.sweep_events
	'created_at', 'updated_at', 'sweep_set_id', 'event_id'
	as select 'created_at', 'timestamp', 'sweep_set', 'event'
	from tabroom. ;


 onetab.sweep_include
	'id', 'created_at', 'updated_at', 'child_id', 'parent_id'
	as select 'id', 'created_at', 'timestamp', 'child', 'parent'
	from tabroom. ;


 onetab.sweep_rules
	'id', 'tag', 'value', 'place', 'created_at', 'updated_at'
	as select 'id', 'tag', 'value', 'place', 'created_at', 'timestamp'
	from tabroom. ;


 onetab.sweep_sets
	'id', 'name', 'created_at', 'updated_at'
	as select 'id', 'name', 'created_at', 'timestamp'
	from tabroom. ;


 onetab.tiebreak_sets
	'id', 'name', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'name', 'created_at', 'timestamp', 'tourn'
	from tabroom. ;


 onetab.tiebreaks
	'id', 'name', 'count', 'multiplier', 'priority', 'highlow', 'highlow_count', 'created_at', 'updated_at', 'tiebreak_set_id'
	as select 'id', 'name', 'count', 'multiplier', 'priority', 'highlow', 'highlow_count', 'created_at', 'timestamp', 'tiebreak_set'
	from tabroom. ;


 onetab.timeslots
	'id', 'name', 'start', 'end', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'name', 'start', 'end', 'created_at', 'timestamp', 'tourn'
	from tabroom. ;


 onetab.tourn_admins
	'created_at', 'updated_at', 'person_id', 'tourn_id'
	as select 'created_at', 'timestamp', 'person', 'tourn'
	from tabroom. ;


 onetab.tourn_circuits
	'created_at', 'updated_at', 'tourn_id', 'circuit_id'
	as select 'created_at', 'timestamp', 'tourn', 'circuit'
	from tabroom. ;


 onetab.tourn_fees
	'id', 'amount', 'reason', 'levied_on', 'start', 'end', 'created_at', 'updated_at', 'tourn_id'
	as select 'id', 'amount', 'reason', 'levied_on', 'start', 'end', 'created_at', 'timestamp', 'tourn'
	from tabroom. ;


 onetab.tourn_ignore
	'created_at', 'updated_at', 'person_id', 'tourn_id'
	as select 'created_at', 'timestamp', 'person', 'tourn'
	from tabroom. ;


 onetab.tourn_sites
	'created_at', 'updated_at', 'tourn_id', 'site_id'
	as select 'created_at', 'timestamp', 'tourn', 'site'
	from tabroom. ;


 onetab.tourns
	'id', 'name', 'start', 'end', 'reg_start', 'reg_end', 'hidden', 'webname', 'tz', 'city', 'state', 'country', 'created_at', 'updated_at'
	as select 'id', 'name', 'start', 'end', 'reg_start', 'reg_end', 'hidden', 'webname', 'tz', 'city', 'state', 'country', 'created_at', 'timestamp'
	from tabroom. ;


 onetab.webpages
	'id', 'title', 'content', 'active', 'sitewide', 'page_order', 'created_at', 'updated_at', 'creator_id', 'editor_id', 'tourn_id', 'circuit_id'
	as select 'id', 'title', 'content', 'active', 'sitewide', 'page_order', 'created_at', 'timestamp', 'creator', 'editor', 'tourn', 'circuit'
	from tabroom. ;
