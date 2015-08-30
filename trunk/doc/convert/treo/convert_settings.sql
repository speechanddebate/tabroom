
DROP TABLE IF EXISTS `round_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `round_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `round` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create index round on round_setting(round);

DROP TABLE IF EXISTS `school_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `school` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create index school on school_setting(school);
DROP TABLE IF EXISTS `rpool_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpool_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `rpool` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create index rpool on rpool_setting(rpool);

DROP TABLE IF EXISTS `jpool_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `jpool_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `jpool` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create index jpool on jpool_setting(jpool);

DROP TABLE IF EXISTS `entry_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `entry` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
create index entry on entry_setting(entry);

alter table entry modify tba int not null default 0;

create table `jpool_round` ( 
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `jpool` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create index round  on jpool_round(round);
create index jpool  on jpool_round(jpool);

rename table room_group to rpool;
rename table room_group_room to rpool_room;

alter table rpool_room drop foreign key rpool_room_ibfk_1; 
alter table rpool_round drop foreign key rpool_round_ibfk_1; 

alter table rpool_room change room_group rpool int(11);
rename table room_group_round to rpool_round;
alter table rpool_round change room_group rpool int(11);

rename table pool to jpool;
rename table pool_judge to jpool_judge;
alter table jpool_judge change pool jpool int(11);

/*!40101 SET character_set_client = @saved_cs_client */;

delete from tourn_setting where value="text" and value_text is null;
delete from tourn_setting where value="text" and value_text="0";

delete from tourn_setting where value="date" and value_date is null;
delete from tourn_setting where value="date" and value_date="0";

delete from account_setting where value="text" and value_text is null;
delete from account_setting where value="text" and value_text="0";

delete from account_setting where value="date" and value_date is null;
delete from account_setting where value="date" and value_date="0";

delete from circuit_setting where value="text" and value_text is null;
delete from circuit_setting where value="text" and value_text="0";

delete from circuit_setting where value="date" and value_date is null;
delete from circuit_setting where value="date" and value_date="0";

delete from event_setting where value="text" and value_text is null;
delete from event_setting where value="text" and value_text="0";

delete from event_setting where value="date" and value_date is null;
delete from event_setting where value="date" and value_date="0";

delete from judge_group_setting where value="text" and value_text is null;
delete from judge_group_setting where value="text" and value_text="0";

delete from judge_group_setting where value="date" and value_date is null;
delete from judge_group_setting where value="date" and value_date="0";

delete from judge_setting where value="text" and value_text is null;
delete from judge_setting where value="text" and value_text="0";

delete from judge_setting where value="date" and value_date is null;
delete from judge_setting where value="date" and value_date="0";

delete from tourn_setting where value="text" and value_text is null;
delete from tourn_setting where value="text" and value_text="0";

delete from tourn_setting where value="date" and value_date is null;
delete from tourn_setting where value="date" and value_date="0";

alter table round_setting add created_at timestamp;
alter table school_setting add created_at timestamp;
alter table tourn_setting add created_at timestamp;
alter table event_setting add created_at timestamp;

insert into round_setting (tag, value, created_at, round) select 'ignore_results', '1', round.timestamp, round.id from round where round.ignore_results = 1;
insert into round_setting (tag, value, created_at, round) select  'reset_room_moves', '1', round.timestamp, round.id from round where round.wipe_rooms = 1;

insert into round_setting (tag, value, created_at, round) select  'sidelock_against', round.sidelock_against, round.timestamp, round.id from round where round.sidelock_against > 0;
insert into round_setting (tag, value, created_at, round) select  'include_room_notes', '1', round.timestamp, round.id from round where round.include_room_notes = 1;
insert into round_setting (tag, value, created_at, round) select  'publish_entry_list', '1', round.timestamp, round.id from round where round.listed = 1;
insert into round_setting (tag, value, created_at, round) select  'num_judges', round.judges, round.timestamp, round.id from round where round.judges > 0;
insert into round_setting (tag, value, created_at, round) select  'cat_id', round.cat_id, round.timestamp, round.id from round where round.cat_id > 0;
insert into round_setting (tag, value, value_text, created_at, round) select  'motion', 'text', round.motion, round.timestamp, round.id from round where round.motion is not null;
insert into round_setting (tag, value, value_text, created_at, round) select  'notes', 'text', round.note, round.timestamp, round.id from round where round.note is not null;
insert into round_setting (tag, value, value_date, created_at, round) select  'completed', 'date', round.completed, round.timestamp, round.id from round where round.completed is not null;
insert into round_setting (tag, value, value_date, created_at, round) select  'blasted', 'date', round.blasted, round.timestamp, round.id from round where round.blasted is not null;

alter table judge_setting add created_at timestamp;
alter table jpool_setting add created_at timestamp;

update judge set special = NULL where special = "";
insert into judge_setting (tag, value, created_at, judge) select 'tab_rating', judge.tab_rating, judge.timestamp, judge.id from judge where judge.tab_rating > 0;
insert into judge_setting (tag, value, created_at, judge) select 'special_job', judge.special, judge.timestamp, judge.id from judge where judge.special is not null;
insert into judge_setting (tag, value, created_at, judge) select 'gender', judge.gender, judge.timestamp, judge.id from judge where judge.gender is not null;
insert into judge_setting (tag, value, created_at, judge) select 'hire_offer', judge.hire_offer, judge.timestamp, judge.id from judge where judge.hire_offer is not null;
insert into judge_setting (tag, value, created_at, judge) select 'hire_approved', judge.hire_approved, judge.timestamp, judge.id from judge where judge.hire_approved is not null;
insert into judge_setting (tag, value, created_at, judge) select 'diverse', judge.diverse, judge.timestamp, judge.id from judge where judge.diverse is not null;
insert into judge_setting (tag, value, created_at, judge) select  'cat_id', judge.cat_id, judge.timestamp, judge.id from judge where judge.cat_id > 0;

alter table judge drop tab_rating;
alter table judge drop special;
alter table judge drop gender;
alter table judge drop hire_offer;
alter table judge drop hire_approved;
alter table judge drop diverse;
alter table judge drop dropped;
alter table judge drop drop_by;
alter table judge drop drop_time;

update judge_setting set tag="prelim_jpool" where tag="prelim_pool";
update judge_setting set tag="prelim_jpool_name" where tag="prelim_pool_name";

insert into jpool_setting (tag, value, created_at, jpool) select 'standby', jpool.standby, jpool.timestamp, jpool.id from jpool where jpool.standby is not null;
insert into jpool_setting (tag, value, created_at, jpool) select 'publish', jpool.publish, jpool.timestamp, jpool.id from jpool where jpool.publish is not null;
insert into jpool_setting (tag, value, created_at, jpool) select 'registrant', jpool.registrant, jpool.timestamp, jpool.id from jpool where jpool.registrant is not null;
insert into jpool_setting (tag, value, created_at, jpool) select 'burden', jpool.burden, jpool.timestamp, jpool.id from jpool where jpool.burden is not null;
insert into jpool_setting (tag, value, created_at, jpool) select 'event_based', jpool.event_based, jpool.timestamp, jpool.id from jpool where jpool.event_based is not null;
insert into jpool_setting (tag, value, created_at, jpool) select 'standby_timeslot', jpool.standby_timeslot, jpool.timestamp, jpool.id from jpool where jpool.standby_timeslot is not null;


insert into school_setting (tag, value, created_at, school) select 'paid_amount', school.paid, school.timestamp, school.id from school where school.paid > 0;
insert into school_setting (tag, value, created_at, school) select 'noprefs', school.paid, school.timestamp, school.id from school where school.paid > 0;
insert into school_setting (tag, value, created_at, school) select 'congress_code', school.congress_code, school.timestamp, school.id from school where school.congress_code > 0;
insert into school_setting (tag, value, created_at, school) select 'contact_name', school.contact_name, school.timestamp, school.id from school where school.contact_name is not null;
insert into school_setting (tag, value, created_at, school) select 'contact_email', school.contact_email, school.timestamp, school.id from school where school.contact_email is not null;
insert into school_setting (tag, value, created_at, school) select 'contact_number', school.contact_number, school.timestamp, school.id from school where school.contact_number is not null;
insert into school_setting (tag, value, created_at, school) select 'individuals', school.individuals, school.timestamp, school.id from school where school.individuals > 0;

alter table school_fine add deleted bool; 
alter table school_fine add deleted_by int; 
alter table school_fine add deleted_at datetime;
alter table school_fine add payment bool;
alter table school_fine add judge int; 
alter table school_fine add region int; 

