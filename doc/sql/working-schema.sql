
CREATE TABLE `ad` (
  `person` int(11) NOT NULL,
  `approved_by` int(11) NOT NULL,
  `created_at` datetime DEFAULT NULL,

;
CREATE TABLE `autoqueue` (
  `round` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT 0,

--;
CREATE TABLE `ballot` (
  `started_by` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,
  `audited_by` int(11) DEFAULT NULL,
  `judge` int(11) NOT NULL DEFAULT 0,
  `panel` int(11) NOT NULL DEFAULT 0,
  `entry` int(11) NOT NULL DEFAULT 0,


CREATE TABLE `campus_log` (
  `person` int(11) NOT NULL DEFAULT 0,
  `tourn` int(11) NOT NULL DEFAULT 0,
  `panel` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,

CREATE TABLE `category` (
  `tourn` int(11) DEFAULT NULL,
  `pattern` int(11) DEFAULT NULL,


CREATE TABLE `category_setting` (
  `category` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

CREATE TABLE `change_log` (
  `person` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `fine` int(11) DEFAULT NULL,
  `new_panel` int(11) DEFAULT NULL,
  `old_panel` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,

CREATE TABLE `chapter` (
  `district` int(11) DEFAULT NULL,

CREATE TABLE `chapter_circuit` (
  `circuit` int(11) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT 0,
  `region` int(11) DEFAULT NULL,
  `circuit_membership` int(11) DEFAULT NULL,

CREATE TABLE `chapter_judge` (
  `chapter` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `person_request` int(11) DEFAULT NULL,


CREATE TABLE `chapter_setting` (
  `chapter` int(11) DEFAULT NULL,

CREATE TABLE `circuit` (


CREATE TABLE `circuit_setting` (
  `circuit` int(11) NOT NULL DEFAULT 0,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `concession` (
   `tourn` int(11) DEFAULT NULL,


CREATE TABLE `concession_option` (
  `name` varchar(8) NOT NULL,
  `concession_type` int(11) NOT NULL,


CREATE TABLE `concession_purchase` (
  `school` int(11) DEFAULT NULL,
  `concession` int(11) DEFAULT NULL,
  `invoice` int(11) DEFAULT NULL,


CREATE TABLE `concession_purchase_option` (
  `concession_purchase` int(11) NOT NULL,
  `concession_option` int(11) NOT NULL,


CREATE TABLE `concession_type` (
  `name` varchar(31) NOT NULL,
  `concession` int(11) NOT NULL,


CREATE TABLE `conflict` (
  `person` int(11) DEFAULT NULL,
  `conflicted` int(11) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT 0,
  `added_by` int(11) DEFAULT NULL,


CREATE TABLE `district` (


CREATE TABLE `email` (
  `sender` int(11) DEFAULT NULL,
  `hidden` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,


CREATE TABLE `entry` (
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `registered_by` int(11) DEFAULT NULL,



CREATE TABLE `entry_setting` (
  `entry` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `entry_student` (
  `entry` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT 0,
  `created_at` datetime DEFAULT NULL,


CREATE TABLE `event` (
  `tourn` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `pattern` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,


CREATE TABLE `event_setting` (
  `event` int(11) NOT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `file` (
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `webpage` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,


CREATE TABLE `fine` (
  `levied_by` int(11) DEFAULT NULL,
  `deleted_by` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `invoice` int(11) DEFAULT NULL,


CREATE TABLE `follower` (
  `follower` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,


CREATE TABLE `hotel` (
  `tourn` int(11) DEFAULT NULL,


CREATE TABLE `housing` (
  `requestor` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,


CREATE TABLE `housing_slots` (
  `night` date DEFAULT NULL,
  `slots` smallint(6) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,


CREATE TABLE `invoice` (
  `school` int(11) NOT NULL,


CREATE TABLE `jpool` (
  `category` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT 0,
  `parent` int(11) DEFAULT NULL,


CREATE TABLE `jpool_judge` (
  `judge` int(11) NOT NULL DEFAULT 0,
  `jpool` int(11) DEFAULT NULL,


CREATE TABLE `jpool_round` (
  `jpool` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,


CREATE TABLE `jpool_setting` (
  `jpool` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `judge` (
  `school` int(11) NOT NULL DEFAULT 0,
  `category` int(11) DEFAULT NULL,
  `alt_category` int(11) DEFAULT NULL,
  `covers` int(11) DEFAULT NULL,
  `chapter_judge` int(11) DEFAULT NULL,
  `person` int(11) NOT NULL DEFAULT 0,
  `person_request` int(11) DEFAULT NULL,


CREATE TABLE `judge_hire` (
  `requestor` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) NOT NULL DEFAULT 0,
  `region` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,


CREATE TABLE `judge_setting` (
  `judge` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `conditional` int(11) DEFAULT NULL,


CREATE TABLE `login` (
  `person` int(11) DEFAULT NULL,

CREATE TABLE `panel` (  -- HAS BECOME SECTION
  `room` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,


CREATE TABLE `pattern` (
  `exclude` mediumtext DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,


CREATE TABLE `permission` (
  `person` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,


CREATE TABLE `person_setting` (
  `person` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `rating` (
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `rating_tier` int(11) NOT NULL DEFAULT 0,
  `judge` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,
  `sheet` int(11) DEFAULT NULL,


CREATE TABLE `rating_tier` (
  `category` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,


CREATE TABLE `region` (
  `circuit` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,


CREATE TABLE `result` (
  `result_set` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `panel` int(11) DEFAULT NULL,


CREATE TABLE `result_key` (
  `result_set` int(11) DEFAULT NULL,


CREATE TABLE `result_set` (
  `tourn` int(11) NOT NULL,
  `sweep_set` int(11) DEFAULT NULL,
  `sweep_award` int(11) DEFAULT NULL,
  `event` int(11) NOT NULL,


CREATE TABLE `result_value` (
  `result` int(11) DEFAULT NULL,
  `result_key` int(11) NOT NULL DEFAULT 0,
  `tiebreak_set` int(11) NOT NULL DEFAULT 0,


CREATE TABLE `room` (
  `site` int(11) DEFAULT NULL,

CREATE TABLE `room_strike` (
  `room` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,


CREATE TABLE `round` (
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `runoff` int(11) DEFAULT NULL,

CREATE TABLE `round_setting` (
  `round` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

CREATE TABLE `rpool` (
  `name` varchar(31) DEFAULT NULL,
  `tourn` int(11) NOT NULL,

CREATE TABLE `rpool_room` (
  `rpool` int(11) DEFAULT NULL,
  `room` int(11) NOT NULL,

CREATE TABLE `rpool_round` (
  `rpool` int(11) DEFAULT NULL,
  `round` int(11) NOT NULL,

CREATE TABLE `rpool_setting` (
  `rpool` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `school` (
  `tourn` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `state` varchar(4) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,

CREATE TABLE `school_setting` (
  `school` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,

CREATE TABLE `score` (
  `ballot` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT 0,

CREATE TABLE `session` (
  `su` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,

CREATE TABLE `setting` (
  `conditions` text DEFAULT NULL,

CREATE TABLE `setting_label` (
  `setting` int(11) NOT NULL,

CREATE TABLE `shift` (
  `category` int(11) DEFAULT NULL,

CREATE TABLE `site` (
  `host` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,


CREATE TABLE `strike` (
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) NOT NULL DEFAULT 0,
  `event` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `shift` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,


CREATE TABLE `student` (
  `chapter` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `person_request` int(11) DEFAULT NULL,


CREATE TABLE `student_setting` (
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `student_vote` (
  `panel` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `voter` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,


CREATE TABLE `sweep_award` (
  `name` varchar(127) NOT NULL,
  `description` text DEFAULT NULL,
  `target` enum('entry','school','individual') DEFAULT NULL,
  `period` enum('annual','cumulative') DEFAULT NULL,
  `count` smallint(6) NOT NULL DEFAULT 0,
  `min_schools` smallint(6) NOT NULL DEFAULT 0,
  `min_entries` smallint(6) NOT NULL DEFAULT 0,
  `circuit` int(11) DEFAULT NULL,


CREATE TABLE `sweep_award_event` (
  `name` varchar(63) NOT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `level` varchar(15) DEFAULT NULL,
  `sweep_set` int(11) DEFAULT NULL,


CREATE TABLE `sweep_event` (
  `sweep_set` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `event_type` enum('all','congress','debate','speech','wsdc','wudc') DEFAULT NULL,
  `event_level` enum('all','open','jv','novice','champ','es-open','es-novice','middle') DEFAULT NULL,
  `nsda_event_category` int(11) DEFAULT NULL,
  `sweep_award_event` int(11) DEFAULT NULL,


CREATE TABLE `sweep_include` (
  `parent` int(11) DEFAULT NULL,
  `child` int(11) DEFAULT NULL,


CREATE TABLE `sweep_rule` (
  `sweep_set` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,


CREATE TABLE `sweep_set` (
  `name` varchar(31) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `sweep_award` int(11) DEFAULT NULL,

######## STOPPED HERE

CREATE TABLE `tiebreak` (
  `name` varchar(15) DEFAULT NULL,
  `count` varchar(15) NOT NULL DEFAULT '0',
  `count_round` int(11) DEFAULT NULL,
  `truncate` int(11) DEFAULT NULL,
  `truncate_smallest` tinyint(1) NOT NULL DEFAULT 0,
  `multiplier` smallint(6) DEFAULT NULL,
  `violation` smallint(6) DEFAULT NULL,
  `priority` smallint(6) DEFAULT NULL,
  `highlow` tinyint(4) DEFAULT NULL,
  `highlow_count` tinyint(4) DEFAULT NULL,
  `child` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,


CREATE TABLE `tiebreak_set` (
  `name` varchar(127) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,


CREATE TABLE `tiebreak_set_setting` (
  `tag` varchar(31) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `value_text` mediumtext DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `timeslot` (
  `name` varchar(63) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,


CREATE TABLE `topic` (
  `tag` varchar(31) DEFAULT NULL,
  `source` varchar(15) DEFAULT NULL,
  `event_type` varchar(31) DEFAULT NULL,
  `topic_text` text DEFAULT NULL,
  `school_year` int(11) DEFAULT NULL,
  `sort_order` smallint(6) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT 0,


CREATE TABLE `tourn` (
  `name` varchar(63) NOT NULL DEFAULT '',
  `city` varchar(31) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `tz` varchar(31) DEFAULT NULL,
  `webname` varchar(64) DEFAULT NULL,
  `hidden` tinyint(1) NOT NULL DEFAULT 0,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `reg_start` datetime DEFAULT NULL,
  `reg_end` datetime DEFAULT NULL,


CREATE TABLE `tourn_circuit` (
  `approved` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) NOT NULL DEFAULT 0,
  `circuit` int(11) NOT NULL DEFAULT 0,


CREATE TABLE `tourn_fee` (
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `amount` decimal(8,2) DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,


CREATE TABLE `tourn_ignore` (
  `tourn` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,


CREATE TABLE `tourn_setting` (
  `tourn` int(11) NOT NULL DEFAULT 0,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,


CREATE TABLE `tourn_site` (
  `tourn` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT 0,


CREATE TABLE `webpage` (
  `title` varchar(63) DEFAULT NULL,
  `content` mediumtext DEFAULT NULL,
  `published` tinyint(1) NOT NULL DEFAULT 0,
  `sitewide` tinyint(1) NOT NULL DEFAULT 0,
  `special` varchar(15) DEFAULT NULL,
  `page_order` smallint(6) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `last_editor` int(11) DEFAULT NULL,

CREATE TABLE `weekend` (
  `name` varchar(64) NOT NULL,
  `start` datetime NOT NULL,
  `end` datetime NOT NULL,
  `reg_start` datetime NOT NULL,
  `reg_end` datetime NOT NULL,
  `freeze_deadline` datetime NOT NULL,
  `drop_deadline` datetime NOT NULL,
  `judge_deadline` datetime NOT NULL,
  `fine_deadline` datetime NOT NULL,
  `tourn` int(11) NOT NULL,
  `site` int(11) NOT NULL,
  `city` varchar(127) DEFAULT NULL,
  `state` varchar(127) DEFAULT NULL,


-- Dump completed on 2021-02-22 15:12:59
