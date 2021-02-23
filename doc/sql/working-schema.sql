-- Table structure for table `ad`

CREATE TABLE `ad` (
  `person` int(11) NOT NULL,
  `approved_by` int(11) NOT NULL,
  `created_at` datetime DEFAULT NULL,

-- Table structure for table `autoqueue`
;
CREATE TABLE `autoqueue` (
  `round` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT 0,

-- Table structure for table `ballot`
--;
CREATE TABLE `ballot` (
  `started_by` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,
  `audited_by` int(11) DEFAULT NULL,
  `judge` int(11) NOT NULL DEFAULT 0,
  `panel` int(11) NOT NULL DEFAULT 0,
  `entry` int(11) NOT NULL DEFAULT 0,

-- Table structure for table `campus_log`

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

-- Table structure for table `category_setting`

CREATE TABLE `category_setting` (
  `category` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `change_log`
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

-- Table structure for table `chapter`
CREATE TABLE `chapter` (
  `district` int(11) DEFAULT NULL,

-- Table structure for table `chapter_circuit`
CREATE TABLE `chapter_circuit` (
  `circuit` int(11) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT 0,
  `region` int(11) DEFAULT NULL,
  `circuit_membership` int(11) DEFAULT NULL,

-- Table structure for table `chapter_judge`
CREATE TABLE `chapter_judge` (
  `chapter` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `person_request` int(11) DEFAULT NULL,

-- Table structure for table `chapter_setting`

CREATE TABLE `chapter_setting` (
  `chapter` int(11) DEFAULT NULL,

CREATE TABLE `circuit` (

-- Table structure for table `circuit_setting`

CREATE TABLE `circuit_setting` (
  `circuit` int(11) NOT NULL DEFAULT 0,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `concession`

CREATE TABLE `concession` (
   `tourn` int(11) DEFAULT NULL,

-- Table structure for table `concession_option`

CREATE TABLE `concession_option` (
  `name` varchar(8) NOT NULL,
  `concession_type` int(11) NOT NULL,

-- Table structure for table `concession_purchase`

CREATE TABLE `concession_purchase` (
  `school` int(11) DEFAULT NULL,
  `concession` int(11) DEFAULT NULL,
  `invoice` int(11) DEFAULT NULL,

-- Table structure for table `concession_purchase_option`

CREATE TABLE `concession_purchase_option` (
  `concession_purchase` int(11) NOT NULL,
  `concession_option` int(11) NOT NULL,

-- Table structure for table `concession_type`

CREATE TABLE `concession_type` (
  `name` varchar(31) NOT NULL,
  `concession` int(11) NOT NULL,

-- Table structure for table `conflict`

CREATE TABLE `conflict` (
  `person` int(11) DEFAULT NULL,
  `conflicted` int(11) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT 0,
  `added_by` int(11) DEFAULT NULL,

-- Table structure for table `district`

CREATE TABLE `district` (

-- Table structure for table `email`

CREATE TABLE `email` (
  `sender` int(11) DEFAULT NULL,
  `hidden` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,

-- Table structure for table `entry`

CREATE TABLE `entry` (
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `registered_by` int(11) DEFAULT NULL,


-- Table structure for table `entry_setting`

CREATE TABLE `entry_setting` (
  `entry` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `entry_student`

CREATE TABLE `entry_student` (
  `entry` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT 0,
  `created_at` datetime DEFAULT NULL,

-- Table structure for table `event`

CREATE TABLE `event` (
  `tourn` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `pattern` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,

-- Table structure for table `event_setting`

CREATE TABLE `event_setting` (
  `event` int(11) NOT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `file`

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

-- Table structure for table `fine`

CREATE TABLE `fine` (
  `levied_by` int(11) DEFAULT NULL,
  `deleted_by` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `invoice` int(11) DEFAULT NULL,

-- Table structure for table `follower`

CREATE TABLE `follower` (
  `follower` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,

-- Table structure for table `hotel`

CREATE TABLE `hotel` (
  `tourn` int(11) DEFAULT NULL,

-- Table structure for table `housing`

CREATE TABLE `housing` (
  `requestor` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,

-- Table structure for table `housing_slots`

CREATE TABLE `housing_slots` (
  `night` date DEFAULT NULL,
  `slots` smallint(6) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,

-- Table structure for table `invoice`

CREATE TABLE `invoice` (
  `school` int(11) NOT NULL,

-- Table structure for table `jpool`

CREATE TABLE `jpool` (
  `name` varchar(63) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT 0,
  `parent` int(11) DEFAULT NULL,

-- Table structure for table `jpool_judge`

CREATE TABLE `jpool_judge` (
  `judge` int(11) NOT NULL DEFAULT 0,
  `jpool` int(11) DEFAULT NULL,

-- Table structure for table `jpool_round`

CREATE TABLE `jpool_round` (
  `jpool` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,

-- Table structure for table `jpool_setting`

CREATE TABLE `jpool_setting` (
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `jpool` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `judge`

CREATE TABLE `judge` (
  `code` varchar(8) DEFAULT NULL,
  `first` varchar(63) DEFAULT NULL,
  `middle` varchar(63) DEFAULT NULL,
  `last` varchar(63) DEFAULT NULL,
  `active` tinyint(1) NOT NULL DEFAULT 0,
  `ada` tinyint(1) NOT NULL DEFAULT 0,
  `obligation` smallint(6) DEFAULT NULL,
  `hired` smallint(6) DEFAULT NULL,
  `school` int(11) NOT NULL DEFAULT 0,
  `category` int(11) DEFAULT NULL,
  `alt_category` int(11) DEFAULT NULL,
  `covers` int(11) DEFAULT NULL,
  `chapter_judge` int(11) DEFAULT NULL,
  `person` int(11) NOT NULL DEFAULT 0,
  `person_request` int(11) DEFAULT NULL,
  `score` int(11) DEFAULT NULL,
  `tmp` varchar(63) DEFAULT NULL,

-- Table structure for table `judge_hire`

CREATE TABLE `judge_hire` (
  `entries_requested` smallint(6) DEFAULT NULL,
  `entries_accepted` smallint(6) DEFAULT NULL,
  `rounds_requested` smallint(6) DEFAULT NULL,
  `rounds_accepted` smallint(6) DEFAULT NULL,
  `requested_at` datetime DEFAULT NULL,
  `requestor` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) NOT NULL DEFAULT 0,
  `region` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,

-- Table structure for table `judge_setting`

CREATE TABLE `judge_setting` (
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `conditional` int(11) DEFAULT NULL,

-- Table structure for table `login`

CREATE TABLE `login` (
  `username` varchar(63) NOT NULL,
  `password` varchar(63) DEFAULT NULL,
  `sha512` varchar(128) DEFAULT NULL,
  `accesses` int(11) NOT NULL DEFAULT 0,
  `last_access` datetime DEFAULT NULL,
  `pass_changekey` varchar(127) DEFAULT NULL,
  `pass_change_expires` datetime DEFAULT NULL,
  `source` char(4) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,

-- Table structure for table `nsda_event_category`

CREATE TABLE `nsda_event_category` (
  `name` varchar(31) DEFAULT NULL,
  `type` enum('s','d','c') DEFAULT NULL,
  `code` smallint(6) DEFAULT NULL,
  `national` tinyint(1) NOT NULL DEFAULT 0,

-- Table structure for table `panel`

CREATE TABLE `panel` (
  `letter` varchar(3) DEFAULT NULL,
  `flight` varchar(3) DEFAULT NULL,
  `bye` tinyint(1) NOT NULL DEFAULT 0,
  `started` datetime DEFAULT NULL,
  `bracket` smallint(6) DEFAULT NULL,
  `flip` varchar(511) DEFAULT NULL,
  `flip_at` datetime DEFAULT NULL,
  `flip_status` enum('winner','loser','anyone','done') NOT NULL DEFAULT 'done',
  `publish` tinyint(1) DEFAULT NULL,
  `room` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `g_event` varchar(63) DEFAULT NULL,
  `room_ext_id` varchar(255) DEFAULT NULL,
  `invites_sent` tinyint(1) NOT NULL DEFAULT 0,

-- Table structure for table `panel_setting`

CREATE TABLE `panel_setting` (
  `panel` int(11) DEFAULT NULL,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `pattern`

CREATE TABLE `pattern` (
  `name` varchar(31) DEFAULT NULL,
  `type` tinyint(4) DEFAULT NULL,
  `max` tinyint(4) DEFAULT NULL,
  `exclude` mediumtext DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,

-- Table structure for table `permission`

CREATE TABLE `permission` (
  `tag` varchar(15) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `details` text DEFAULT NULL,

-- Table structure for table `person`

CREATE TABLE `person` (
  `email` varchar(127) NOT NULL DEFAULT '',
  `first` varchar(63) DEFAULT NULL,
  `middle` varchar(63) DEFAULT NULL,
  `last` varchar(63) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `pronoun` varchar(63) DEFAULT NULL,
  `no_email` tinyint(1) NOT NULL DEFAULT 0,
  `street` varchar(127) DEFAULT NULL,
  `city` varchar(63) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `zip` varchar(15) DEFAULT NULL,
  `postal` varchar(15) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `tz` varchar(63) DEFAULT NULL,
  `phone` varchar(31) DEFAULT NULL,
  `provider` varchar(63) DEFAULT NULL,
  `site_admin` tinyint(1) DEFAULT NULL,
  `diversity` tinyint(1) DEFAULT NULL,
  `nsda` int(11) DEFAULT NULL,

-- Table structure for table `person_setting`

CREATE TABLE `person_setting` (
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `qualifier`

CREATE TABLE `qualifier` (
  `name` varchar(63) DEFAULT NULL,
  `result` varchar(127) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `qualifier_tourn` int(11) DEFAULT NULL,

-- Table structure for table `rating`

CREATE TABLE `rating` (
  `type` enum('school','entry','coach') DEFAULT NULL,
  `draft` tinyint(1) NOT NULL DEFAULT 0,
  `entered` datetime DEFAULT NULL,
  `ordinal` smallint(6) NOT NULL DEFAULT 0,
  `percentile` decimal(8,2) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `rating_tier` int(11) NOT NULL DEFAULT 0,
  `judge` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,
  `sheet` int(11) DEFAULT NULL,

-- Table structure for table `rating_subset`

CREATE TABLE `rating_subset` (
  `name` varchar(63) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,

-- Table structure for table `rating_tier`

CREATE TABLE `rating_tier` (
  `type` enum('coach','mpj') DEFAULT NULL,
  `name` varchar(15) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `strike` tinyint(1) NOT NULL DEFAULT 0,
  `conflict` tinyint(1) NOT NULL DEFAULT 0,
  `min` decimal(8,2) DEFAULT NULL,
  `max` decimal(8,2) DEFAULT NULL,
  `start` tinyint(1) NOT NULL DEFAULT 0,
  `category` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,

-- Table structure for table `region`

CREATE TABLE `region` (
  `name` varchar(127) DEFAULT NULL,
  `code` varchar(31) DEFAULT NULL,
  `quota` tinyint(4) DEFAULT NULL,
  `arch` tinyint(1) DEFAULT NULL,
  `cooke_pts` int(11) DEFAULT NULL,
  `sweeps` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,

-- Table structure for table `region_fine`

CREATE TABLE `region_fine` (
  `amount` float DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `levied_at` datetime DEFAULT NULL,
  `levied_by` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,

-- Table structure for table `region_setting`

CREATE TABLE `region_setting` (
  `tag` varchar(32) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,

-- Table structure for table `result`

CREATE TABLE `result` (
  `rank` smallint(6) DEFAULT NULL,
  `place` varchar(15) DEFAULT NULL,
  `percentile` decimal(6,2) DEFAULT NULL,
  `honor` varchar(255) DEFAULT NULL,
  `honor_site` varchar(63) DEFAULT NULL,
  `result_set` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `panel` int(11) DEFAULT NULL,

-- Table structure for table `result_key`

CREATE TABLE `result_key` (
  `tag` varchar(63) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `no_sort` tinyint(1) DEFAULT NULL,
  `sort_desc` tinyint(1) DEFAULT NULL,
  `result_set` int(11) DEFAULT NULL,

-- Table structure for table `result_set`

CREATE TABLE `result_set` (
  `label` varchar(255) DEFAULT NULL,
  `bracket` tinyint(1) NOT NULL DEFAULT 0,
  `published` tinyint(1) NOT NULL DEFAULT 0,
  `coach` tinyint(1) DEFAULT NULL,
  `generated` datetime DEFAULT NULL,
  `tourn` int(11) NOT NULL,
  `sweep_set` int(11) DEFAULT NULL,
  `sweep_award` int(11) DEFAULT NULL,
  `event` int(11) NOT NULL,
  `tag` enum('entry','student','chapter') NOT NULL DEFAULT 'entry',

-- Table structure for table `result_value`

CREATE TABLE `result_value` (
  `value` mediumtext DEFAULT NULL,
  `priority` smallint(6) DEFAULT NULL,
  `result` int(11) DEFAULT NULL,
  `result_key` int(11) NOT NULL DEFAULT 0,
  `tiebreak_set` int(11) NOT NULL DEFAULT 0,

-- Table structure for table `room`

CREATE TABLE `room` (
  `building` varchar(31) DEFAULT NULL,
  `name` varchar(127) NOT NULL DEFAULT '',
  `quality` smallint(6) DEFAULT NULL,
  `capacity` smallint(6) DEFAULT NULL,
  `rowcount` int(11) DEFAULT NULL,
  `seats` tinyint(4) DEFAULT NULL,
  `inactive` tinyint(1) NOT NULL DEFAULT 0,
  `deleted` tinyint(1) NOT NULL DEFAULT 0,
  `ada` tinyint(1) NOT NULL DEFAULT 0,
  `notes` varchar(63) DEFAULT NULL,
  `url` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `judge_url` varchar(255) DEFAULT NULL,
  `judge_password` varchar(255) DEFAULT NULL,
  `api` int(11) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,

-- Table structure for table `room_strike`

CREATE TABLE `room_strike` (
  `type` varchar(15) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `room` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,

-- Table structure for table `round`

CREATE TABLE `round` (
  `type` varchar(15) DEFAULT NULL,
  `name` smallint(6) DEFAULT NULL,
  `label` varchar(31) DEFAULT NULL,
  `flighted` tinyint(4) DEFAULT NULL,
  `published` tinyint(4) DEFAULT NULL,
  `post_results` tinyint(4) DEFAULT NULL,
  `post_primary` tinyint(4) DEFAULT NULL,
  `post_secondary` tinyint(4) DEFAULT NULL,
  `post_feedback` tinyint(4) DEFAULT NULL,
  `created` datetime DEFAULT NULL,
  `start_time` datetime DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `runoff` int(11) DEFAULT NULL,

-- Table structure for table `round_setting`

CREATE TABLE `round_setting` (
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `rpool`

CREATE TABLE `rpool` (
  `name` varchar(31) DEFAULT NULL,
  `tourn` int(11) NOT NULL,

-- Table structure for table `rpool_room`

CREATE TABLE `rpool_room` (
  `rpool` int(11) DEFAULT NULL,
  `room` int(11) NOT NULL,

-- Table structure for table `rpool_round`

CREATE TABLE `rpool_round` (
  `rpool` int(11) DEFAULT NULL,
  `round` int(11) NOT NULL,

-- Table structure for table `rpool_setting`

CREATE TABLE `rpool_setting` (
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `rpool` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `school`

CREATE TABLE `school` (
  `name` varchar(127) DEFAULT NULL,
  `code` varchar(15) DEFAULT NULL,
  `onsite` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `state` varchar(4) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,

-- Table structure for table `school_setting`

CREATE TABLE `school_setting` (
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,

-- Table structure for table `score`

CREATE TABLE `score` (
  `tag` varchar(15) DEFAULT NULL,
  `value` float NOT NULL DEFAULT 0,
  `content` mediumtext DEFAULT NULL,
  `topic` varchar(127) DEFAULT NULL,
  `speech` smallint(6) NOT NULL DEFAULT 0,
  `position` tinyint(4) NOT NULL DEFAULT 0,
  `ballot` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT 0,
  `tiebreak` tinyint(4) DEFAULT NULL,
  `cat_id` int(11) DEFAULT NULL,

-- Table structure for table `session`

CREATE TABLE `session` (
  `userkey` varchar(127) DEFAULT NULL,
  `ip` varchar(63) DEFAULT NULL,
  `defaults` mediumtext DEFAULT NULL,
  `su` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `weekend` int(11) DEFAULT NULL,

-- Table structure for table `setting`

CREATE TABLE `setting` (
  `type` varchar(31) NOT NULL,
  `subtype` varchar(31) NOT NULL,
  `tag` varchar(31) NOT NULL,
  `value_type` enum('text','string','bool','integer','decimal','datetime','enum') DEFAULT NULL,
  `conditions` text DEFAULT NULL,

-- Table structure for table `setting_label`

CREATE TABLE `setting_label` (
  `lang` char(2) DEFAULT NULL,
  `label` varchar(127) DEFAULT NULL,
  `guide` text DEFAULT NULL,
  `options` text DEFAULT NULL,
  `setting` int(11) NOT NULL,

-- Table structure for table `shift`

CREATE TABLE `shift` (
  `name` varchar(31) DEFAULT NULL,
  `type` enum('signup','strike','both') DEFAULT NULL,
  `fine` smallint(6) DEFAULT NULL,
  `no_hires` tinyint(1) NOT NULL DEFAULT 0,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `category` int(11) DEFAULT NULL,

-- Table structure for table `site`

CREATE TABLE `site` (
  `name` varchar(127) NOT NULL DEFAULT '',
  `online` tinyint(1) NOT NULL DEFAULT 0,
  `directions` mediumtext DEFAULT NULL,
  `dropoff` varchar(255) DEFAULT NULL,
  `host` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,

-- Table structure for table `stats`

CREATE TABLE `stats` (
  `type` varchar(15) DEFAULT NULL,
  `tag` varchar(31) DEFAULT NULL,
  `value` decimal(8,2) DEFAULT NULL,
  `event` int(11) NOT NULL,

-- Table structure for table `strike`

CREATE TABLE `strike` (
  `type` varchar(31) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `registrant` tinyint(1) NOT NULL DEFAULT 0,
  `conflict` tinyint(1) NOT NULL DEFAULT 0,
  `conflictee` tinyint(1) NOT NULL DEFAULT 0,
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
  `created_at` datetime DEFAULT NULL,
  `dioregion` int(11) DEFAULT NULL,

-- Table structure for table `student`

CREATE TABLE `student` (
  `first` varchar(63) NOT NULL DEFAULT '',
  `middle` varchar(63) DEFAULT NULL,
  `last` varchar(63) NOT NULL DEFAULT '',
  `phonetic` varchar(63) DEFAULT NULL,
  `grad_year` smallint(6) DEFAULT NULL,
  `novice` tinyint(1) NOT NULL DEFAULT 0,
  `retired` tinyint(1) NOT NULL DEFAULT 0,
  `gender` char(1) DEFAULT NULL,
  `diet` varchar(31) DEFAULT NULL,
  `birthdate` date DEFAULT NULL,
  `school_sid` varchar(63) DEFAULT NULL,
  `race` varchar(31) DEFAULT NULL,
  `nsda` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `person_request` int(11) DEFAULT NULL,

-- Table structure for table `student_ballot`

CREATE TABLE `student_ballot` (
  `tag` varchar(15) DEFAULT NULL,
  `panel` int(11) NOT NULL DEFAULT 0,
  `entry` int(11) NOT NULL DEFAULT 0,
  `voter` int(11) NOT NULL DEFAULT 0,
  `value` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,

-- Table structure for table `student_setting`

CREATE TABLE `student_setting` (
  `student` int(11) DEFAULT NULL,
  `tag` varchar(32) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `student_vote`

CREATE TABLE `student_vote` (
  `tag` enum('nominee','rank') NOT NULL DEFAULT 'rank',
  `value` int(11) DEFAULT NULL,
  `panel` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `voter` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,
  `entered_at` datetime DEFAULT NULL,

-- Table structure for table `sweep_award`

CREATE TABLE `sweep_award` (
  `name` varchar(127) NOT NULL,
  `description` text DEFAULT NULL,
  `target` enum('entry','school','individual') DEFAULT NULL,
  `period` enum('annual','cumulative') DEFAULT NULL,
  `count` smallint(6) NOT NULL DEFAULT 0,
  `min_schools` smallint(6) NOT NULL DEFAULT 0,
  `min_entries` smallint(6) NOT NULL DEFAULT 0,
  `circuit` int(11) DEFAULT NULL,

-- Table structure for table `sweep_award_event`

CREATE TABLE `sweep_award_event` (
  `name` varchar(63) NOT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `level` varchar(15) DEFAULT NULL,
  `sweep_set` int(11) DEFAULT NULL,

-- Table structure for table `sweep_event`

CREATE TABLE `sweep_event` (
  `sweep_set` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `event_type` enum('all','congress','debate','speech','wsdc','wudc') DEFAULT NULL,
  `event_level` enum('all','open','jv','novice','champ','es-open','es-novice','middle') DEFAULT NULL,
  `nsda_event_category` int(11) DEFAULT NULL,
  `sweep_award_event` int(11) DEFAULT NULL,

-- Table structure for table `sweep_include`

CREATE TABLE `sweep_include` (
  `parent` int(11) DEFAULT NULL,
  `child` int(11) DEFAULT NULL,

-- Table structure for table `sweep_rule`

CREATE TABLE `sweep_rule` (
  `tag` varchar(31) DEFAULT NULL,
  `value` varchar(63) DEFAULT NULL,
  `place` smallint(6) DEFAULT NULL,
  `count` varchar(15) DEFAULT NULL,
  `rev_min` int(11) DEFAULT NULL,
  `count_round` int(11) DEFAULT NULL,
  `truncate` smallint(6) DEFAULT NULL,
  `sweep_set` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,

-- Table structure for table `sweep_set`

CREATE TABLE `sweep_set` (
  `name` varchar(31) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `sweep_award` int(11) DEFAULT NULL,

-- Table structure for table `tiebreak`

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

-- Table structure for table `tiebreak_set`

CREATE TABLE `tiebreak_set` (
  `name` varchar(127) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,

-- Table structure for table `tiebreak_set_setting`

CREATE TABLE `tiebreak_set_setting` (
  `tag` varchar(31) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `value_text` mediumtext DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `timeslot`

CREATE TABLE `timeslot` (
  `name` varchar(63) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,

-- Table structure for table `topic`

CREATE TABLE `topic` (
  `tag` varchar(31) DEFAULT NULL,
  `source` varchar(15) DEFAULT NULL,
  `event_type` varchar(31) DEFAULT NULL,
  `topic_text` text DEFAULT NULL,
  `school_year` int(11) DEFAULT NULL,
  `sort_order` smallint(6) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT 0,

-- Table structure for table `tourn`

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

-- Table structure for table `tourn_circuit`

CREATE TABLE `tourn_circuit` (
  `approved` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) NOT NULL DEFAULT 0,
  `circuit` int(11) NOT NULL DEFAULT 0,

-- Table structure for table `tourn_fee`

CREATE TABLE `tourn_fee` (
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `amount` decimal(8,2) DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,

-- Table structure for table `tourn_ignore`

CREATE TABLE `tourn_ignore` (
  `tourn` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,

-- Table structure for table `tourn_setting`

CREATE TABLE `tourn_setting` (
  `tourn` int(11) NOT NULL DEFAULT 0,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,

-- Table structure for table `tourn_site`

CREATE TABLE `tourn_site` (
  `tourn` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT 0,

-- Table structure for table `webpage`

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

-- Table structure for table `weekend`

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
