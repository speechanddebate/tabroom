create index tourn on changes(tourn);
create index panel on changes(panel);
create index entry on changes(entry);
create index tourn on double_entry(tourn);

alter table bin rename to strike_time;
alter table class rename to double_entry;
alter table comp rename to entry;
alter table housing rename to housing_student;
alter table item rename to concession;
alter table league rename to circuit;
alter table account_access rename to tourn_admin;
alter table league_admin rename to circuit_admin;
alter table chapter_access rename to chapter_admin;
alter table chapter_league rename to circuit_chapter;
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


drop table flight;
drop table interest;
drop table account_interest;
drop table elim_assign;

