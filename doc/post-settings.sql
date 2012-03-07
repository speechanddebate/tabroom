create index tourn on changes(tourn);
create index panel on changes(panel);
create index entry on changes(entry);

alter table bin rename to strike_time;

alter table class rename to event_double;
create index tourn on event_double(tourn);

alter table comp rename to entry;
alter table housing rename to housing_student;
alter table item rename to concession;
alter table league rename to circuit;
alter table membership rename to circuit_membership;

alter table account_access rename to tourn_admin;
alter table league_admin rename to circuit_admin;
alter table chapter_access rename to chapter_admin;
alter table chapter_league rename to chapter_circuit;
alter table purchase rename to concession_purchase;
alter table qual rename to rating_tier;
alter table qual_subset rename to rating_subset;
alter table roomblock rename to room_strike;
alter table team_member rename to entry_student;

alter table follow_comp rename to follow_entry;
alter table ballot change comp entry int;
alter table changes change comp entry int;
alter table follow_comp change comp entry int;
alter table rating change comp entry int;
alter table strike change comp entry int;
alter table student_result change comp entry int;
alter table entry_student change comp entry int;


alter table chapter_circuit change league circuit int;
alter table due_payment change league circuit int;
alter table due_payment rename to circuit_dues;
alter table email change league circuit int;
alter table circuit_admin change league circuit int;
alter table circuit_chapter change league circuit int;
alter table news change league circuit int;
alter table region change league circuit int;
alter table site change league circuit int;

alter table concession_purchase change item concession int;

alter table rating change qual rating_tier int;
alter table rating_tier change qual_subset rating_subset int;
alter table rating add ordinal int;


drop table bill;
drop table sweep;
drop table flight;
drop table interest;
drop table account_interest;
drop table elim_assign;
drop table resultfile;
drop table method;
drop table schedule;
drop table link;
drop table news;
drop table setting;
drop table coach;

alter table uber rename to chapter_judge;

alter table tournament rename to tourn;
alter table tournament_site rename to tourn_site;
alter table fine rename to school_fine;
alter table changes rename to tourn_change;
alter table no_interest rename to account_ignore;

alter table account drop password;
alter table account drop active;
alter table account drop hotel;
alter table account drop saw_judgewarn;
alter table account drop change_deadline;
alter table account change change_timestamp password_timestamp datetime;
alter table account drop is_cell;
alter table account add provider varchar(63);
alter table account drop type;
alter table account add gender char;
alter table account add started date;
alter table account add paradigm text;

alter table ballot drop rank;
alter table ballot drop points;

alter table ballot change real_rank rank int;
alter table ballot change real_points points int;
alter table ballot drop flight;
alter table ballot drop rankinround;

alter table judge change uber chapter_judge int;
alter table event_double add min int;
alter table double_entry change setting int(3);
alter table event change class event_double int;


