-- MySQL dump 10.13  Distrib 5.1.41, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: tab
-- ------------------------------------------------------
-- Server version	5.1.41-3ubuntu12.10-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(127) NOT NULL DEFAULT '',
  `password` varchar(63) NOT NULL DEFAULT '',
  `passhash` varchar(127) NOT NULL DEFAULT '',
  `type` varchar(63) NOT NULL DEFAULT 'coach',
  `first` varchar(63) DEFAULT NULL,
  `last` varchar(63) DEFAULT NULL,
  `phone` varchar(27) DEFAULT NULL,
  `street` varchar(127) DEFAULT NULL,
  `city` varchar(63) DEFAULT NULL,
  `state` varchar(11) DEFAULT NULL,
  `zip` int(11) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `hotel` varchar(127) DEFAULT NULL,
  `is_cell` tinyint(1) DEFAULT NULL,
  `site_admin` tinyint(1) DEFAULT NULL,
  `saw_judgewarn` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `noemail` tinyint(1) DEFAULT NULL,
  `change_pass_key` varchar(255) DEFAULT NULL,
  `change_timestamp` datetime DEFAULT NULL,
  `change_deadline` datetime DEFAULT NULL,
  `multiple` int(11) DEFAULT NULL,
  `tz` varchar(63) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5433 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `account_access`
--

DROP TABLE IF EXISTS `account_access`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_access` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `contact` tinyint(1) NOT NULL DEFAULT '0',
  `entry` tinyint(1) DEFAULT NULL,
  `nosetup` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2701 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `account_interest`
--

DROP TABLE IF EXISTS `account_interest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_interest` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) NOT NULL DEFAULT '0',
  `interest` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=346 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ballot`
--

DROP TABLE IF EXISTS `ballot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ballot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT '0',
  `panel` int(11) DEFAULT NULL,
  `comp` int(11) DEFAULT NULL,
  `rank` int(11) NOT NULL DEFAULT '0',
  `points` int(11) NOT NULL DEFAULT '0',
  `speakerorder` int(11) NOT NULL DEFAULT '0',
  `tv` tinyint(1) DEFAULT NULL,
  `noshow` tinyint(1) NOT NULL DEFAULT '0',
  `real_rank` int(11) NOT NULL DEFAULT '0',
  `real_points` int(11) NOT NULL DEFAULT '0',
  `seed` int(11) DEFAULT NULL,
  `audit` tinyint(1) NOT NULL DEFAULT '0',
  `rankinround` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `chair` tinyint(1) NOT NULL DEFAULT '0',
  `speechnumber` int(11) DEFAULT NULL,
  `topic` int(11) DEFAULT NULL,
  `win` tinyint(1) DEFAULT NULL,
  `countmenot` tinyint(1) DEFAULT NULL,
  `collected` datetime DEFAULT NULL,
  `collected_by` int(11) NOT NULL DEFAULT '0',
  `flight` char(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `comp` (`comp`),
  KEY `panel` (`panel`),
  KEY `judge` (`judge`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1102778 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bill`
--

DROP TABLE IF EXISTS `bill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bill` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(127) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT '0',
  `file` varchar(63) DEFAULT NULL,
  `approved` tinyint(1) DEFAULT '0',
  `tournament` int(11) DEFAULT NULL,
  `submitted` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bin`
--

DROP TABLE IF EXISTS `bin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_group` int(11) NOT NULL,
  `fine` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `name` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=335 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `changes`
--

DROP TABLE IF EXISTS `changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `changes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(63) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `panel` int(11) DEFAULT NULL,
  `comp` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `judge` int(11) DEFAULT NULL,
  `fine` int(11) DEFAULT NULL,
  `moved_from` int(11) DEFAULT NULL,
  `regline` varchar(255) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=190384 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter`
--

DROP TABLE IF EXISTS `chapter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL DEFAULT '',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `state` char(4) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5520 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_access`
--

DROP TABLE IF EXISTS `chapter_access`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_access` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6237 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_league`
--

DROP TABLE IF EXISTS `chapter_league`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_league` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `league` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) NOT NULL DEFAULT '0',
  `full_member` tinyint(1) NOT NULL DEFAULT '0',
  `code` varchar(12) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `paid` tinyint(1) DEFAULT NULL,
  `membership` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`),
  KEY `league` (`league`)
) ENGINE=InnoDB AUTO_INCREMENT=8408 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `class`
--

DROP TABLE IF EXISTS `class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `class` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `tournament` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `double_entry` int(11) DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `max` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `judge_group` (`judge_group`)
) ENGINE=InnoDB AUTO_INCREMENT=4658 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `coach`
--

DROP TABLE IF EXISTS `coach`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `coach` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3808 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `comp`
--

DROP TABLE IF EXISTS `comp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `school` int(11) NOT NULL DEFAULT '0',
  `event` int(11) NOT NULL DEFAULT '0',
  `code` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT '0',
  `partner` int(11) DEFAULT NULL,
  `dropped` tinyint(1) NOT NULL DEFAULT '0',
  `dq_reason` varchar(127) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `apda_seed` int(11) DEFAULT NULL,
  `tb0` float NOT NULL DEFAULT '0',
  `tb1` float NOT NULL DEFAULT '0',
  `tb2` float NOT NULL DEFAULT '0',
  `tb3` float NOT NULL DEFAULT '0',
  `tb4` float NOT NULL DEFAULT '0',
  `tb5` float NOT NULL DEFAULT '0',
  `tb6` float NOT NULL DEFAULT '0',
  `tb7` float NOT NULL DEFAULT '0',
  `tb8` float NOT NULL DEFAULT '0',
  `tb9` float NOT NULL DEFAULT '0',
  `waitlist` tinyint(1) NOT NULL DEFAULT '0',
  `dq` tinyint(1) NOT NULL DEFAULT '0',
  `results_bar` varchar(127) DEFAULT NULL,
  `bid` tinyint(1) NOT NULL DEFAULT '0',
  `doubled` int(11) DEFAULT NULL,
  `housing` tinyint(1) DEFAULT NULL,
  `partner_housing` tinyint(1) DEFAULT NULL,
  `sweeps_points` int(11) NOT NULL DEFAULT '0',
  `noshow` tinyint(1) DEFAULT NULL,
  `qualifier` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `housing_start` datetime DEFAULT NULL,
  `housing_end` datetime DEFAULT NULL,
  `partner_housing_end` datetime DEFAULT NULL,
  `partner_housing_start` datetime DEFAULT NULL,
  `qual2` varchar(63) DEFAULT NULL,
  `qualexp` varchar(63) DEFAULT NULL,
  `qual2exp` varchar(63) DEFAULT NULL,
  `registered_at` datetime DEFAULT NULL,
  `dropped_at` datetime DEFAULT NULL,
  `title` varchar(127) DEFAULT NULL,
  `ada` int(11) DEFAULT NULL,
  `notes` varchar(128) DEFAULT NULL,
  `wins` int(11) DEFAULT NULL,
  `losses` int(11) DEFAULT NULL,
  `initials` char(1) DEFAULT NULL,
  `trpc_string` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tournament` (`tournament`,`school`),
  KEY `tournament_2` (`tournament`,`school`,`event`),
  KEY `code` (`code`),
  KEY `school` (`school`),
  KEY `id` (`id`),
  KEY `event` (`event`),
  KEY `student` (`student`),
  KEY `partner` (`partner`)
) ENGINE=InnoDB AUTO_INCREMENT=273706 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `due_payment`
--

DROP TABLE IF EXISTS `due_payment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `due_payment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `paid_on` datetime DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `amount` float DEFAULT NULL,
  `league` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=586 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `elim_assign`
--

DROP TABLE IF EXISTS `elim_assign`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `elim_assign` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `round` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1833 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `email`
--

DROP TABLE IF EXISTS `email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `email` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sender` int(11) DEFAULT NULL,
  `blurb` text,
  `subject` varchar(127) DEFAULT NULL,
  `senton` datetime DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `sent_to` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `tournament` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1868 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) NOT NULL DEFAULT '',
  `type` enum('debate','speech','congress') DEFAULT NULL,
  `abbr` varchar(11) DEFAULT NULL,
  `code` int(11) DEFAULT NULL,
  `class` int(11) NOT NULL DEFAULT '0',
  `team` int(11) DEFAULT NULL,
  `double_flight` tinyint(1) DEFAULT NULL,
  `cap` int(11) DEFAULT NULL,
  `blurb` text,
  `fee` float DEFAULT NULL,
  `double_factor` enum('late','early','neutral') NOT NULL DEFAULT 'neutral',
  `ballot` varchar(127) DEFAULT NULL,
  `alumni` tinyint(1) DEFAULT NULL,
  `school_cap` int(11) DEFAULT NULL,
  `allow_judge_own` tinyint(1) NOT NULL DEFAULT '0',
  `waitlist` tinyint(1) DEFAULT NULL,
  `omit_sweeps` tinyint(1) DEFAULT NULL,
  `no_judge_burden` tinyint(1) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `reg_blockable` tinyint(1) NOT NULL DEFAULT '0',
  `ballot_type` varchar(24) DEFAULT NULL,
  `deadline` datetime DEFAULT NULL,
  `ask_for_titles` tinyint(1) DEFAULT NULL,
  `supp` tinyint(1) DEFAULT NULL,
  `no_codes` tinyint(1) DEFAULT NULL,
  `field_report` tinyint(1) DEFAULT NULL,
  `waitlist_all` tinyint(1) DEFAULT NULL,
  `textpost` tinyint(1) DEFAULT NULL,
  `live_updates` tinyint(1) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `reg_codes` tinyint(1) DEFAULT '0',
  `initial_codes` tinyint(1) DEFAULT '0',
  `bids` tinyint(1) DEFAULT '0',
  `min_entry` int(11) DEFAULT NULL,
  `max_entry` int(11) DEFAULT NULL,
  `bid_cume` int(11) DEFAULT NULL,
  `qual_subset` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `tournament` (`tournament`)
) ENGINE=InnoDB AUTO_INCREMENT=14042 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `file`
--

DROP TABLE IF EXISTS `file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `file` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL,
  `label` varchar(127) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `uploaded` datetime DEFAULT NULL,
  `posting` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1245 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fine`
--

DROP TABLE IF EXISTS `fine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fine` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `school` int(11) DEFAULT NULL,
  `amount` float DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `region` int(11) DEFAULT NULL,
  `levied` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5061779 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `flight`
--

DROP TABLE IF EXISTS `flight`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `flight` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `tournament` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=281 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follow_comp`
--

DROP TABLE IF EXISTS `follow_comp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follow_comp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `comp` int(11) DEFAULT NULL,
  `cell` varchar(16) DEFAULT NULL,
  `email` varchar(127) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follow_judge`
--

DROP TABLE IF EXISTS `follow_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follow_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) DEFAULT NULL,
  `cell` varchar(16) DEFAULT NULL,
  `email` varchar(127) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing`
--

DROP TABLE IF EXISTS `housing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `housing` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `type` varchar(7) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `waitlist` tinyint(1) NOT NULL DEFAULT '0',
  `night` date DEFAULT NULL,
  `requested` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11082 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing_slots`
--

DROP TABLE IF EXISTS `housing_slots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `housing_slots` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) DEFAULT NULL,
  `night` date DEFAULT NULL,
  `slots` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `interest`
--

DROP TABLE IF EXISTS `interest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `interest` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL DEFAULT '',
  `active` tinyint(1) DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `closed` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `price` float DEFAULT NULL,
  `description` text,
  `tournament` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `event` int(11) DEFAULT NULL,
  `deadline` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=264 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge`
--

DROP TABLE IF EXISTS `judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `school` int(11) NOT NULL DEFAULT '0',
  `first` varchar(63) NOT NULL DEFAULT '',
  `last` varchar(63) NOT NULL DEFAULT '',
  `code` int(11) DEFAULT NULL,
  `active` int(11) NOT NULL DEFAULT '0',
  `special` text,
  `score` int(11) DEFAULT NULL,
  `notes` text,
  `judge_group` int(11) DEFAULT NULL,
  `neutral` int(11) DEFAULT NULL,
  `tmp` varchar(11) DEFAULT NULL,
  `cfl_tab_first` varchar(127) DEFAULT NULL,
  `cfl_tab_second` varchar(127) DEFAULT NULL,
  `cfl_tab_third` varchar(127) DEFAULT NULL,
  `alt_group` int(11) NOT NULL DEFAULT '0',
  `housing` tinyint(1) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `spare_pool` tinyint(1) NOT NULL DEFAULT '0',
  `hire` int(11) DEFAULT NULL,
  `prelim_pool` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `cell` varchar(63) DEFAULT NULL,
  `first_year` tinyint(1) NOT NULL DEFAULT '0',
  `paradigm` text,
  `covers` int(11) DEFAULT NULL,
  `uber` int(11) DEFAULT NULL,
  `novice` tinyint(1) DEFAULT NULL,
  `obligation` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`),
  KEY `judge_group` (`judge_group`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=85264 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_class`
--

DROP TABLE IF EXISTS `judge_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_class` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT '0',
  `class` int(11) NOT NULL DEFAULT '0',
  `qual` int(11) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`),
  KEY `class` (`class`)
) ENGINE=InnoDB AUTO_INCREMENT=122049 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_group`
--

DROP TABLE IF EXISTS `judge_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `judge_per` float DEFAULT NULL,
  `fee_missing` float DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `dio_min` int(11) DEFAULT NULL,
  `ask_alts` tinyint(1) DEFAULT NULL,
  `missing_judge_fee` float DEFAULT NULL,
  `uncovered_entry_fee` float DEFAULT NULL,
  `tab_room` tinyint(1) DEFAULT NULL,
  `track_judge_hires` tinyint(1) DEFAULT NULL,
  `hired_pool` int(11) DEFAULT NULL,
  `hired_fee` int(11) DEFAULT NULL,
  `special` text,
  `free` int(11) DEFAULT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `collective` tinyint(1) DEFAULT NULL,
  `track_by_pools` tinyint(1) DEFAULT NULL,
  `default_alt_reduce` int(11) DEFAULT NULL,
  `reduce_alt_burden` int(11) DEFAULT NULL,
  `alt_max` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `ask_paradigm` tinyint(1) NOT NULL DEFAULT '0',
  `paradigm_explain` text,
  `elim_special` text,
  `school_strikes` int(11) DEFAULT NULL,
  `strike_reg_opens` datetime DEFAULT NULL,
  `strike_reg_closes` datetime DEFAULT NULL,
  `strikes_explain` text,
  `min_burden` int(11) DEFAULT NULL,
  `max_pool_burden` float DEFAULT NULL,
  `deadline` datetime DEFAULT NULL,
  `ratings_need_paradigms` tinyint(1) DEFAULT NULL,
  `strikes_need_paradigms` tinyint(1) DEFAULT NULL,
  `ratings_need_entry` tinyint(1) DEFAULT NULL,
  `strikes_need_entry` tinyint(1) DEFAULT NULL,
  `comp_strikes` int(11) DEFAULT NULL,
  `coach_ratings` tinyint(1) DEFAULT NULL,
  `school_ratings` tinyint(1) DEFAULT NULL,
  `comp_ratings` tinyint(1) DEFAULT NULL,
  `group_max` int(11) DEFAULT NULL,
  `max_burden` int(11) DEFAULT NULL,
  `pub_assigns` tinyint(1) DEFAULT NULL,
  `obligation_before_strikes` tinyint(1) DEFAULT NULL,
  `cumulate_mjp` tinyint(1) DEFAULT NULL,
  `conflicts` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3527 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_hire`
--

DROP TABLE IF EXISTS `judge_hire`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_hire` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) DEFAULT NULL,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `school` int(11) NOT NULL DEFAULT '0',
  `judge_group` int(11) NOT NULL DEFAULT '0',
  `accepted` int(11) DEFAULT NULL,
  `request_made` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `covers` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7005477 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `league`
--

DROP TABLE IF EXISTS `league`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `league` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `url` varchar(127) DEFAULT NULL,
  `public_email` tinyint(1) DEFAULT NULL,
  `short_name` varchar(63) DEFAULT NULL,
  `admin` int(11) DEFAULT NULL,
  `dues_to` int(11) DEFAULT NULL,
  `region_based` int(11) DEFAULT NULL,
  `diocese_based` tinyint(1) DEFAULT NULL,
  `timezone` varchar(67) DEFAULT NULL,
  `dues_amount` varchar(15) DEFAULT NULL,
  `hosted_site` int(11) DEFAULT NULL,
  `apda_seeds` tinyint(1) DEFAULT NULL,
  `logo_file` varchar(127) DEFAULT NULL,
  `site_theme` varchar(127) DEFAULT NULL,
  `public_results` tinyint(1) DEFAULT NULL,
  `header_file` varchar(127) DEFAULT NULL,
  `invoice_message` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `track_bids` tinyint(1) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `last_change` int(11) DEFAULT NULL,
  `approved` tinyint(1) DEFAULT NULL,
  `tourn_only` tinyint(1) DEFAULT NULL,
  `full_members` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `league_admin`
--

DROP TABLE IF EXISTS `league_admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `league_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `league` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=227 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `link`
--

DROP TABLE IF EXISTS `link`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `link` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `blurb` text,
  `league` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `active` tinyint(1) NOT NULL DEFAULT '0',
  `display_order` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `membership`
--

DROP TABLE IF EXISTS `membership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `membership` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `approval` tinyint(1) DEFAULT NULL,
  `dues` float DEFAULT NULL,
  `dues_start` datetime DEFAULT NULL,
  `dues_expire` datetime DEFAULT NULL,
  `blurb` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `method`
--

DROP TABLE IF EXISTS `method`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `method` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `is_standard` tinyint(1) NOT NULL DEFAULT '0',
  `name` varchar(63) NOT NULL DEFAULT '',
  `mfl_time_violation` tinyint(1) NOT NULL DEFAULT '0',
  `truncate_to_smallest` tinyint(1) NOT NULL DEFAULT '0',
  `drop_worst_rank` tinyint(1) NOT NULL DEFAULT '0',
  `drop_best_rank` tinyint(1) NOT NULL DEFAULT '0',
  `snake_elims` tinyint(1) NOT NULL DEFAULT '0',
  `honorable_mentions` tinyint(1) NOT NULL DEFAULT '0',
  `mfl_flex_finals` tinyint(1) NOT NULL DEFAULT '0',
  `noshows_never_break` tinyint(1) NOT NULL DEFAULT '0',
  `sweep_method` varchar(24) NOT NULL DEFAULT '0',
  `sweep_wildcards` int(11) NOT NULL DEFAULT '0',
  `sweep_event_total` int(11) NOT NULL DEFAULT '0',
  `sweep_class_based` tinyint(1) NOT NULL DEFAULT '0',
  `truncate_ranks_to` int(11) NOT NULL DEFAULT '0',
  `double_entry` varchar(24) NOT NULL DEFAULT '0',
  `judge_event_twice` tinyint(1) NOT NULL DEFAULT '0',
  `judge_quality_system` tinyint(1) NOT NULL DEFAULT '0',
  `school_bump_penalty` int(11) NOT NULL DEFAULT '0',
  `region_bump_penalty` int(11) NOT NULL DEFAULT '0',
  `second_bump_penalty` int(11) NOT NULL DEFAULT '0',
  `full_panel_penalty` int(11) NOT NULL DEFAULT '0',
  `min_panel_size` int(11) NOT NULL DEFAULT '0',
  `default_panel_size` int(11) NOT NULL DEFAULT '0',
  `max_panel_size` int(11) NOT NULL DEFAULT '0',
  `min_chamber_size` int(11) NOT NULL DEFAULT '0',
  `default_chamber_size` int(11) NOT NULL DEFAULT '0',
  `max_chamber_size` int(11) NOT NULL DEFAULT '0',
  `default_qualification` int(11) NOT NULL DEFAULT '0',
  `sweep_per_event` int(11) NOT NULL DEFAULT '0',
  `large_schools_first` tinyint(1) NOT NULL DEFAULT '0',
  `allow_school_panels` tinyint(1) NOT NULL DEFAULT '0',
  `add_fine` float NOT NULL DEFAULT '0',
  `drop_fine` float NOT NULL DEFAULT '0',
  `sweep_count_elims` tinyint(1) NOT NULL DEFAULT '0',
  `sweep_count_prelims` tinyint(1) NOT NULL DEFAULT '0',
  `sweep_rank_in_elims` tinyint(1) NOT NULL DEFAULT '0',
  `sweep_final_rank` tinyint(1) NOT NULL DEFAULT '0',
  `sweep_count_finals` tinyint(1) NOT NULL DEFAULT '0',
  `allow_judge_own` tinyint(1) NOT NULL DEFAULT '0',
  `noshow_judge_fine` float DEFAULT NULL,
  `bid_min_cume` int(11) DEFAULT NULL,
  `bid_percent` int(11) DEFAULT NULL,
  `bid_round_to_rank` tinyint(1) DEFAULT NULL,
  `bid_min_round` int(11) DEFAULT NULL,
  `league` int(11) NOT NULL DEFAULT '0',
  `housing` int(11) NOT NULL DEFAULT '0',
  `housing_message` text,
  `points_per_finalist` float DEFAULT NULL,
  `bid_min_round_type` varchar(12) DEFAULT NULL,
  `points_per_elim` float DEFAULT NULL,
  `hide_codes` tinyint(1) DEFAULT NULL,
  `bid_min_number` int(11) DEFAULT NULL,
  `elim_method` varchar(63) DEFAULT NULL,
  `allow_neutral_judges` tinyint(1) DEFAULT NULL,
  `ask_qualifying_tournament` tinyint(1) DEFAULT NULL,
  `elim_method_basis` varchar(15) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `judge_cells` tinyint(1) DEFAULT NULL,
  `track_first_year` tinyint(1) NOT NULL DEFAULT '0',
  `publish_schools` tinyint(1) NOT NULL DEFAULT '0',
  `incremental_school_codes` tinyint(1) NOT NULL DEFAULT '0',
  `first_school_code` varchar(15) DEFAULT NULL,
  `schemat_school_code` tinyint(1) NOT NULL DEFAULT '0',
  `schemat_display` varchar(15) NOT NULL DEFAULT '0',
  `per_student_fee` float NOT NULL DEFAULT '0',
  `publish_paradigms` tinyint(1) NOT NULL DEFAULT '0',
  `must_pay_dues` tinyint(1) NOT NULL DEFAULT '0',
  `no_back_to_back` tinyint(1) NOT NULL DEFAULT '0',
  `master_printouts` varchar(24) DEFAULT NULL,
  `ask_two_quals` tinyint(1) NOT NULL DEFAULT '0',
  `audit_method` varchar(15) DEFAULT NULL,
  `require_adult_contact` tinyint(1) DEFAULT NULL,
  `novices` varchar(15) DEFAULT NULL,
  `points_per_entry` int(11) DEFAULT NULL,
  `sweep_only_place_final` tinyint(1) DEFAULT NULL,
  `track_reg_changes` tinyint(1) DEFAULT NULL,
  `drop_worst_elim` int(11) DEFAULT NULL,
  `drop_best_elim` int(11) DEFAULT NULL,
  `drop_worst_final` int(11) DEFAULT NULL,
  `drop_best_final` int(11) DEFAULT NULL,
  `rating_system` varchar(15) DEFAULT NULL,
  `rating_covers` varchar(15) DEFAULT NULL,
  `hide_debate_codes` tinyint(1) DEFAULT NULL,
  `noshow_judge_fine_elim` int(11) DEFAULT NULL,
  `concession_name` varchar(127) DEFAULT NULL,
  `track_novice_judges` tinyint(1) DEFAULT NULL,
  `judge_sheet_notice` text,
  `num_judges` int(11) DEFAULT NULL,
  `disable_region_strikes` tinyint(1) DEFAULT NULL,
  `double_max` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1250 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `news`
--

DROP TABLE IF EXISTS `news`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(127) DEFAULT NULL,
  `blurb` text,
  `author` int(11) DEFAULT NULL,
  `posted` datetime DEFAULT NULL,
  `file` varchar(127) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `link` int(11) DEFAULT NULL,
  `main_site` tinyint(1) DEFAULT NULL,
  `league` int(11) NOT NULL DEFAULT '0',
  `edited_by` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `sitewide` tinyint(1) DEFAULT NULL,
  `edited_on` datetime DEFAULT NULL,
  `pinned` tinyint(1) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `display_order` int(11) DEFAULT NULL,
  `special` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=525 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `no_interest`
--

DROP TABLE IF EXISTS `no_interest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `no_interest` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=756 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `panel`
--

DROP TABLE IF EXISTS `panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `panel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `event` int(11) NOT NULL DEFAULT '0',
  `room` int(11) NOT NULL DEFAULT '0',
  `letter` char(2) NOT NULL DEFAULT '',
  `type` varchar(63) NOT NULL DEFAULT '',
  `round` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `nosweep` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `room` (`room`),
  KEY `event` (`event`),
  KEY `round` (`round`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=111679 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pool`
--

DROP TABLE IF EXISTS `pool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `site` int(11) NOT NULL DEFAULT '0',
  `name` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `timeslot` int(11) DEFAULT NULL,
  `standby` tinyint(1) DEFAULT NULL,
  `publish` tinyint(1) DEFAULT NULL,
  `registrant` tinyint(1) DEFAULT NULL,
  `burden` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1107 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pool_group`
--

DROP TABLE IF EXISTS `pool_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pool_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pool` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=346 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pool_judge`
--

DROP TABLE IF EXISTS `pool_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pool_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT '0',
  `pool` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `type` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pool` (`pool`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=33432 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pool_round`
--

DROP TABLE IF EXISTS `pool_round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pool_round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pool` int(11) NOT NULL,
  `round` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=268 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `purchase`
--

DROP TABLE IF EXISTS `purchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `purchase` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `item` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `school` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5592 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `qual`
--

DROP TABLE IF EXISTS `qual`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `qual` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(12) DEFAULT NULL,
  `description` text,
  `tournament` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `strike` tinyint(1) DEFAULT NULL,
  `type` enum('mpj','qual') DEFAULT NULL,
  `conflict` tinyint(1) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `min` float DEFAULT NULL,
  `max` float DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8812 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `qual_subset`
--

DROP TABLE IF EXISTS `qual_subset`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `qual_subset` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating`
--

DROP TABLE IF EXISTS `rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL,
  `school` int(11) DEFAULT NULL,
  `comp` int(11) DEFAULT NULL,
  `type` enum('school','comp','coach') DEFAULT NULL,
  `qual` int(11) DEFAULT NULL,
  `entered` datetime DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `subset` int(11) DEFAULT NULL,
  `name` char(3) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=275151 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `region`
--

DROP TABLE IF EXISTS `region`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `region` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `director` int(11) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `code` varchar(12) DEFAULT NULL,
  `diocese` tinyint(1) DEFAULT NULL,
  `quota` int(11) DEFAULT NULL,
  `arch` tinyint(1) DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `cooke_pts` int(11) DEFAULT NULL,
  `sweeps` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `resultfile`
--

DROP TABLE IF EXISTS `resultfile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `resultfile` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL,
  `name` varchar(128) DEFAULT NULL,
  `filename` varchar(128) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=547 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) NOT NULL DEFAULT '',
  `quality` int(11) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `capacity` int(11) DEFAULT NULL,
  `inactive` tinyint(1) DEFAULT NULL,
  `notes` varchar(127) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `site` (`site`)
) ENGINE=InnoDB AUTO_INCREMENT=13566 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room_pool`
--

DROP TABLE IF EXISTS `room_pool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room_pool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `room` int(11) DEFAULT NULL,
  `reserved` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=31856 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `roomblock`
--

DROP TABLE IF EXISTS `roomblock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `roomblock` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(67) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `room` int(11) NOT NULL DEFAULT '0',
  `tournament` int(11) NOT NULL DEFAULT '0',
  `special` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `room` (`room`)
) ENGINE=InnoDB AUTO_INCREMENT=1346 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `round`
--

DROP TABLE IF EXISTS `round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `type` varchar(63) DEFAULT NULL,
  `label` varchar(15) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,
  `pool` int(11) DEFAULT NULL,
  `preset` tinyint(1) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `no_first_year` tinyint(1) DEFAULT NULL,
  `published` tinyint(1) DEFAULT NULL,
  `number_judges` int(11) DEFAULT NULL,
  `listed` tinyint(1) DEFAULT NULL,
  `created` datetime DEFAULT NULL,
  `completed` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `timeslot` (`timeslot`),
  KEY `pool` (`pool`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=40453 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `schedule`
--

DROP TABLE IF EXISTS `schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `schedule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `location` varchar(127) DEFAULT NULL,
  `description` text,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `file` varchar(127) DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `school`
--

DROP TABLE IF EXISTS `school`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `sweeps_points` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `code` varchar(15) DEFAULT NULL,
  `score` int(11) DEFAULT NULL,
  `paid_amount` float DEFAULT NULL,
  `registered` tinyint(1) NOT NULL DEFAULT '0',
  `contact_number` varchar(63) DEFAULT NULL,
  `contact_name` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `concession_paid_amount` float DEFAULT NULL,
  `entered` datetime DEFAULT NULL,
  `registered_on` datetime DEFAULT NULL,
  `disclaimed` int(11) DEFAULT NULL,
  `noprefs` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=26500 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `session`
--

DROP TABLE IF EXISTS `session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `session` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `timestamp` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  `account` int(11) NOT NULL DEFAULT '0',
  `authkey` varchar(63) NOT NULL DEFAULT '',
  `ip` varchar(63) DEFAULT NULL,
  `limited` tinyint(1) DEFAULT NULL,
  `entry` tinyint(1) DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `userkey` varchar(127) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `director` int(11) DEFAULT NULL,
  `ie_annoy` tinyint(1) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=145249 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `tournament` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=296 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `site`
--

DROP TABLE IF EXISTS `site`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `site` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL DEFAULT '',
  `host` int(11) DEFAULT NULL,
  `directions` text,
  `league` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `dropoff` varchar(127) DEFAULT NULL,
  `approved` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=480 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `strike`
--

DROP TABLE IF EXISTS `strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `judge` int(11) NOT NULL DEFAULT '0',
  `type` varchar(63) NOT NULL DEFAULT '',
  `timeslot` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `comp` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `bin` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `strike` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=18897 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `first` varchar(63) NOT NULL DEFAULT '',
  `last` varchar(63) NOT NULL DEFAULT '',
  `chapter` int(11) DEFAULT NULL,
  `grad_year` int(11) NOT NULL DEFAULT '0',
  `novice` tinyint(1) DEFAULT '0',
  `retired` tinyint(1) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `phonetic` varchar(127) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `student` (`id`),
  KEY `id` (`id`),
  KEY `student_chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=89900 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student_result`
--

DROP TABLE IF EXISTS `student_result`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student_result` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `results_bar` varchar(127) DEFAULT NULL,
  `bid` tinyint(4) DEFAULT NULL,
  `rank` int(11) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `comp` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `student_result` (`student`)
) ENGINE=InnoDB AUTO_INCREMENT=129623 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep`
--

DROP TABLE IF EXISTS `sweep`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `place` int(11) DEFAULT NULL,
  `value` float DEFAULT NULL,
  `prelim_cume` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2949 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `team_member`
--

DROP TABLE IF EXISTS `team_member`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `team_member` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `comp` int(11) NOT NULL DEFAULT '0',
  `student` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3531 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak`
--

DROP TABLE IF EXISTS `tiebreak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tiebreaker` varchar(15) NOT NULL DEFAULT 'cumulative',
  `count` varchar(15) NOT NULL DEFAULT '0',
  `multiplier` int(11) NOT NULL DEFAULT '1',
  `priority` int(11) NOT NULL DEFAULT '0',
  `method` int(11) NOT NULL DEFAULT '0',
  `covers` varchar(15) NOT NULL DEFAULT 'prelim',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `round` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `type` int(11) DEFAULT NULL,
  `tb_set` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5701 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak_set`
--

DROP TABLE IF EXISTS `tiebreak_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak_set` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `tournament` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `timeslot`
--

DROP TABLE IF EXISTS `timeslot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `timeslot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5695 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tournament`
--

DROP TABLE IF EXISTS `tournament`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tournament` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `director` int(11) DEFAULT NULL,
  `flighted` tinyint(1) DEFAULT NULL,
  `reg_start` datetime DEFAULT NULL,
  `reg_end` datetime DEFAULT NULL,
  `invite` text,
  `judge_policy` text,
  `invitename` varchar(127) DEFAULT NULL,
  `inviteurl` varchar(127) DEFAULT NULL,
  `results` tinyint(1) DEFAULT NULL,
  `league` int(11) DEFAULT NULL,
  `method` int(11) DEFAULT NULL,
  `bill_packet` varchar(127) DEFAULT NULL,
  `disclaimer` text,
  `invoice_message` text,
  `ballot_message` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `chair_ballot_message` text,
  `drop_deadline` datetime DEFAULT NULL,
  `fine_deadline` datetime DEFAULT NULL,
  `judge_deadline` datetime DEFAULT NULL,
  `hidden` tinyint(1) DEFAULT NULL,
  `freeze_deadline` datetime DEFAULT NULL,
  `web_message` text,
  `approved` tinyint(1) DEFAULT NULL,
  `vcorner` float DEFAULT NULL,
  `hcorner` float DEFAULT NULL,
  `vlabel` float DEFAULT NULL,
  `hlabel` float DEFAULT NULL,
  `webname` varchar(63) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1214 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tournament_site`
--

DROP TABLE IF EXISTS `tournament_site`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tournament_site` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tournament` int(11) NOT NULL DEFAULT '0',
  `site` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1747 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `uber`
--

DROP TABLE IF EXISTS `uber`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `uber` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `first` varchar(128) DEFAULT NULL,
  `last` varchar(128) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `started` int(11) DEFAULT NULL,
  `retired` tinyint(1) DEFAULT NULL,
  `notes` varchar(254) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `created` datetime DEFAULT NULL,
  `last_judged` datetime DEFAULT NULL,
  `cell` varchar(15) DEFAULT NULL,
  `paradigm` text,
  `account` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14546 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2011-10-11  9:42:59
INSERT INTO `account` VALUES (1,'admin@tabroom.com','admin','$1$paYkBo5N$DzMFtxzOonfFBFSx5OKZU1','god','Admin','User','800-555-1234',NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,NULL,NULL,1,1,'0000-00-00 00:00:00');

INSERT INTO `league` VALUES (1,'Default League',NULL,NULL,'DEF',1,1,NULL,NULL,NULL,NULL,'America/New_York',NULL,1,NULL,NULL,NULL,NULL,1,NULL,NULL,1,NULL,NULL,'0000-00-00 00:00:00');
