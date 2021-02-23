
alter table ballot drop cat_id;

alter table ballot drop index uk_ballots;
alter table ballot drop speechnumber;
alter table ballot add constraint uk_ballots UNIQUE (`judge`,`entry`,`panel`);
alter table ballot modify speakerorder smallint not null default 0;

alter table ballot drop seed;
alter table ballot drop pullup;
alter table ballot drop collected;
alter table ballot drop collected_by;

alter table panel drop cat_id;
alter table panel drop score;
alter table panel drop label;
alter table panel drop confirmed;
alter table panel modify g_event varchar(63);

alter table panel add flip varchar(511) after bracket;
alter table panel add flip_done bool after flip;
alter table panel add flip_at datetime after flip_done;

