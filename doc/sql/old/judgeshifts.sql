
alter table strike_timeslot rename to shift;

alter table strike change strike_timeslot shift int;

alter table shift add type enum('both', 'public', 'strike') default 'both' after name;


