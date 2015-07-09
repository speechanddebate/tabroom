
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `updated_at` datetime NOT NULL,
  `circuit` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `jpool` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `jpool_round` ( 
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `jpool` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

rename table room_group to rpool;
rename table room_group_room to rpool_room;
alter table rpool_room change room_group rpool int(11);
rename table room_group_round to rpool_round;
alter table rpool_round change room_group rpool int(11);

rename table pool to jpool;
rename table pool_judge to jpool_judge;
alter table jpool_judge change pool jpool int(11);


/*!40101 SET character_set_client = @saved_cs_client */;

create index school on setting(school);
create index tourn on setting(tourn);
create index person on setting(person);
create index judge_group on setting(judge_group);
create index judge on setting(judge);
create index jpool on setting(jpool);
create index round on setting(round);
create index event on setting(event);
create index circuit on setting(circuit);
create index tiebreak_set on setting(tiebreak_set);

delete from tourn_setting where value="text" and value_text is null;
delete from tourn_setting where value="text" and value_text="0";

delete from tourn_setting where value="date" and value_date is null;
delete from tourn_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, tourn)
	select 'tourn', tourn_set.tag, tourn_set.value, tourn_set.value_text, tourn_set.value_date, tourn_set.timestamp, tourn_set.tourn
	from tourn_setting tourn_set;

delete from account_setting where value="text" and value_text is null;
delete from account_setting where value="text" and value_text="0";

delete from account_setting where value="date" and value_date is null;
delete from account_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, person)
	select 'person', person_set.tag, person_set.value, person_set.value_text, person_set.value_date, person_set.timestamp, person_set.account
	from account_setting person_set;

delete from circuit_setting where value="text" and value_text is null;
delete from circuit_setting where value="text" and value_text="0";

delete from circuit_setting where value="date" and value_date is null;
delete from circuit_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, circuit)
	select 'circuit', circuit_set.tag, circuit_set.value, circuit_set.value_text, circuit_set.value_date, circuit_set.timestamp, circuit_set.circuit
	from circuit_setting circuit_set;

delete from event_setting where value="text" and value_text is null;
delete from event_setting where value="text" and value_text="0";

delete from event_setting where value="date" and value_date is null;
delete from event_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, event)
	select 'event', event_set.tag, event_set.value, event_set.value_text, event_set.value_date, event_set.timestamp, event_set.event
	from event_setting event_set;

delete from judge_group_setting where value="text" and value_text is null;
delete from judge_group_setting where value="text" and value_text="0";

delete from judge_group_setting where value="date" and value_date is null;
delete from judge_group_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, judge_group)
	select 'class', judge_group_set.tag, judge_group_set.value, judge_group_set.value_text, judge_group_set.value_date, judge_group_set.timestamp, judge_group_set.judge_group
	from judge_group_setting judge_group_set;


delete from judge_setting where value="text" and value_text is null;
delete from judge_setting where value="text" and value_text="0";

delete from judge_setting where value="date" and value_date is null;
delete from judge_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, judge)
	select 'judge', judge_set.tag, judge_set.value, judge_set.value_text, judge_set.value_date, judge_set.timestamp, judge_set.judge
	from judge_setting judge_set;

insert into setting (type, tag, value, created_at, tiebreak_set)
	select 'tiebreak_set', tiebreak_set_set.tag, tiebreak_set_set.value, tiebreak_set_set.timestamp, tiebreak_set_set.tiebreak_set
	from tiebreak_setting tiebreak_set_set;

delete from tourn_setting where value="text" and value_text is null;
delete from tourn_setting where value="text" and value_text="0";

delete from tourn_setting where value="date" and value_date is null;
delete from tourn_setting where value="date" and value_date="0";

insert into setting (type, tag, value, value_text, value_date, created_at, tourn)
	select 'tourn', tourn_set.tag, tourn_set.value, tourn_set.value_text, tourn_set.value_date, tourn_set.timestamp, tourn_set.tourn
	from tourn_setting tourn_set;

insert into setting (type, tag, value, created_at, round) select 'round', 'ignore_results', '1', round.timestamp, round.id from round where round.ignore_results = 1;
insert into setting (type, tag, value, created_at, round) select 'round', 'reset_room_moves', , round.timestamp, round.id from round where round.wipe_rooms = 1;
insert into setting (type, tag, value, created_at, round) select 'round', 'sidelock_against', round.sidelock_against, round.timestamp, round.id from round where round.sidelock_against > 0;
insert into setting (type, tag, value, created_at, round) select 'round', 'include_room_notes', '1', round.timestamp, round.id from round where round.include_room_notes = 1;
insert into setting (type, tag, value, created_at, round) select 'round', 'publish_entry_list', '1', round.timestamp, round.id from round where round.listed = 1;
insert into setting (type, tag, value, created_at, round) select 'round', 'num_judges', round.judges, round.timestamp, round.id from round where round.judges > 0;
insert into setting (type, tag, value, created_at, round) select 'round', 'cat_id', round.cat_id, round.timestamp, round.id from round where round.cat_id > 0;

insert into setting (type, tag, value, value_text, created_at, round) select 'round', 'motion', 'text', round.motion, round.timestamp, round.id from round where round.motion is not null;
insert into setting (type, tag, value, value_text, created_at, round) select 'round', 'notes', 'text', round.note, round.timestamp, round.id from round where round.note is not null;
insert into setting (type, tag, value, value_date, created_at, round) select 'round', 'completed', 'date', round.completed, round.timestamp, round.id from round where round.completed is not null;
insert into setting (type, tag, value, value_date, created_at, round) select 'round', 'blasted', 'date', round.blasted, round.timestamp, round.id from round where round.blasted is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'judge', 'special_job', judge.special, judge.timestamp, judge.id from judge where judge.special is not null;
insert into setting (type, tag, value, created_at, round) 
	select 'judge', 'gender', judge.gender, judge.timestamp, judge.id from judge where judge.gender is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'judge', 'hire_offer', judge.hire_offer, judge.timestamp, judge.id from judge where judge.hire_offer is not null;
insert into setting (type, tag, value, created_at, round) 
	select 'judge', 'hire_approved', judge.hire_approved, judge.timestamp, judge.id from judge where judge.hire_approved is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'judge', 'diverse', judge.diverse, judge.timestamp, judge.id from judge where judge.diverse is not null;


insert into setting (type, tag, value, created_at, round) 
	select 'jpool', 'standby', jpool.standby, jpool.timestamp, jpool.id from jpool where jpool.standby is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'jpool', 'publish', jpool.publish, jpool.timestamp, jpool.id from jpool where jpool.publish is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'jpool', 'registrant', jpool.registrant, jpool.timestamp, jpool.id from jpool where jpool.registrant is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'jpool', 'burden', jpool.burden, jpool.timestamp, jpool.id from jpool where jpool.burden is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'jpool', 'event_based', jpool.event_based, jpool.timestamp, jpool.id from jpool where jpool.event_based is not null;

insert into setting (type, tag, value, created_at, round) 
	select 'jpool', 'standby_timeslot', jpool.standby_timeslot, jpool.timestamp, jpool.id from jpool where jpool.standby_timeslot is not null;

update table setting set tag="prelim_jpool" where tag="prelim_pool";
update table setting set tag="prelim_jpool_name" where tag="prelim_pool_name";
