-- MySQL dump 10.13  Distrib 5.1.60, for apple-darwin11.2.0 (i386)
--
-- Host: localhost    Database: itab
-- ------------------------------------------------------
-- Server version	5.1.60-log

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
  `first` varchar(63) DEFAULT NULL,
  `last` varchar(63) DEFAULT NULL,
  `phone` varchar(27) DEFAULT NULL,
  `provider` varchar(63) DEFAULT NULL,
  `street` varchar(127) DEFAULT NULL,
  `city` varchar(63) DEFAULT NULL,
  `state` varchar(11) DEFAULT NULL,
  `zip` int(11) DEFAULT NULL,
  `tz` varchar(63) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `passhash` varchar(127) NOT NULL DEFAULT '',
  `paradigm` text,
  `site_admin` tinyint(1) DEFAULT NULL,
  `no_email` tinyint(1) DEFAULT NULL,
  `change_pass_key` varchar(255) DEFAULT NULL,
  `multiple` int(11) DEFAULT NULL,
  `started_judging` date DEFAULT NULL,
  `password_timestamp` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25266 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `account_ignore`   THIS IS TO GET RID OF A TOURNAMENT FROM THE HOME SCREEN.
--

DROP TABLE IF EXISTS `account_ignore`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account_ignore` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1315 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ballot`       THIS IS THE RECORD FOR W/L, RESULTS, ETC.  1 FOR EACH JUDGE/ENTRY COMBO
--

DROP TABLE IF EXISTS `ballot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ballot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT '0',
  `chair` tinyint(1) NOT NULL DEFAULT '0',
  `panel` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `bye` tinyint(1) DEFAULT NULL,
  `side` enum('A','N') DEFAULT NULL,
  `tv` tinyint(1) DEFAULT NULL,
  `win` tinyint(1) DEFAULT NULL,
  `rank` int(11) DEFAULT NULL,
  `points` int(11) DEFAULT NULL,
  `noshow` tinyint(1) NOT NULL DEFAULT '0',
  `audit` tinyint(1) NOT NULL DEFAULT '0',
  `topic` int(11) DEFAULT NULL,
  `speakerorder` int(11) NOT NULL DEFAULT '0',
  `speechnumber` int(11) DEFAULT NULL,
  `collected_on` datetime DEFAULT NULL,
  `collected_by` int(11) NOT NULL DEFAULT '0',
  `countmenot` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `comp` (`entry`),
  KEY `panel` (`panel`),
  KEY `judge` (`judge`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1256949 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ballot_speaks`        THIS IS TO KEEP THE SPEAKER POINTS FOR EACH INDIVIDUAL STUDENT.
--

DROP TABLE IF EXISTS `ballot_speaks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ballot_speaks` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ballot` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `points` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `student` (`student`),
  KEY `ballot` (`ballot`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter`          THE MASTER SCHOOL RECORD. ("school" refers to an entry w/in a tourney)
--

DROP TABLE IF EXISTS `chapter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL DEFAULT '',
  `state` char(4) DEFAULT NULL,
  `coaches` varchar(255) DEFAULT NULL,           
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5913 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_admin`            ACCOUNT ACCESS to a given CHAPTER.
--

DROP TABLE IF EXISTS `chapter_admin`;                   
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6789 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_circuit`          CHAPTER joined to a CIRCUIT (League)
--

DROP TABLE IF EXISTS `chapter_circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) DEFAULT NULL,
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
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=9225 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_judge`            ACCOUNT/JUDGE record that a CHAPTER pulls from
--

DROP TABLE IF EXISTS `chapter_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `notes` varchar(254) DEFAULT NULL,
  `created` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=19340 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit`              LEAGUE or GROUPING OF TOURNAMENTS
--

DROP TABLE IF EXISTS `circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `region_based` int(11) DEFAULT NULL,
  `diocese_based` tinyint(1) DEFAULT NULL,
  `timezone` varchar(67) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_admin`        ACCOUNT WITH ADMIN POWERS OVER A CIRCUIT
--

DROP TABLE IF EXISTS `circuit_admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) DEFAULT NULL,
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=244 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_dues`         CIRCUIT/LEAGUES THAT CHARGE MEMBERSHIP DUES; THIS IS PAYMENT
--

DROP TABLE IF EXISTS `circuit_dues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_dues` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `amount` float DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `paid_on` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=617 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_membership`      SETTINGS/DUES FOR HOW A CHAPTER SHOULD JOIN A CIRCUIT
--

DROP TABLE IF EXISTS `circuit_membership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_membership` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `approval` tinyint(1) DEFAULT NULL,
  `dues` float DEFAULT NULL,
  `dues_start` datetime DEFAULT NULL,
  `dues_expire` datetime DEFAULT NULL,
  `blurb` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_setting`         SETTINGS TABLE FOR CIRCUIT GENERAL/EXTENSIBLE SETTINGS
--

DROP TABLE IF EXISTS `circuit_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) NOT NULL DEFAULT '0',
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` text,
  `value_date` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=189 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession`       FOOD, TRAVEL TIX, ETC WITHIN A TOURNEY
--

DROP TABLE IF EXISTS `concession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `price` float DEFAULT NULL,
  `description` text,
  `tourn` int(11) DEFAULT NULL,
  `deadline` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=276 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession_purchase`      JOIN A SCHOOL TO A CONCESSION IT WANTS TO BUY
--

DROP TABLE IF EXISTS `concession_purchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession_purchase` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `school` int(11) NOT NULL,
  `concession` int(11) DEFAULT NULL,
  `quantity` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5686 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `email`                    RECORD OF EMAILS SENT TO CIRCUIT/TOURNEY MEMBERS
--

DROP TABLE IF EXISTS `email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `email` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `sender` int(11) DEFAULT NULL,
  `subject` varchar(127) DEFAULT NULL,
  `content` text,
  `sent_on` datetime DEFAULT NULL,
  `sent_to` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2379 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry`            AN ENTRY/DEBATER IN THE TOURNAMENT
--

DROP TABLE IF EXISTS `entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) NOT NULL DEFAULT '0',
  `event` int(11) NOT NULL DEFAULT '0',
  `code` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `seed` varchar(15) DEFAULT NULL,
  `dropped` tinyint(1) NOT NULL DEFAULT '0',
  `waitlist` tinyint(1) NOT NULL DEFAULT '0',
  `dq` tinyint(1) NOT NULL DEFAULT '0',
  `bid` tinyint(1) NOT NULL DEFAULT '0',
  `reg_time` datetime DEFAULT NULL,
  `drop_time` datetime DEFAULT NULL,
  `title` varchar(127) DEFAULT NULL,
  `ada` int(11) DEFAULT NULL,
  `notes` varchar(128) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`,`school`),
  KEY `tournament_2` (`tourn`,`school`,`event`),
  KEY `code` (`code`),
  KEY `school` (`school`),
  KEY `id` (`id`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=331371 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry_qualifier`      SOME OF MY TOURNAMENTS REQUIRE YOU TO ENTER WHERE ENTRIES QUALIFIED
--

DROP TABLE IF EXISTS `entry_qualifier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry_qualifier` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `name` varchar(63) DEFAULT NULL,
  `result` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=15009 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry_student`        JOINS A STUDENT TO AN ENTRY (MANY TO MANY)
--

DROP TABLE IF EXISTS `entry_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry_student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `entry` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=294211 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event`                EVENT/DIVISION OFFERING
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) NOT NULL DEFAULT '',
  `abbr` varchar(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `type` varchar(15) NOT NULL DEFAULT 'debate',
  `fee` float DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,
  `event_double` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=16659 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event_double`     THESE DEFINE LIMITS ON HOW/WHO CAN DOUBLE ENTRY.  PROB NOT USEFUL TO YOU.
--

DROP TABLE IF EXISTS `event_double`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_double` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `tourn` int(11) DEFAULT NULL,
  `setting` varchar(15) DEFAULT NULL,
  `max` int(11) DEFAULT NULL,
  `min` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4696 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event_setting`            SETTINGS TABLE FOR EVENTS
--

DROP TABLE IF EXISTS `event_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `event` int(11) NOT NULL DEFAULT '0',
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `text` text,
  `value_date` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=33363 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `file`                 FILES THAT HAVE BEEN UPLOADED TO THE WEBSITE.
--

DROP TABLE IF EXISTS `file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `file` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL,
  `label` varchar(127) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `uploaded` datetime DEFAULT NULL,
  `posting` tinyint(1) DEFAULT NULL,
  `result` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2263 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follow_account`       SETS UP ABILITY TO ALWAYS FOLLOW A GIVEN ACCOUNT, JUDGE OR STUDENT BOTH
--

DROP TABLE IF EXISTS `follow_account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follow_account` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `follower` int(11) DEFAULT NULL,
  `parent` tinyint(1) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `account` (`account`),
  KEY `follower` (`follower`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follow_entry`         FOLLOWS A PARTICULAR ENTRY AT A TOURNAMENT
--

DROP TABLE IF EXISTS `follow_entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follow_entry` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `comp` int(11) DEFAULT NULL,
  `cell` varchar(16) DEFAULT NULL,
  `email` varchar(127) DEFAULT NULL,
  `domain` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follow_judge`         FOLLOWS A PARTICULAR JUDGE AT A TOURNAMENT
--

DROP TABLE IF EXISTS `follow_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follow_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) DEFAULT NULL,
  `cell` varchar(16) DEFAULT NULL,
  `email` varchar(127) DEFAULT NULL,
  `domain` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing_slots`        MAKES HOUSING SLOTS AVAILABLE.  PROB NOT USEFUL TO YOU.
--

DROP TABLE IF EXISTS `housing_slots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `housing_slots` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `night` date DEFAULT NULL,
  `slots` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing_student`      SIGNS A STUDENT UP FOR HOUSING; AGAIN PROB IGNORE.
--

DROP TABLE IF EXISTS `housing_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `housing_student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `type` varchar(7) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `waitlist` tinyint(1) NOT NULL DEFAULT '0',
  `night` date DEFAULT NULL,
  `requested` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13786 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge`                A JUDGE WITHIN A TOURNAMENT.  MANY JUDGES CAN BE THE SAME PERSON;
--                                                  THIS REFLECTS A PARTICULAR OBLIGATION FULFILLED.  

DROP TABLE IF EXISTS `judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `school` int(11) NOT NULL DEFAULT '0',
  `code` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `first` varchar(63) NOT NULL DEFAULT '',              -- SHEER LAZYNESS SO I DON'T HAVE TO KEEP GOING TO ACCOUNT TO GET NAMES
  `last` varchar(63) NOT NULL DEFAULT '',
  `active` int(11) NOT NULL DEFAULT '0',
  `judge_group` int(11) DEFAULT NULL,
  `covers` int(11) DEFAULT NULL,                        -- THE JUDGE GROUP THE JUDGE COVERS OBLIGATIONS IN, IF DIFFERENT
  `obligation` int(11) DEFAULT NULL,                    -- HOW MANY ROUNDS THIS JUDGE IS ON THE HOOK FOR
  `hired` int(11) DEFAULT NULL,                         -- IS THIS JUDGE A TOURNAMENT HIRE?
  `chapter_judge` int(11) DEFAULT NULL,                 -- MAY NOT HAVE IF A HIRED JUDGE
  `special` text,                                       -- NOTES THE TOURNAMENT WRITES FOR A SPECIAL ASSIGNMENT (ie TAB).
  `notes` text,                                         -- NOTES THE COACH WRITES TO US
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `id` (`id`),
  KEY `judge_group` (`judge_group`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=102852 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_group`  -- GROUP OF JUDGES, CAN BE MULTIPLE EVENTS (ie Policy covers both JV & Varsity)
--

DROP TABLE IF EXISTS `judge_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge_per` float DEFAULT NULL,           -- JUDGE BURDEN (2 == "1 judge per 2 debaters");
  `deadline` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4187 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_group_setting`
--

DROP TABLE IF EXISTS `judge_group_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_group_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_group` int(11) NOT NULL DEFAULT '0',
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_date` datetime DEFAULT NULL,
  `value_text` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `judge_group` (`judge_group`)
) ENGINE=InnoDB AUTO_INCREMENT=23190 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_hire`               A SCHOOL REQUEST TO HIRE A JUDGE ON ENTRY.
--

DROP TABLE IF EXISTS `judge_hire`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_hire` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) DEFAULT NULL,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `school` int(11) NOT NULL DEFAULT '0',
  `covers` int(11) DEFAULT NULL,                        -- HOW MANY ENTRIES THE REQUEST COVERS
  `judge_group` int(11) NOT NULL DEFAULT '0',
  `accepted` int(11) DEFAULT NULL,
  `request_made` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7006183 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_setting`
--

DROP TABLE IF EXISTS `judge_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT '0',
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_date` datetime DEFAULT NULL,
  `value_text` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `panel`            -- AN INDIVIDUAL DEBATE WITHIN A ROUND.  PANEL IS A SPEECH TERM. 
--

DROP TABLE IF EXISTS `panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `panel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `round` int(11) DEFAULT NULL,
  `room` int(11) NOT NULL DEFAULT '0',
  `letter` char(2) NOT NULL DEFAULT '',
  `type` varchar(63) NOT NULL DEFAULT '',
  `flight` char(1) DEFAULT NULL,
  `bye` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `room` (`room`),
  KEY `event` (`event`),
  KEY `round` (`round`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=129635 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pool`         A SUBGROUP OF JUDGES FOR A GIVEN ROUND. 
--

DROP TABLE IF EXISTS `pool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_group` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT '0',          -- tie a pool to a given location's rounds.
  `name` varchar(63) DEFAULT NULL,
  `standby` tinyint(1) DEFAULT NULL,            -- DON'T USE THEM, they are pre-assigned to be hot spares.
  `publish` tinyint(1) DEFAULT NULL,            -- PUBLISH THE MEMBERS OF THIS POOL ON THE WEB ("judges called for AM octos")
  `registrant` tinyint(1) DEFAULT NULL,         --- THE PEOPLE REGISTERING MUST SIGN JUDGES UP FOR THIS POOL
  `burden` int(11) DEFAULT NULL,                -- when registering online, this %age of overall burden must be signed up.
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1111 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pool_judge`           JUDGES CONNECTED TO POOLS
--

DROP TABLE IF EXISTS `pool_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pool_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT '0',
  `pool` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `pool` (`pool`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=33546 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating`               FOR PREFS & COACH RATINGS.  
--

DROP TABLE IF EXISTS `rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL,
  `judge` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,                 
  `type` enum('school','entry','coach') DEFAULT NULL,       -- RATING IS BY WHOLE SCHOOL, ENTRY or coach's own rating
  `tier` int(11) DEFAULT NULL,                  -- WHEN USING 6/9 CATEGORY SYSTEMS
  `subset` int(11) DEFAULT NULL,                -- IE THING, CAN RATE EACH JUDGE DIFFERENTLY FOR DIFF EVENTS
  `ordinal` int(11) DEFAULT NULL,               -- WHEN USING ORDINALS
  `entered` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=561350 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating_subset`        -- SUBSETS FOR IE ENTRY (DIFFERENT RATINGS DIFFERENT EVENTS)
--

DROP TABLE IF EXISTS `rating_subset`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating_subset` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating_tier`          -- TIERS FOR 6/9 tier systems
--

DROP TABLE IF EXISTS `rating_tier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating_tier` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_group` int(11) DEFAULT NULL,
  `name` varchar(12) DEFAULT NULL,
  `type` enum('mpj','coach') DEFAULT NULL,
  `strike` tinyint(1) DEFAULT NULL,
  `conflict` tinyint(1) DEFAULT NULL,
  `min` float DEFAULT NULL,
  `max` float DEFAULT NULL,
  `description` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9501 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `region`               -- SOME IE TOURNEYS USE REGIONS AND REGISTER BY THEM.  REGION ADMINS
--                                                  -- HAVE POWERS OVER THEIR REGION MEMBERS.  DUNNO IF THIS IS USEFUL FOR YOU.

DROP TABLE IF EXISTS `region`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `region` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `code` varchar(12) DEFAULT NULL,
  `diocese` tinyint(1) DEFAULT NULL,
  `quota` int(11) DEFAULT NULL,
  `arch` tinyint(1) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `cooke_pts` int(11) DEFAULT NULL,
  `sweeps` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `region_admin`         -- DIRECTOR OF A REGION.
--

DROP TABLE IF EXISTS `region_admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `region_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `region` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=149 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `region_fine`          -- WHEN A REGION IS FINED; USED ONLY BY NCFL NATIONALS.
--

DROP TABLE IF EXISTS `region_fine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `region_fine` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `region` int(11) DEFAULT NULL,
  `amount` float DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `levied_on` datetime DEFAULT NULL,
  `levied_by` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `site` int(11) DEFAULT NULL,
  `building` varchar(127) NOT NULL DEFAULT '',
  `name` varchar(127) NOT NULL DEFAULT '',
  `quality` int(11) DEFAULT NULL,
  `capacity` int(11) DEFAULT NULL,
  `inactive` tinyint(1) DEFAULT NULL,
  `notes` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `site` (`site`)
) ENGINE=InnoDB AUTO_INCREMENT=17179 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room_pool`            -- CREATE A POOL OF ROOMS TO DRAW FROM FOR A GIVEN EVENT
--

DROP TABLE IF EXISTS `room_pool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room_pool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `room` int(11) DEFAULT NULL,
  `reserved` tinyint(1) DEFAULT NULL,               -- USE THIS ROOM EXCLUSIVELY FOR THIS EVENT; OTHERWISE ONLY PREFER IT
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=42335 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room_strike`          -- BLOCK A ROOM FROM A GIVEN EVENT OR A TIME PERIOD.
--

DROP TABLE IF EXISTS `room_strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room_strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `room` int(11) NOT NULL DEFAULT '0',
  `type` varchar(67) DEFAULT NULL,                  -- "ENTRY, JUDGE, TIME, EVENT"
  `event` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `start` datetime,
  `end`   datetime,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `room` (`room`)
) ENGINE=InnoDB AUTO_INCREMENT=1346 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `round`            -- A ROUND IS A WHOLE ROUND, NOT AN INDIVIDUAL DEBATE.
--

DROP TABLE IF EXISTS `round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,
  `name` int(11) DEFAULT NULL,                  -- NAME IS STRICTLY NUMERICAL; LABEL IS USER-ENTERED ('semis, finals').
  `label` varchar(15) DEFAULT NULL,
  `tb_set` int(11) DEFAULT NULL,                -- CAN SET TIEBREAKERS FOR A GIVEN ROUND; OTHERWISE IT ASKS.
  `pool` int(11) DEFAULT NULL,                  -- USE ONLY JUDGES FROM THIS POOL, OTHERWISE PULLS FROM ALL
  `published` tinyint(1) DEFAULT NULL,          -- PUBLISH SCHEM OF THIS ROUND ONLINE.
  `listed` tinyint(1) DEFAULT NULL,             -- PUBLISH ALPHA LIST OF STUDENTS IN THIS ROUND ONLINE
  `created` datetime DEFAULT NULL,              -- ROUND PANELED
  `completed` datetime DEFAULT NULL,            -- LAST BALLOT ENTERED
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `timeslot` (`timeslot`),
  KEY `pool` (`pool`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=47285 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `school`               -- AN ENTRY OF A CHAPTER INTO A TOURNAMENT
--

DROP TABLE IF EXISTS `school`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `code` varchar(15) DEFAULT NULL,
  `paid` float DEFAULT NULL,                        -- HOW MUCH MONEY THEY'VE PAID.
  `registered` tinyint(1) NOT NULL DEFAULT '0',     -- DENOTES THAT THEY'VE CHECKED IN ONSITE
  `registered_on` datetime DEFAULT NULL,           
  `contact` int(11) DEFAULT NULL,                   --- TIES TO AN ACCOUNT WHO THEN GETS UPDATES
  `contact_number` varchar(63) DEFAULT NULL,        -- IF THE ADULT CONTACT DOES NOT HAVE AN ACCOUNT
  `contact_name` varchar(127) DEFAULT NULL,
  `entered_on` datetime DEFAULT NULL,               -- ONLINE ENTRY
  `noprefs` tinyint(1) DEFAULT NULL,                -- THIS SCHOOL HAS LOST THEIR PREFS FOR SOME TRANSGRESSION.  FIENDS!
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=31381 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `school_fine`          -- A SEMI-ARBITRARY FINE ADDED TO THEIR INVOICE FOR VARIOUS REASON.
--

DROP TABLE IF EXISTS `school_fine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school_fine` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `school` int(11) DEFAULT NULL,
  `amount` float DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `levied_on` datetime DEFAULT NULL,
  `levied_by` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5065326 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `session`          -- USED BY THE WEB END TO TRACK WHO IS LOGGED ON.  IGNORE.
--

DROP TABLE IF EXISTS `session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `session` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) NOT NULL DEFAULT '0',
  `userkey` varchar(127) DEFAULT NULL,
  `authkey` varchar(63) NOT NULL DEFAULT '',
  `ip` varchar(63) DEFAULT NULL,
  `limited` tinyint(1) DEFAULT NULL,
  `entry_only` tinyint(1) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `timestamp` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=177942 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `site`                 LOCATION(S).   CONTAINS THE ROOMS.
--

DROP TABLE IF EXISTS `site`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `site` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL DEFAULT '',
  `host` int(11) DEFAULT NULL,
  `directions` text,
  `circuit` int(11) DEFAULT NULL,
  `dropoff` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=620 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `strike`           THIS IS FOR STRIKES WITHOUT PREFS, OR INTERNAL STRIKES/CONFLICTS.
--

DROP TABLE IF EXISTS `strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) NOT NULL DEFAULT '0',
  `type` varchar(63) NOT NULL DEFAULT '',           "event, elims_only, region, school, entry"
  `event` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,                    -- beginning of a time strike
  `end` datetime DEFAULT NULL,                      -- end of a time strike
  `strike_time` int(11) DEFAULT NULL,               -- judge cannot judge during the time marked in strike_time (and pays for it)
  `registrant` tinyint(4) DEFAULT NULL,             -- true if this was entered by the school, not the tourn staff. 
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=21602 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `strike_time`      -- this is for a judge who can't be there the whole time, some tourns get 
--                                                  sat-only and sun-only judges.  this charges them if theyre short either day.

DROP TABLE IF EXISTS `strike_time`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `strike_time` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_group` int(11) NOT NULL,
  `name` varchar(63) DEFAULT NULL,
  `fine` int(11) DEFAULT NULL,                  -- the charge
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=388 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student`          STUDENT RECORD PARTICULAR TO A CHAPTER.
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `first` varchar(63) NOT NULL DEFAULT '',      
  `last` varchar(63) NOT NULL DEFAULT '',
  `grad_year` int(11) NOT NULL DEFAULT '0',     -- PAST GRADUATES AUTOMATICALLY STOP APPEARING ON ROSTER.
  `novice` tinyint(1) DEFAULT '0',              -- GETS STRIPPED OUT EVERY YEAR ON JULY 1st
  `retired` tinyint(1) DEFAULT NULL,            -- PREVENTS THEM FROM SHOWING UP ON THE ROSTER
  `gender` char(1) DEFAULT NULL,
  `phonetic` varchar(127) DEFAULT NULL,         -- AWARD CEREMONY LIFESAVER.
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`)
  KEY `account` (`account`)
) ENGINE=InnoDB AUTO_INCREMENT=101324 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak`         -- METHOD USED TO BREAK TIES.
--

DROP TABLE IF EXISTS `tiebreak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tb_set` int(11) DEFAULT NULL,                        -- COLLECTION OF TIEBREAKERS FOR A GIVEN ROUND/DIVISION
  `name` varchar(15) NOT NULL DEFAULT 'cumulative',     -- TYPE.  We'll have to come up with a complete list sometime.
  `count` varchar(15) NOT NULL DEFAULT '0',             -- "ALL", "PRELIMS", "ELIMS", "Last Elim"< "Finals".  More for IEs
  `multiplier` int(11) NOT NULL DEFAULT '1',            -- Multiply this tiebreak by this number if adding together.  for IEs
  `priority` int(11) NOT NULL DEFAULT '0',              -- Which priority is this tiebreaker.  2 of same priority are added.
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tb_set` (`tb_set`)
) ENGINE=InnoDB AUTO_INCREMENT=6898 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak_set`         -- COLLECTION FOR A GIVEN ROUND/DIVISION.
--

DROP TABLE IF EXISTS `tiebreak_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak_set` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4843 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `timeslot`             -- BLOCK OF TIME
--

DROP TABLE IF EXISTS `timeslot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `timeslot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6788 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn`            -- TOURNAMENT
--

DROP TABLE IF EXISTS `tourn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `reg_start` datetime DEFAULT NULL,        -- ONLINE REGISTRATION OPENS
  `reg_end` datetime DEFAULT NULL,          -- ONLINE REGISTRATION CLOSES
  `invite` text,                            -- URL OF THE INVITATION (UPLOADED FILE OR WEBSITE)
  `hidden` tinyint(1) DEFAULT NULL,         -- "HIDDEN" TOURNS won't show up on the web; so people can play/test
  `approved` tinyint(1) DEFAULT NULL,       -- APPROVED by a circuit administrator
  `webname` varchar(63) DEFAULT NULL,       -- For the URL/website thing I'm working on
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1463 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_admin`      -- I CAN HAZ TOURNAMENT?
--

DROP TABLE IF EXISTS `tourn_admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `contact` tinyint(1) NOT NULL DEFAULT '0',        -- LIST AS A PUBLIC CONTACT ON THE WEBSITE
  `entry_only` tinyint(4) DEFAULT NULL,             -- I CAN ONLY ENTER RESULTS, NOT MUCK AROUND WITH ANYTHING ELSE
  `no_setup` tinyint(1) DEFAULT NULL,                -- I CAN DO NEARLY EVERYTHING EXCEPT CHANGE SETTINGS
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4727 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_change`     -- LOGS OF CHANGES MADE BY IDIOTS TO MY REGISTRATION.  THIS ENABLES
--                                                  AND FUELS WONDERFUL ACTS OF VINDICTIVENESS.

DROP TABLE IF EXISTS `tourn_change`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_change` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `type` varchar(63) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `old_panel` int(11) DEFAULT NULL,
  `new_panel` int(11) DEFAULT NULL,
  `fine` int(11) DEFAULT NULL,
  `text` varchar(255) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `panel` (`new_panel`)
) ENGINE=InnoDB AUTO_INCREMENT=257537 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_circuit`        -- JOIN A TOURNAMENT TO A CIRCUIT.  THIS IS SO IT CAN SHOW IN MULTIPLE.
--

DROP TABLE IF EXISTS `tourn_circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `circuit` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1206 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_fee`        -- STANDING SCHOOL FEE CHARGED TO EVERYONE WHO REGSITERS (OR DURING A WINDOW)
--

DROP TABLE IF EXISTS `tourn_fee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_fee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `amount` float DEFAULT NULL,
  `start` datetime DEFAULT NULL,            -- DONT CHARGE BEFORE THIS TIME
  `end` datetime DEFAULT NULL,              -- DONT CHARGE AFTER THIS TIME
  `reason` varchar(127) DEFAULT NULL,       -- EXPLAIN THE FEE IF YOU DEIGN TO
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_setting`        
--

DROP TABLE IF EXISTS `tourn_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_date` datetime DEFAULT NULL,
  `value_text` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_site`       JOIN A TOURNAMENT TO A SITE/SET OF ROOMS
--

DROP TABLE IF EXISTS `tourn_site`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_site` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `site` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2125 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-03-13 13:39:25
