-- MySQL dump 10.13  Distrib 5.1.58, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: itab
-- ------------------------------------------------------
-- Server version	5.1.58-1ubuntu1-log

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
  `noemail` tinyint(1) DEFAULT NULL,
  `phone` varchar(27) DEFAULT NULL,
  `is_cell` tinyint(1) DEFAULT NULL,
  `passhash` varchar(127) NOT NULL DEFAULT '',
  `first` varchar(63) DEFAULT NULL,
  `last` varchar(63) DEFAULT NULL,
  `street` varchar(127) DEFAULT NULL,
  `city` varchar(63) DEFAULT NULL,
  `state` varchar(11) DEFAULT NULL,
  `zip` int(11) DEFAULT NULL,
  `site_admin` tinyint(1) DEFAULT NULL,
  `change_pass_key` varchar(255) DEFAULT NULL,
  `multiple` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_account`
--

DROP TABLE IF EXISTS `tourn_account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_account` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `contact` tinyint(1) NOT NULL DEFAULT '0',
  `entry` tinyint(1) DEFAULT NULL,
  `nosetup` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ballot`
--

DROP TABLE IF EXISTS `ballot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ballot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `side` char(2) DEFAULT NULL,
  `audit` tinyint(1) NOT NULL DEFAULT '0',
  `panel` int(11) DEFAULT NULL,
  `judge` int(11) NOT NULL DEFAULT '0',
  `entry` int(11) DEFAULT NULL,
  `win` tinyint(1) DEFAULT NULL,
  `rank` int(11) NOT NULL DEFAULT '0',
  `points` int(11) NOT NULL DEFAULT '0',
  `speakerorder` int(11) NOT NULL DEFAULT '0',
  `tv` tinyint(1) DEFAULT NULL,
  `noshow` tinyint(1) NOT NULL DEFAULT '0',
  `bye` tinyint(1) DEFAULT NULL,
  `seed` int(11) DEFAULT NULL,
  `rankinround` int(11) DEFAULT NULL,
  `chair` tinyint(1) NOT NULL DEFAULT '0',
  `speechnumber` int(11) DEFAULT NULL,
  `topic` int(11) DEFAULT NULL,
  `collected` datetime DEFAULT NULL,
  `collected_by` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`),
  KEY `panel` (`panel`),
  KEY `judge` (`judge`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `ballot_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ballot_student` (
	`id` int(11) NOT NULL AUTO_INCREMENT,
	`ballot` int(11) DEFAULT NULL,
	`student` int(11) DEFAULT NULL,
	`points` int(11) DEFAULT NULL,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `student` (`student`),
  	KEY `ballot` (`ballot`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `strike_time`
--

DROP TABLE IF EXISTS `strike_time`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `strike_time` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge_group` int(11) NOT NULL,
  `fine` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `name` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `tourn` int(11) NOT NULL DEFAULT '0',
  `panel` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `fine` int(11) DEFAULT NULL,
  `moved_from` int(11) DEFAULT NULL,
  `regline` varchar(255) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `state` char(4) DEFAULT NULL,
  `coaches` varchar(255) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_account`
--

DROP TABLE IF EXISTS `chapter_account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_account` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_circuit`
--

DROP TABLE IF EXISTS `chapter_circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) NOT NULL DEFAULT '0',
  `full_member` tinyint(1) NOT NULL DEFAULT '0',
  `code` varchar(12) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `paid` tinyint(1) DEFAULT NULL,
  `membership` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `double_entry`
--

DROP TABLE IF EXISTS `double_entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `double_entry` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `name` varchar(63) NOT NULL DEFAULT '',
  `setting` int(11) DEFAULT '0',
  `max` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry`
--

DROP TABLE IF EXISTS `entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `school` int(11) NOT NULL DEFAULT '0',
  `event` int(11) NOT NULL DEFAULT '0',
  `code` varchar(63) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `title` varchar(127) DEFAULT NULL,
  `dropped` tinyint(1) NOT NULL DEFAULT '0',
  `seed` int(11) DEFAULT NULL,
  `waitlist` tinyint(1) NOT NULL DEFAULT '0',
  `dq` tinyint(1) NOT NULL DEFAULT '0',
  `bid` tinyint(1) NOT NULL DEFAULT '0',
  `ada` int(11) DEFAULT NULL,
  `reg_time` datetime DEFAULT NULL,
  `drop_time` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tourn_school` (`tourn`,`school`),
  KEY `tourn_school_event` (`tourn`,`school`,`event`),
  KEY `code` (`code`),
  KEY `school` (`school`),
  KEY `id` (`id`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `qualifier`
--

DROP TABLE IF EXISTS `qualifier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `qualifier` (
  	`id` int(11) NOT NULL AUTO_INCREMENT,
	entry int default null,
	name varchar(63) default null,
	result varchar(127) default null,
	tourn int default null,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `entry` (`entry`),
  	KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `circuit` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `email`
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
  `text` text,
  `senton` datetime DEFAULT NULL,
  `sent_to` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `name` varchar(127) NOT NULL DEFAULT '',
  `abbr` varchar(11) DEFAULT NULL,
  `fee` float DEFAULT NULL,
  `type` varchar(15) NOT NULL DEFAULT '',
  `code` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,
  `double_entry` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `event_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_setting` (
 	`id` int(11) NOT NULL AUTO_INCREMENT,
	`event` int(11) NOT NULL DEFAULT '0',
	`key` varchar(15) NOT NULL DEFAULT '',
	`value` varchar(127) NOT NULL DEFAULT '',
	`text` text,
  	PRIMARY KEY (`id`),
  	KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `file`
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
  `results` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `tourn` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `levied` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `follower`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follower` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `follower` int(11) DEFAULT NULL,
  `parent` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `account` (`account`),
  KEY `follower` (`follower`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follow_comp`
--

DROP TABLE IF EXISTS `follow_comp`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follow_comp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `entry` int(11) DEFAULT NULL,
  `cell` varchar(16) DEFAULT NULL,
  `email` varchar(127) DEFAULT NULL,
  `domain` varchar(63) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `domain` varchar(63) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing`
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
  `night` date DEFAULT NULL,
  `waitlist` tinyint(1) NOT NULL DEFAULT '0',
  `requested` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing_slots`
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession`
--

DROP TABLE IF EXISTS `concession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `name` varchar(63) DEFAULT NULL,
  `price` float DEFAULT NULL,
  `description` text,
  `deadline` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge`
--

DROP TABLE IF EXISTS `judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge` (
  	`id` int(11) NOT NULL AUTO_INCREMENT,
  	`account` int(11) DEFAULT NULL,
  	`covers` int(11) DEFAULT NULL,
  	`code` int(11) DEFAULT NULL,
  	`school` int(11) NOT NULL DEFAULT '0',
  	`judge_group` int(11) DEFAULT NULL,
  	`obligation` int(11) DEFAULT NULL,
  	`first` varchar(63) NOT NULL DEFAULT '',
  	`last` varchar(63) NOT NULL DEFAULT '',
  	`active` int(11) NOT NULL DEFAULT '0',
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
	PRIMARY KEY (`id`),
  	KEY `judge_group` (`judge_group`),
  	KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `judge_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_setting` (
 	`id` int(11) NOT NULL AUTO_INCREMENT,
	`judge` int(11) NOT NULL DEFAULT '0',
	`key` varchar(15) NOT NULL DEFAULT '',
	`value` varchar(127) NOT NULL DEFAULT '',
	`text` text,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `abbr` varchar(15) DEFAULT NULL,
  `judge_per` float DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `deadline` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `judge_group_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_group_setting` (
 	`id` int(11) NOT NULL AUTO_INCREMENT,
	`judge_group` int(11) NOT NULL DEFAULT '0',
	`key` varchar(15) NOT NULL DEFAULT '',
	`value` varchar(127) NOT NULL DEFAULT '',
	`text` text,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `judge_group` (`judge_group`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_hire`
--

DROP TABLE IF EXISTS `judge_hire`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_hire` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `school` int(11) NOT NULL DEFAULT '0',
  `judge_group` int(11) NOT NULL DEFAULT '0',
  `covers` int(11) DEFAULT NULL,
  `accepted` int(11) DEFAULT NULL,
  `request_made` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
   PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit`
--

DROP TABLE IF EXISTS `circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL DEFAULT '',
  `region_based` int(11) DEFAULT NULL,
  `diocese_based` tinyint(1) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `circuit_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_setting` (
 	`id` int(11) NOT NULL AUTO_INCREMENT,
	`circuit` int(11) NOT NULL DEFAULT '0',
	`key` varchar(15) NOT NULL DEFAULT '',
	`value` varchar(127) NOT NULL DEFAULT '',
	`text` text,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_admin`
--

DROP TABLE IF EXISTS `circuit_admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) NOT NULL DEFAULT '0',
  `account` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `circuit` int(11) DEFAULT NULL,
  `approval` tinyint(1) DEFAULT NULL,
  `dues` float DEFAULT NULL,
  `dues_start` datetime DEFAULT NULL,
  `dues_expire` datetime DEFAULT NULL,
  `text` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `text` text,
  `author` int(11) DEFAULT NULL,
  `posted` datetime DEFAULT NULL,
  `file` varchar(127) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `link` int(11) DEFAULT NULL,
  `main_site` tinyint(1) DEFAULT NULL,
  `circuit` int(11) NOT NULL DEFAULT '0',
  `edited_by` int(11) NOT NULL DEFAULT '0',
  `sitewide` tinyint(1) DEFAULT NULL,
  `edited_on` datetime DEFAULT NULL,
  `pinned` tinyint(1) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `display_order` int(11) DEFAULT NULL,
  `special` varchar(15) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `no_interest`
--

DROP TABLE IF EXISTS `no_interest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `no_interest` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `panel`
--

DROP TABLE IF EXISTS `panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `panel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `letter` char(2) NOT NULL DEFAULT '',
  `round` int(11) DEFAULT NULL,
  `room` int(11) NOT NULL DEFAULT '0',
  `flight` char(1) DEFAULT NULL,
  `bye` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `room` (`room`),
  KEY `round` (`round`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `standby` tinyint(1) DEFAULT NULL,
  `publish` tinyint(1) DEFAULT NULL,
  `registrant` tinyint(1) DEFAULT NULL,
  `burden` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `type` varchar(128) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `pool` (`pool`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession_purchase`
--

DROP TABLE IF EXISTS `purchase`;
DROP TABLE IF EXISTS `concession_purchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession_purchase` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `concession` int(11) NOT NULL,
  `school` int(11) NOT NULL,
  `quantity` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating_tier`
--

DROP TABLE IF EXISTS `rating_tier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating_tier` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `name` varchar(12) DEFAULT NULL,
  `description` text,
  `type` enum('mpj','qual') DEFAULT NULL,
  `conflict` tinyint(1) DEFAULT NULL,
  `strike` tinyint(1) DEFAULT NULL,
  `min` float DEFAULT NULL,
  `max` float DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating_subset`
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating`
--

DROP TABLE IF EXISTS `rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL,
  `school` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `type` enum('school','comp','coach','ordinal') DEFAULT NULL,
  `rating_tier` int(11) DEFAULT NULL,
  `ordinal` int(11) DEFAULT NULL,
  `entered` datetime DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `subset` int(11) DEFAULT NULL,
  `name` char(3) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`),
  KEY `entry` (`entry`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `tourn` int(11) DEFAULT NULL,
  `code` varchar(12) DEFAULT NULL,
  `diocese` tinyint(1) DEFAULT NULL,
  `director` int(11) DEFAULT NULL,
  `quota` int(11) DEFAULT NULL,
  `arch` tinyint(1) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `cooke_pts` int(11) DEFAULT NULL,
  `sweeps` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `building` int(11) DEFAULT NULL,
  `name` varchar(127) NOT NULL DEFAULT '',
  `quality` int(11) DEFAULT NULL,
  `capacity` int(11) DEFAULT NULL,
  `inactive` tinyint(1) DEFAULT NULL,
  `notes` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `site` (`site`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room_pool`
--

DROP TABLE IF EXISTS `room_pool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room_pool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `room` int(11) DEFAULT NULL,
  `reserved` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room_strike`
--

DROP TABLE IF EXISTS `room_strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room_strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `room` int(11) NOT NULL DEFAULT '0',
  `tourn` int(11) NOT NULL DEFAULT '0',
  `type` varchar(67) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `special` varchar(127) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `room` (`room`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `round`
--

DROP TABLE IF EXISTS `round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `event` int(11) DEFAULT NULL,
  `timeslot` int(11) DEFAULT NULL,
  `name` int(11) DEFAULT NULL,
  `label` varchar(15) DEFAULT NULL,
  `type` varchar(15) DEFAULT NULL,
  `site` int(11) DEFAULT NULL,
  `pool` int(11) DEFAULT NULL,
  `published` tinyint(1) DEFAULT NULL,
  `listed` tinyint(1) DEFAULT NULL,
  `no_first_year` tinyint(1) DEFAULT NULL,
  `created` datetime DEFAULT NULL,
  `completed` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `timeslot` (`timeslot`),
  KEY `pool` (`pool`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `school`
--

DROP TABLE IF EXISTS `school`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `code` varchar(15) DEFAULT NULL,
  `paid` float DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `concession_paid` float DEFAULT NULL,
  `contact_name` varchar(127) DEFAULT NULL,
  `contact_number` varchar(63) DEFAULT NULL,
  `entered` datetime DEFAULT NULL,
  `registered` tinyint(1) NOT NULL DEFAULT '0',
  `registered_on` datetime DEFAULT NULL,
  `noprefs` tinyint(1) DEFAULT NULL,
  `disclaimed` tinyint(1) DEFAULT NULL,
  `self_register` tinyint(1) DEFAULT NULL,
  `sweeps_points` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `session`
--

DROP TABLE IF EXISTS `session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `session` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) NOT NULL DEFAULT '0',
  `authkey` varchar(63) NOT NULL DEFAULT '',
  `ip` varchar(63) DEFAULT NULL,
  `limited` tinyint(1) DEFAULT NULL,
  `entry` tinyint(1) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `userkey` varchar(127) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `site`
--

DROP TABLE IF EXISTS `site`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `site` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) DEFAULT NULL,
  `name` varchar(127) NOT NULL DEFAULT '',
  `host` int(11) DEFAULT NULL,
  `directions` text,
  `dropoff` varchar(127) DEFAULT NULL,
  `approved` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `strike`
--

DROP TABLE IF EXISTS `strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT '0',
  `judge` int(11) NOT NULL DEFAULT '0',
  `type` varchar(63) NOT NULL DEFAULT '',
  `timeslot` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `strike` tinyint(1) DEFAULT NULL,
  `strike_time` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) NOT NULL DEFAULT '0',
  `first` varchar(63) NOT NULL DEFAULT '',
  `last` varchar(63) NOT NULL DEFAULT '',
  `grad_year` int(11) NOT NULL DEFAULT '0',
  `novice` tinyint(1) DEFAULT '0',
  `retired` tinyint(1) DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `phonetic` varchar(127) DEFAULT NULL,
  `self_register` tinyint(1) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `student` (`id`),
  KEY `id` (`id`),
  KEY `student_chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `tourn` int(11) DEFAULT NULL,
  `entry` int(11) NOT NULL DEFAULT '0',
  `chapter` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `student` (`student`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry_student`
--

DROP TABLE IF EXISTS `entry_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry_student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `entry` int(11) NOT NULL DEFAULT '0',
  `student` int(11) NOT NULL DEFAULT '0',
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak`
--

DROP TABLE IF EXISTS `tiebreak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(15) NOT NULL DEFAULT 'cumulative',
  `count` varchar(15) NOT NULL DEFAULT '0',
  `multiplier` int(11) NOT NULL DEFAULT '1',
  `priority` int(11) NOT NULL DEFAULT '0',
  `tb_set` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
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
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `timeslot`
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn`
--

DROP TABLE IF EXISTS `tourn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) DEFAULT NULL,
  `webname` varchar(63) DEFAULT NULL,
  `name` varchar(63) NOT NULL DEFAULT '',
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `reg_start` datetime DEFAULT NULL,
  `reg_end` datetime DEFAULT NULL,
  `results` tinyint(1) DEFAULT NULL,
  `hidden` tinyint(1) DEFAULT NULL,
  `approved` tinyint(1) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `tourn_circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_circuit` (
 	`id` int(11) NOT NULL AUTO_INCREMENT,
	`tourn` int(11) NOT NULL DEFAULT '0',
	`circuit` int(11) NOT NULL DEFAULT '0',
	`key` varchar(15) NOT NULL DEFAULT '',
	`value` varchar(127) NOT NULL DEFAULT '',
	`text` text,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `tourn` (`tourn`),
  	KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


DROP TABLE IF EXISTS `tourn_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_setting` (
 	`id` int(11) NOT NULL AUTO_INCREMENT,
	`tourn` int(11) NOT NULL DEFAULT '0',
	`key` varchar(15) NOT NULL DEFAULT '',
	`value` varchar(127) NOT NULL DEFAULT '',
	`text` text,
  	`timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  	PRIMARY KEY (`id`),
  	KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `tourn_site`
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
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_judge`
--

DROP TABLE IF EXISTS `chapter_judge`;
DROP TABLE IF EXISTS `chapter_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `notes` varchar(254) DEFAULT NULL,
  `cell` varchar(15) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `account` (`account`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-02-29 20:17:30
