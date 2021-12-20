
alter table quiz add badge_description varchar(511) after badge;
alter table quiz add badge_link varchar(511) after badge;
alter table person_quiz add approved_by int after approved;

