
update event_setting set value="numbers" where value="number" and tag="code_style";

alter table event modify `type` enum('speech','congress','debate','wudc','wsdc','attendee','mock_trial') NOT NULL DEFAULT 'attendee';

alter table event add code_style enum('code_name', 'full_initials', 'initials', 'last_names', 'names', 'names_lastfirst', 'numbers', 'prepend_school', 'register', 'schoolname_code', 'school_first_names', 'school_last_names', 'school_names', 'school_name_only', 'school_number') NOT NULL DEFAULT 'numbers' after fee;

update event, event_setting set event.code_style = event_setting.value where event.id = event_setting.event and event_setting.tag="code_style";

