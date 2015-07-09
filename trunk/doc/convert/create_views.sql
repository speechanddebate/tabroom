create view onetab.ballots
	'id', 'side', 'bye', 'forfeit', 'chair', 'speaker_order', 'speech_number', 'collected_time', 'created_at', 'updated_at', 'entry_id', 'judge_id', 'section_id', 'collected_by_id', 'entered_by_id', 'audited_by_id'

create view onetab.calendars
	'id', 'name', 'start', 'end', 'reg_start', 'reg_end', 'webname', 'location', 'city', 'state', 'country', 'tz', 'contact', 'url', 'source', 'hidden', 'created_at', 'updated_at', 'tourn_id', 'person_id'
create view onetab.change_logs
	'id', 'type', 'description', 'created_at', 'updated_at', 'tourn_id', 'class_id', 'event_id', 'webpage_id', 'school_id', 'entry_id', 'judge_id', 'strike_id', 'round_id', 'section_id'
create view onetab.circuit_admins
	'created_at', 'updated_at', 'person_id', 'circuit_id'
create view onetab.circuit_dues
	'id', 'amount', 'paid_on', 'created_at', 'updated_at', 'circuit_id', 'squad_id'
create view onetab.circuit_memberships
	'id', 'name', 'approval_required', 'region_required', 'dues_required', 'dues_amount', 'dues_fiscal_year', 'description', 'created_at', 'updated_at', 'circuit_id'
create view onetab.circuits
	'id', 'name', 'abbr', 'active', 'state', 'country', 'tz', 'website', 'webname', 'created_at', 'updated_at'
create view onetab.classes
	'id', 'name', 'abbr', 'created_at', 'updated_at', 'tourn_id'
create view onetab.concession_purchases
	'id', 'quantity', 'fulfilled', 'created_at', 'updated_at', 'concession_id', 'school_id'
create view onetab.concessions
	'id', 'name', 'price', 'description', 'cap', 'school_cap', 'deadline', 'created_at', 'updated_at', 'tourn_id'
create view onetab.conflicts
	'id', 'name', 'created_at', 'updated_at', 'person_id', 'target_person_id', 'creator_id', 'target_squad_id'
create view onetab.double_entry_sets
	'id', 'name', 'type', 'max', 'created_at', 'updated_at', 'mutex_id'
create view onetab.emails
	'id', 'subject', 'content', 'recipients', 'created_at', 'updated_at', 'sender_id', 'tourn_id', 'region_id', 'circuit_id'
create view onetab.entries
	'id', 'code', 'name', 'dropped', 'waitlisted', 'dq', 'seed', 'ada', 'registered_by', 'tba', 'created_at', 'updated_at', 'event_id', 'school_id'
create view onetab.entry_students
	'created_at', 'updated_at', 'entry_id', 'student_id'
create view onetab.events
	'id', 'name', 'type', 'abbr', 'fee', 'created_at', 'updated_at', 'tourn_id', 'class_id', 'double_entry_set_id'
create view onetab.files
	'id', 'label', 'filename', 'public', 'created_at', 'updated_at', 'circuit_id', 'tourn_id', 'event_id', 'webpage_id', 'school_id', 'result_set_id'
create view onetab.fines
	'id', 'amount', 'reason', 'levied_on', 'created_at', 'updated_at', 'region_id', 'tourn_id', 'school_id', 'judge_id', 'levied_by_id'
create view onetab.followers
	'id', 'cell', 'email', 'domain', 'created_at', 'updated_at', 'tourn_id', 'judge_id', 'entry_id', 'school_id', 'person_id'
create view onetab.hotels
	'id', 'name', 'fee_multiplier', 'created_at', 'updated_at', 'tourn_id'
create view onetab.housing_requests
	'id', 'night', 'type', 'tba', 'waitlist', 'created_at', 'updated_at', 'student_id', 'judge_id', 'school_id', 'requestor_id'
create view onetab.housing_slots
	'id', 'night', 'slots', 'created_at', 'updated_at', 'tourn_id'
create view onetab.jpool_judges
	'created_at', 'updated_at', 'jpool_id', 'judge_id'
create view onetab.jpool_rounds
	'created_at', 'updated_at', 'round_id', 'jpool_id'
create view onetab.jpools
	'id', 'name', 'created_at', 'updated_at', 'class_id', 'site_id'
create view onetab.judge_hires
	'id', 'requested_at', 'entries_requested', 'entries_accepted', 'rounds_requested', 'rounds_accepted', 'created_at', 'updated_at', 'school_id', 'tourn_id', 'judge_id', 'requestor_id'
create view onetab.judges
	'id', 'first', 'last', 'code', 'active', 'ada', 'tab_rating', 'obligation_rounds', 'hired_rounds', 'created_at', 'updated_at', 'squad_judge_id', 'class_id', 'school_id', 'person_id', 'alternate_class_id'
create view onetab.logins
	'id', 'username', 'password', 'sha512', 'name', 'accesses', 'last_access', 'pass_changekey', 'pass_timestamp', 'pass_change_expires', 'source', 'ualt_id', 'created_at', 'updated_at', 'person_id'
create view onetab.people
	'id', 'email', 'first', 'middle', 'last', 'phone', 'alt_phone', 'provider', 'street', 'city', 'state', 'zip', 'country', 'gender', 'ualt_id', 'site_admin', 'no_email', 'multiple', 'tz', 'diversity', 'flags', 'created_at', 'updated_at'
create view onetab.pool_judges
	'created_at', 'updated_at', 'judge_id', 'jpool_id'
create view onetab.pool_rooms
	'created_at', 'updated_at', 'room_id', 'rpool_id'
create view onetab.qualifiers
	'id', 'tag', 'value', 'created_at', 'updated_at', 'event_id', 'entry_id', 'tourn_id', 'qualified_at_id'
create view onetab.rating_sheets
	'id', 'name', 'description', 'created_at', 'updated_at'
create view onetab.rating_subsets
	'id', 'name', 'created_at', 'updated_at', 'class_id'
create view onetab.rating_tiers
	'id', 'name', 'type', 'description', 'strike', 'conflict', 'min', 'max', 'default_tier', 'created_at', 'updated_at', 'rating_subset_id', 'class_id', 'rating_sheet_id'
create view onetab.ratings
	'id', 'ordinal', 'percentile', 'type', 'created_at', 'updated_at', 'school_id', 'entry_id', 'judge_id', 'rating_tier_id', 'rating_subset_id', 'rating_sheet_id'
create view onetab.region_admins
	'created_at', 'updated_at', 'person_id', 'region_id'
create view onetab.regions
	'id', 'name', 'code', 'active', 'ncfl_quota', 'ncfl_archdiocese', 'ncfl_cooke', 'ncfl_sweeps', 'created_at', 'updated_at', 'circuit_id', 'tourn_id'
create view onetab.result_sets
	'id', 'label', 'bracket', 'published', 'created_at', 'updated_at', 'tourn_id', 'event_id', 'person_id'
create view onetab.result_values
	'id', 'tag', 'value', 'priority', 'sort', 'description', 'created_at', 'updated_at', 'result_id'
create view onetab.results
	'id', 'honor', 'honor_site', 'rank', 'percentile', 'created_at', 'updated_at', 'student_id', 'entry_id', 'school_id', 'result_set_id'
create view onetab.room_strikes
	'id', 'type', 'start', 'end', 'created_at', 'updated_at', 'room_id'
create view onetab.rooms
	'id', 'name', 'building', 'quality', 'capacity', 'active', 'ada', 'created_at', 'updated_at', 'site_id'
create view onetab.rounds
	'id', 'number', 'name', 'type', 'published', 'post_results', 'flighted', 'start_time', 'created_at', 'updated_at', 'site_id', 'timeslot_id', 'event_id', 'tiebreak_set_id'
create view onetab.rpool_rooms
	'created_at', 'updated_at', 'rpool_id', 'room_id'
create view onetab.rpool_rounds
	'created_at', 'updated_at', 'round_id', 'rpool_id'
create view onetab.rpools
	'id', 'name', 'created_at', 'updated_at', 'tourn_id'
create view onetab.school_contacts
	'created_at', 'updated_at', 'person_id', 'school_id'
create view onetab.schools
	'id', 'name', 'code', 'paid', 'registered', 'contact_person_id', 'contact_number', 'contact_name', 'contact_email', 'created_at', 'updated_at', 'squad_id', 'tourn_id'
create view onetab.scores
	'id', 'tag', 'value', 'content', 'position', 'speech', 'created_at', 'updated_at', 'student_id', 'ballot_id'
create view onetab.sections
	'id', 'letter', 'flight', 'bye', 'started', 'bracket', 'created_at', 'updated_at', 'room_id', 'round_id'
create view onetab.settings
	'id', 'type', 'tag', 'value', 'value_text', 'value_date', 'created_at', 'updated_at', 'circuit_id', 'tourn_id', 'class_id', 'event_id', 'jpool_id', 'tiebreak_set_id', 'judge_id', 'round_id', 'person_id', 'school_id'
create view onetab.sites
	'id', 'name', 'directions', 'created_at', 'updated_at', 'host_id', 'circuit_id'
create view onetab.squad_admins
	'created_at', 'updated_at', 'person_id', 'squad_id'
create view onetab.squad_circuits
	'id', 'name', 'created_at', 'updated_at', 'region_id', 'squad_id', 'circuit_id', 'circuit_membership_id'
create view onetab.squad_judges
	'id', 'first', 'last', 'gender', 'retired', 'cell', 'ada', 'diet', 'notes', 'notes_timestamp', 'created_at', 'updated_at', 'squad_id', 'person_id', 'person_request_id'
create view onetab.squads
	'id', 'name', 'city', 'state', 'country', 'coaches', 'district_id', 'level', 'naudl', 'ipeds', 'nces', 'nsda', 'created_at', 'updated_at'
create view onetab.stats
	'id', 'type', 'tag', 'value', 'created_at', 'updated_at', 'event_id'
create view onetab.strike_timeslots
	'id', 'name', 'start', 'end', 'fine', 'created_at', 'updated_at', 'class_id'
create view onetab.strikes
	'id', 'type', 'start', 'end', 'hidden', 'created_at', 'updated_at', 'region_id', 'timeslot_id', 'school_id', 'entry_id', 'judge_id', 'entered_by_id', 'strike_timeslot_id'
create view onetab.students
	'id', 'first', 'last', 'grad_year', 'novice', 'retired', 'gender', 'phonetic', 'ualt_id', 'created_at', 'updated_at', 'squad_id', 'person_id'
create view onetab.sweep_events
	'created_at', 'updated_at', 'sweep_set_id', 'event_id'
create view onetab.sweep_include
	'id', 'created_at', 'updated_at', 'child_id', 'parent_id'
create view onetab.sweep_rules
	'id', 'tag', 'value', 'place', 'created_at', 'updated_at'
create view onetab.sweep_sets
	'id', 'name', 'created_at', 'updated_at'
create view onetab.tiebreak_sets
	'id', 'name', 'created_at', 'updated_at', 'tourn_id'
create view onetab.tiebreaks
	'id', 'name', 'count', 'multiplier', 'priority', 'highlow', 'highlow_count', 'created_at', 'updated_at', 'tiebreak_set_id'
create view onetab.timeslots
	'id', 'name', 'start', 'end', 'created_at', 'updated_at', 'tourn_id'
create view onetab.tourn_admins
	'created_at', 'updated_at', 'person_id', 'tourn_id'
create view onetab.tourn_circuits
	'created_at', 'updated_at', 'tourn_id', 'circuit_id'
create view onetab.tourn_fees
	'id', 'amount', 'reason', 'levied_on', 'start', 'end', 'created_at', 'updated_at', 'tourn_id'
create view onetab.tourn_ignore
	'created_at', 'updated_at', 'person_id', 'tourn_id'
create view onetab.tourn_sites
	'created_at', 'updated_at', 'tourn_id', 'site_id'
create view onetab.tourns
	'id', 'name', 'start', 'end', 'reg_start', 'reg_end', 'hidden', 'webname', 'tz', 'city', 'state', 'country', 'created_at', 'updated_at'
create view onetab.webpages
	'id', 'title', 'content', 'active', 'sitewide', 'page_order', 'created_at', 'updated_at', 'creator_id', 'editor_id', 'tourn_id', 'circuit_id'
