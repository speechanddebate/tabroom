alter table event modify type enum('speech','congress','debate','wudc','wsdc','attendee');
update event_setting set value = "nsda_campus" where value = "nsda_private";
update event_setting set value = "public_jitsi" where value = "nsda_jitsi";

update tourn_setting set tag = "nc_purchased" where tag = "nsda_campus_purchased";
update tourn_setting set tag = "nc_requested" where tag = "nsda_campus_requested";
