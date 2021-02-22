-- MySQL dump 10.17  Distrib 10.3.25-MariaDB, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: tabroom
-- ------------------------------------------------------
-- Server version	10.3.25-MariaDB-0ubuntu0.20.04.1-log

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
-- Table structure for table `ad`
--

DROP TABLE IF EXISTS `ad`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ad` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(255) DEFAULT NULL,
  `filename` varchar(127) DEFAULT NULL,
  `url` varchar(255) DEFAULT NULL,
  `sort_order` smallint(6) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `approved` tinyint(1) NOT NULL DEFAULT 0,
  `person` int(11) NOT NULL,
  `approved_by` int(11) NOT NULL,
  `created_at` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `person` (`person`),
  KEY `approved_by` (`approved_by`)
) ENGINE=InnoDB AUTO_INCREMENT=99 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `autoqueue`
--

DROP TABLE IF EXISTS `autoqueue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `autoqueue` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `active_at` datetime DEFAULT NULL,
  `message` varchar(255) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=146313 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ballot`
--

DROP TABLE IF EXISTS `ballot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ballot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `side` tinyint(1) NOT NULL DEFAULT 0,
  `speakerorder` smallint(6) NOT NULL DEFAULT 0,
  `seat` varchar(6) DEFAULT NULL,
  `chair` tinyint(1) NOT NULL DEFAULT 0,
  `bye` tinyint(1) NOT NULL DEFAULT 0,
  `forfeit` tinyint(1) NOT NULL DEFAULT 0,
  `tv` tinyint(1) NOT NULL DEFAULT 0,
  `audit` tinyint(1) NOT NULL DEFAULT 0,
  `judge_started` datetime DEFAULT NULL,
  `started_by` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,
  `audited_by` int(11) DEFAULT NULL,
  `judge` int(11) NOT NULL DEFAULT 0,
  `panel` int(11) NOT NULL DEFAULT 0,
  `entry` int(11) NOT NULL DEFAULT 0,
  `hangout_admin` tinyint(4) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `ballot_ejp` (`entry`,`judge`,`panel`),
  UNIQUE KEY `ballot_sideorder` (`panel`,`judge`,`side`,`speakerorder`),
  UNIQUE KEY `uk_ballots` (`judge`,`entry`,`panel`),
  KEY `panel` (`panel`),
  KEY `judge` (`judge`),
  KEY `audited_by` (`audited_by`),
  KEY `entered_by` (`entered_by`),
  KEY `entry` (`entry`),
  KEY `started_by` (`started_by`)
) ENGINE=InnoDB AUTO_INCREMENT=24990130 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `campus_log`
--

DROP TABLE IF EXISTS `campus_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `campus_log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `uuid` varchar(127) NOT NULL DEFAULT '',
  `description` varchar(255) NOT NULL DEFAULT '',
  `person` int(11) NOT NULL DEFAULT 0,
  `tourn` int(11) NOT NULL DEFAULT 0,
  `panel` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `panel` (`panel`),
  KEY `tourn` (`tourn`),
  KEY `school` (`school`),
  KEY `entry` (`entry`),
  KEY `judge` (`judge`),
  KEY `person` (`person`)
) ENGINE=InnoDB AUTO_INCREMENT=2096922 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `category` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `pattern` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=50508 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `category_setting`
--

DROP TABLE IF EXISTS `category_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `category_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `category` int(11) DEFAULT NULL,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_judge_group_setting` (`category`,`tag`),
  KEY `category` (`category`)
) ENGINE=InnoDB AUTO_INCREMENT=704478 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `change_log`
--

DROP TABLE IF EXISTS `change_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `change_log` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(63) DEFAULT NULL,
  `description` mediumtext DEFAULT NULL,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `deleted` tinyint(1) DEFAULT NULL,
  `created` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `event` (`event`),
  KEY `tournament` (`tourn`),
  KEY `tourn` (`tourn`),
  KEY `new_panel` (`new_panel`),
  KEY `old_panel` (`old_panel`),
  KEY `entry` (`entry`),
  KEY `types` (`type`,`tourn`),
  KEY `type` (`type`),
  KEY `created` (`created`),
  KEY `person` (`person`)
) ENGINE=InnoDB AUTO_INCREMENT=8532139 DEFAULT CHARSET=utf8;
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
  `street` varchar(255) DEFAULT NULL,
  `city` varchar(63) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `zip` mediumint(9) DEFAULT NULL,
  `postal` varchar(15) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `coaches` varchar(255) DEFAULT NULL,
  `self_prefs` tinyint(1) NOT NULL DEFAULT 0,
  `level` varchar(15) DEFAULT NULL,
  `nsda` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `naudl` tinyint(1) NOT NULL DEFAULT 0,
  `ipeds` varchar(15) DEFAULT NULL,
  `nces` varchar(15) DEFAULT NULL,
  `ceeb` varchar(15) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `nsda_index` (`nsda`),
  KEY `nsda` (`nsda`)
) ENGINE=InnoDB AUTO_INCREMENT=76193 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_circuit`
--

DROP TABLE IF EXISTS `chapter_circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(15) DEFAULT NULL,
  `full_member` tinyint(1) NOT NULL DEFAULT 0,
  `circuit` int(11) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT 0,
  `region` int(11) DEFAULT NULL,
  `circuit_membership` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_chapter_circuit` (`chapter`,`circuit`),
  KEY `chapter` (`chapter`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=54150 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_judge`
--

DROP TABLE IF EXISTS `chapter_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `first` varchar(127) DEFAULT NULL,
  `middle` varchar(63) DEFAULT NULL,
  `last` varchar(127) DEFAULT NULL,
  `ada` tinyint(1) NOT NULL DEFAULT 0,
  `retired` tinyint(1) NOT NULL DEFAULT 0,
  `phone` varchar(31) DEFAULT NULL,
  `email` varchar(127) DEFAULT NULL,
  `diet` varchar(255) DEFAULT NULL,
  `notes` varchar(255) DEFAULT NULL,
  `notes_timestamp` datetime DEFAULT NULL,
  `gender` char(1) DEFAULT NULL,
  `nsda` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `person_request` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`),
  KEY `person` (`person`)
) ENGINE=InnoDB AUTO_INCREMENT=281542 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `chapter_setting`
--

DROP TABLE IF EXISTS `chapter_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `chapter_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `chapter` int(11) DEFAULT NULL,
  `tag` varchar(32) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=161133 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit`
--

DROP TABLE IF EXISTS `circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `tz` varchar(63) DEFAULT NULL,
  `active` tinyint(1) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `webname` varchar(31) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=220 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_membership`
--

DROP TABLE IF EXISTS `circuit_membership`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_membership` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) DEFAULT NULL,
  `approval` tinyint(1) DEFAULT NULL,
  `description` varchar(127) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `circuit_setting`
--

DROP TABLE IF EXISTS `circuit_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `circuit_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `circuit` int(11) NOT NULL DEFAULT 0,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_circuit_setting` (`circuit`,`tag`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=778 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession`
--

DROP TABLE IF EXISTS `concession`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `price` decimal(8,2) DEFAULT NULL,
  `description` mediumtext DEFAULT NULL,
  `deadline` datetime DEFAULT NULL,
  `cap` int(11) DEFAULT NULL,
  `school_cap` int(11) DEFAULT NULL,
  `billing_code` varchar(63) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3372 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession_option`
--

DROP TABLE IF EXISTS `concession_option`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession_option` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(8) NOT NULL,
  `description` varchar(31) NOT NULL,
  `disabled` tinyint(1) DEFAULT NULL,
  `concession_type` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `concession_type` (`concession_type`)
) ENGINE=InnoDB AUTO_INCREMENT=222 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession_purchase`
--

DROP TABLE IF EXISTS `concession_purchase`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession_purchase` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `quantity` int(11) NOT NULL,
  `placed` datetime DEFAULT NULL,
  `fulfilled` tinyint(1) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `concession` int(11) DEFAULT NULL,
  `invoice` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=31845 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession_purchase_option`
--

DROP TABLE IF EXISTS `concession_purchase_option`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession_purchase_option` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `concession_purchase` int(11) NOT NULL,
  `concession_option` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_purchase_option` (`concession_purchase`,`concession_option`),
  KEY `concession_purchase` (`concession_purchase`),
  KEY `concession_option` (`concession_option`)
) ENGINE=InnoDB AUTO_INCREMENT=10266 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `concession_type`
--

DROP TABLE IF EXISTS `concession_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `concession_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(31) NOT NULL,
  `description` text DEFAULT NULL,
  `concession` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `concession` (`concession`)
) ENGINE=InnoDB AUTO_INCREMENT=112 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `conflict`
--

DROP TABLE IF EXISTS `conflict`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `conflict` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(15) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `conflicted` int(11) DEFAULT NULL,
  `chapter` int(11) NOT NULL DEFAULT 0,
  `added_by` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_constraint` (`person`,`conflicted`,`chapter`),
  KEY `chapter` (`chapter`),
  KEY `conflict` (`conflicted`),
  KEY `added_by` (`added_by`),
  KEY `person` (`person`)
) ENGINE=InnoDB AUTO_INCREMENT=38365 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `diocese`
--

DROP TABLE IF EXISTS `diocese`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `diocese` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `code` varchar(15) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `quota` tinyint(4) DEFAULT NULL,
  `active` tinyint(1) NOT NULL DEFAULT 0,
  `archdiocese` tinyint(1) NOT NULL DEFAULT 0,
  `cooke_award_points` smallint(6) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `codes` (`active`,`code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `district`
--

DROP TABLE IF EXISTS `district`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `district` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) DEFAULT NULL,
  `code` varchar(16) DEFAULT NULL,
  `location` varchar(16) DEFAULT NULL,
  `chair` int(11) DEFAULT NULL,
  `level` tinyint(4) DEFAULT NULL,
  `realm` varchar(8) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `code` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=1008 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `email`
--

DROP TABLE IF EXISTS `email`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `email` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `subject` varchar(127) DEFAULT NULL,
  `content` mediumtext DEFAULT NULL,
  `metadata` mediumtext DEFAULT NULL,
  `sent_to` varchar(511) DEFAULT NULL,
  `sent_at` datetime DEFAULT NULL,
  `sender` int(11) DEFAULT NULL,
  `hidden` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=55994 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry`
--

DROP TABLE IF EXISTS `entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(63) DEFAULT NULL,
  `name` varchar(127) DEFAULT NULL,
  `ada` tinyint(1) DEFAULT NULL,
  `active` tinyint(1) NOT NULL DEFAULT 0,
  `tba` tinyint(1) NOT NULL DEFAULT 0,
  `dropped` tinyint(1) NOT NULL DEFAULT 0,
  `waitlist` tinyint(1) NOT NULL DEFAULT 0,
  `unconfirmed` tinyint(1) NOT NULL DEFAULT 0,
  `dq` tinyint(1) NOT NULL DEFAULT 0,
  `created_at` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `registered_by` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tournament` (`tourn`,`school`),
  KEY `tournament_2` (`tourn`,`school`,`event`),
  KEY `code` (`code`),
  KEY `school` (`school`),
  KEY `id` (`id`),
  KEY `event` (`event`),
  KEY `registered_by` (`registered_by`)
) ENGINE=InnoDB AUTO_INCREMENT=3474562 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`tabroom`@`localhost`*/ /*!50003 trigger `insert_entry_active`
	before insert on `entry`

	FOR EACH ROW
		BEGIN

			IF
				NEW.dropped = 1
				OR NEW.waitlist = 1
				OR NEW.unconfirmed = 1
				OR NEW.unconfirmed = 2
			THEN
				set NEW.active = 0;
			ELSE
				set NEW.active = 1;

			END IF;

		END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`tabroom`@`localhost`*/ /*!50003 trigger `update_entry_active`
	before update on `entry`

	FOR EACH ROW
		BEGIN

			IF
				NEW.dropped = 1
				OR NEW.waitlist = 1
				OR NEW.unconfirmed = 1
				OR NEW.unconfirmed = 2
			THEN
				set NEW.active = 0;
			ELSE
				set NEW.active = 1;


			END IF;

		END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `entry_setting`
--

DROP TABLE IF EXISTS `entry_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(32) NOT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`)
) ENGINE=InnoDB AUTO_INCREMENT=4469470 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `entry_student`
--

DROP TABLE IF EXISTS `entry_student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `entry_student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `entry` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_entry_student` (`entry`,`student`),
  KEY `entry` (`entry`),
  KEY `student` (`student`)
) ENGINE=InnoDB AUTO_INCREMENT=4511683 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `type` enum('speech','congress','debate','wudc','wsdc') NOT NULL DEFAULT 'speech',
  `level` enum('open','jv','novice','champ','es-open','es-novice','middle') NOT NULL DEFAULT 'open',
  `fee` decimal(8,2) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `pattern` int(11) DEFAULT NULL,
  `rating_subset` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`),
  KEY `category` (`category`)
) ENGINE=InnoDB AUTO_INCREMENT=170267 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event_setting`
--

DROP TABLE IF EXISTS `event_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `event` int(11) NOT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=3977970 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `file`
--

DROP TABLE IF EXISTS `file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `file` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(127) DEFAULT NULL,
  `type` varchar(16) DEFAULT NULL,
  `label` varchar(127) DEFAULT NULL,
  `filename` varchar(127) DEFAULT NULL,
  `published` tinyint(1) NOT NULL DEFAULT 0,
  `coach` tinyint(1) DEFAULT NULL,
  `uploaded` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `webpage` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`),
  KEY `school` (`school`),
  KEY `district` (`district`)
) ENGINE=InnoDB AUTO_INCREMENT=24912 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `fine`
--

DROP TABLE IF EXISTS `fine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `fine` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `reason` varchar(255) DEFAULT NULL,
  `amount` decimal(8,2) DEFAULT NULL,
  `payment` tinyint(1) NOT NULL DEFAULT 0,
  `levied_at` datetime DEFAULT NULL,
  `levied_by` int(11) DEFAULT NULL,
  `deleted` tinyint(1) NOT NULL DEFAULT 0,
  `deleted_at` datetime DEFAULT NULL,
  `deleted_by` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `invoice` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `school` (`school`),
  KEY `tourn` (`tourn`),
  KEY `levied_by` (`levied_by`)
) ENGINE=InnoDB AUTO_INCREMENT=5346877 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `follower`
--

DROP TABLE IF EXISTS `follower`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follower` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(8) DEFAULT NULL,
  `cell` bigint(20) DEFAULT NULL,
  `domain` varchar(64) DEFAULT NULL,
  `email` varchar(64) DEFAULT NULL,
  `follower` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`),
  KEY `judge` (`judge`),
  KEY `school` (`school`),
  KEY `follower` (`follower`),
  KEY `type` (`type`)
) ENGINE=InnoDB AUTO_INCREMENT=881680 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hotel`
--

DROP TABLE IF EXISTS `hotel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `hotel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `multiple` float DEFAULT NULL,
  `surcharge` float DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=290 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing`
--

DROP TABLE IF EXISTS `housing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `housing` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(7) DEFAULT NULL,
  `night` date DEFAULT NULL,
  `waitlist` tinyint(1) NOT NULL DEFAULT 0,
  `tba` tinyint(1) NOT NULL DEFAULT 0,
  `requested` datetime DEFAULT NULL,
  `requestor` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `student` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=33904 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `housing_slots`
--

DROP TABLE IF EXISTS `housing_slots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `housing_slots` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `night` date DEFAULT NULL,
  `slots` smallint(6) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=615 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `invoice`
--

DROP TABLE IF EXISTS `invoice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `invoice` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `blusynergy` int(11) DEFAULT NULL,
  `blu_number` varchar(31) DEFAULT NULL,
  `total` decimal(8,2) NOT NULL DEFAULT 0.00,
  `paid` tinyint(1) DEFAULT NULL,
  `school` int(11) NOT NULL,
  `details` text DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=21067 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jpool`
--

DROP TABLE IF EXISTS `jpool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `jpool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT 0,
  `parent` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `category` (`category`),
  KEY `parent` (`parent`)
) ENGINE=InnoDB AUTO_INCREMENT=63304 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jpool_judge`
--

DROP TABLE IF EXISTS `jpool_judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `jpool_judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `judge` int(11) NOT NULL DEFAULT 0,
  `jpool` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `pool` (`jpool`),
  KEY `judge` (`judge`)
) ENGINE=InnoDB AUTO_INCREMENT=929186 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jpool_round`
--

DROP TABLE IF EXISTS `jpool_round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `jpool_round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `jpool` int(11) DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `round` (`round`),
  KEY `jpool` (`jpool`)
) ENGINE=InnoDB AUTO_INCREMENT=96458 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jpool_setting`
--

DROP TABLE IF EXISTS `jpool_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `jpool_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `jpool` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `jpool` (`jpool`)
) ENGINE=InnoDB AUTO_INCREMENT=47077 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge`
--

DROP TABLE IF EXISTS `judge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `score` int(11) DEFAULT NULL,
  `tmp` varchar(63) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`),
  KEY `school` (`school`),
  KEY `chapter_judge` (`chapter_judge`),
  KEY `person` (`person`),
  KEY `category` (`category`),
  KEY `alt` (`alt_category`)
) ENGINE=InnoDB AUTO_INCREMENT=1488746 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_hire`
--

DROP TABLE IF EXISTS `judge_hire`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_hire` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=7053471 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `judge_setting`
--

DROP TABLE IF EXISTS `judge_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `judge_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `conditional` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_judge_setting` (`judge`,`tag`,`conditional`),
  KEY `judge` (`judge`),
  KEY `tag` (`tag`)
) ENGINE=InnoDB AUTO_INCREMENT=3220570 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `login`
--

DROP TABLE IF EXISTS `login`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `login` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(63) NOT NULL,
  `password` varchar(63) DEFAULT NULL,
  `sha512` varchar(128) DEFAULT NULL,
  `accesses` int(11) NOT NULL DEFAULT 0,
  `last_access` datetime DEFAULT NULL,
  `pass_timestamp` datetime DEFAULT NULL,
  `pass_changekey` varchar(127) DEFAULT NULL,
  `pass_change_expires` datetime DEFAULT NULL,
  `source` char(4) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `login_username` (`username`),
  KEY `person` (`person`),
  CONSTRAINT `login_ibfk_1` FOREIGN KEY (`person`) REFERENCES `person` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=347039 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `nsda_event_category`
--

DROP TABLE IF EXISTS `nsda_event_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `nsda_event_category` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(31) DEFAULT NULL,
  `type` enum('s','d','c') DEFAULT NULL,
  `code` smallint(6) DEFAULT NULL,
  `national` tinyint(1) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=302 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `panel`
--

DROP TABLE IF EXISTS `panel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `panel` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `room` (`room`),
  KEY `round` (`round`),
  KEY `flip_at` (`flip_at`)
) ENGINE=InnoDB AUTO_INCREMENT=5061927 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `panel_setting`
--

DROP TABLE IF EXISTS `panel_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `panel_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `panel` int(11) DEFAULT NULL,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_judge_group_setting` (`panel`,`tag`),
  KEY `panel` (`panel`),
  KEY `setting` (`setting`)
) ENGINE=InnoDB AUTO_INCREMENT=457030 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pattern`
--

DROP TABLE IF EXISTS `pattern`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `pattern` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(31) DEFAULT NULL,
  `type` tinyint(4) DEFAULT NULL,
  `max` tinyint(4) DEFAULT NULL,
  `exclude` mediumtext DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=16687 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `permission`
--

DROP TABLE IF EXISTS `permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `permission` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(15) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `details` text DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`),
  KEY `region` (`region`),
  KEY `tourn` (`tourn`),
  KEY `circuit` (`circuit`),
  KEY `person` (`person`),
  KEY `category` (`category`),
  KEY `district` (`district`)
) ENGINE=InnoDB AUTO_INCREMENT=28261382 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `person` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `person_email` (`email`),
  KEY `email` (`email`),
  KEY `noemail` (`no_email`),
  KEY `nsda` (`nsda`)
) ENGINE=InnoDB AUTO_INCREMENT=346358 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `person_setting`
--

DROP TABLE IF EXISTS `person_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `person_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_account_setting` (`person`,`tag`),
  KEY `person` (`person`)
) ENGINE=InnoDB AUTO_INCREMENT=1164373 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `qualifier`
--

DROP TABLE IF EXISTS `qualifier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `qualifier` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `result` varchar(127) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `qualifier_tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=90487 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating`
--

DROP TABLE IF EXISTS `rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `sheet` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_entry_rating` (`judge`,`entry`,`sheet`),
  UNIQUE KEY `uk_school_rating` (`judge`,`school`,`sheet`),
  KEY `entry` (`entry`),
  KEY `judge` (`judge`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=59539007 DEFAULT CHARSET=utf8;
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
  `category` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=617 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rating_tier`
--

DROP TABLE IF EXISTS `rating_tier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rating_tier` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20029 DEFAULT CHARSET=utf8;
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
  `code` varchar(31) DEFAULT NULL,
  `quota` tinyint(4) DEFAULT NULL,
  `arch` tinyint(1) DEFAULT NULL,
  `cooke_pts` int(11) DEFAULT NULL,
  `sweeps` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1572 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `region_fine`
--

DROP TABLE IF EXISTS `region_fine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `region_fine` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `amount` float DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `levied_at` datetime DEFAULT NULL,
  `levied_by` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=383 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `region_setting`
--

DROP TABLE IF EXISTS `region_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `region_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(32) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `event` (`event`),
  KEY `region` (`region`)
) ENGINE=InnoDB AUTO_INCREMENT=1130 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `result`
--

DROP TABLE IF EXISTS `result`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `result` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `entry` (`entry`),
  KEY `student` (`student`),
  KEY `result_set` (`result_set`),
  KEY `round` (`round`)
) ENGINE=InnoDB AUTO_INCREMENT=4202035 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `result_key`
--

DROP TABLE IF EXISTS `result_key`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `result_key` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(63) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `no_sort` tinyint(1) DEFAULT NULL,
  `sort_desc` tinyint(1) DEFAULT NULL,
  `result_set` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `result_set` (`result_set`)
) ENGINE=InnoDB AUTO_INCREMENT=702723 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `result_set`
--

DROP TABLE IF EXISTS `result_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `result_set` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `label` varchar(255) DEFAULT NULL,
  `bracket` tinyint(1) NOT NULL DEFAULT 0,
  `published` tinyint(1) NOT NULL DEFAULT 0,
  `coach` tinyint(1) DEFAULT NULL,
  `generated` datetime DEFAULT NULL,
  `tourn` int(11) NOT NULL,
  `sweep_set` int(11) DEFAULT NULL,
  `sweep_award` int(11) DEFAULT NULL,
  `event` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `tag` enum('entry','student','chapter') NOT NULL DEFAULT 'entry',
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`),
  KEY `event` (`event`),
  KEY `result_labels` (`label`,`event`)
) ENGINE=InnoDB AUTO_INCREMENT=166786 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `result_value`
--

DROP TABLE IF EXISTS `result_value`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `result_value` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `value` mediumtext DEFAULT NULL,
  `priority` smallint(6) DEFAULT NULL,
  `result` int(11) DEFAULT NULL,
  `result_key` int(11) NOT NULL DEFAULT 0,
  `tiebreak_set` int(11) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `result` (`result`),
  KEY `result_key` (`result_key`)
) ENGINE=InnoDB AUTO_INCREMENT=21802693 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room`
--

DROP TABLE IF EXISTS `room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_room` (`site`,`name`),
  KEY `site` (`site`),
  KEY `api` (`api`)
) ENGINE=InnoDB AUTO_INCREMENT=290960 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `room_strike`
--

DROP TABLE IF EXISTS `room_strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `room_strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(15) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `room` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `room` (`room`)
) ENGINE=InnoDB AUTO_INCREMENT=28872 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `round`
--

DROP TABLE IF EXISTS `round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_round` (`name`,`event`),
  KEY `timeslot` (`timeslot`),
  KEY `event` (`event`),
  KEY `tiebreak_set` (`tiebreak_set`)
) ENGINE=InnoDB AUTO_INCREMENT=687972 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `round_setting`
--

DROP TABLE IF EXISTS `round_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `round_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `round` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `round` (`round`)
) ENGINE=InnoDB AUTO_INCREMENT=2084746 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpool`
--

DROP TABLE IF EXISTS `rpool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpool` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(31) DEFAULT NULL,
  `tourn` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23164305 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpool_room`
--

DROP TABLE IF EXISTS `rpool_room`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpool_room` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `rpool` int(11) DEFAULT NULL,
  `room` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `room` (`room`),
  KEY `room_group` (`rpool`)
) ENGINE=InnoDB AUTO_INCREMENT=597674 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpool_round`
--

DROP TABLE IF EXISTS `rpool_round`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpool_round` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `rpool` int(11) DEFAULT NULL,
  `round` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `round` (`round`),
  KEY `room_group` (`rpool`)
) ENGINE=InnoDB AUTO_INCREMENT=476502 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rpool_setting`
--

DROP TABLE IF EXISTS `rpool_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `rpool_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(32) NOT NULL,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `rpool` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `rpool` (`rpool`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `school`
--

DROP TABLE IF EXISTS `school`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `code` varchar(15) DEFAULT NULL,
  `onsite` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `state` varchar(4) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `district` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `chapter` (`chapter`),
  KEY `tourn` (`tourn`),
  KEY `region` (`region`)
) ENGINE=InnoDB AUTO_INCREMENT=421614 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `school_setting`
--

DROP TABLE IF EXISTS `school_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(32) NOT NULL,
  `value` varchar(64) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `chapter` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `school` (`school`)
) ENGINE=InnoDB AUTO_INCREMENT=1985597 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `score`
--

DROP TABLE IF EXISTS `score`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `score` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(15) DEFAULT NULL,
  `value` float NOT NULL DEFAULT 0,
  `content` mediumtext DEFAULT NULL,
  `topic` varchar(127) DEFAULT NULL,
  `speech` smallint(6) NOT NULL DEFAULT 0,
  `position` tinyint(4) NOT NULL DEFAULT 0,
  `ballot` int(11) DEFAULT NULL,
  `student` int(11) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `tiebreak` tinyint(4) DEFAULT NULL,
  `cat_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_score_restraint` (`tag`,`ballot`,`speech`,`position`,`student`),
  KEY `student` (`student`),
  KEY `ballot` (`ballot`),
  KEY `tags` (`ballot`,`tag`)
) ENGINE=InnoDB AUTO_INCREMENT=27907268 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `session`
--

DROP TABLE IF EXISTS `session`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `session` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userkey` varchar(127) DEFAULT NULL,
  `ip` varchar(63) DEFAULT NULL,
  `defaults` mediumtext DEFAULT NULL,
  `su` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `weekend` int(11) DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `userkey` (`userkey`)
) ENGINE=InnoDB AUTO_INCREMENT=4094158 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `setting`
--

DROP TABLE IF EXISTS `setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(31) NOT NULL,
  `subtype` varchar(31) NOT NULL,
  `tag` varchar(31) NOT NULL,
  `value_type` enum('text','string','bool','integer','decimal','datetime','enum') DEFAULT NULL,
  `conditions` text DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tag` (`tag`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `setting_label`
--

DROP TABLE IF EXISTS `setting_label`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `setting_label` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `lang` char(2) DEFAULT NULL,
  `label` varchar(127) DEFAULT NULL,
  `guide` text DEFAULT NULL,
  `options` text DEFAULT NULL,
  `setting` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `shift`
--

DROP TABLE IF EXISTS `shift`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `shift` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(31) DEFAULT NULL,
  `type` enum('signup','strike','both') DEFAULT NULL,
  `fine` smallint(6) DEFAULT NULL,
  `no_hires` tinyint(1) NOT NULL DEFAULT 0,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=20776 DEFAULT CHARSET=utf8;
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
  `online` tinyint(1) NOT NULL DEFAULT 0,
  `directions` mediumtext DEFAULT NULL,
  `dropoff` varchar(255) DEFAULT NULL,
  `host` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=8322 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `stats`
--

DROP TABLE IF EXISTS `stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `stats` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `type` varchar(15) DEFAULT NULL,
  `tag` varchar(31) DEFAULT NULL,
  `value` decimal(8,2) DEFAULT NULL,
  `event` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=2541896 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `strike`
--

DROP TABLE IF EXISTS `strike`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `strike` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `judge` (`judge`),
  KEY `school` (`school`),
  KEY `entry` (`entry`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1277872 DEFAULT CHARSET=utf8;
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `last` (`last`),
  KEY `first` (`first`),
  KEY `person` (`person`),
  KEY `nsda` (`nsda`),
  KEY `chapter` (`chapter`)
) ENGINE=InnoDB AUTO_INCREMENT=1044846 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student_ballot`
--

DROP TABLE IF EXISTS `student_ballot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student_ballot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(15) DEFAULT NULL,
  `panel` int(11) NOT NULL DEFAULT 0,
  `entry` int(11) NOT NULL DEFAULT 0,
  `voter` int(11) NOT NULL DEFAULT 0,
  `value` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `panel` (`panel`),
  KEY `entry` (`entry`),
  KEY `voter` (`voter`)
) ENGINE=InnoDB AUTO_INCREMENT=140 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student_setting`
--

DROP TABLE IF EXISTS `student_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `student` int(11) DEFAULT NULL,
  `tag` varchar(32) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_text` text DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `student` (`student`)
) ENGINE=InnoDB AUTO_INCREMENT=2208801 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `student_vote`
--

DROP TABLE IF EXISTS `student_vote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `student_vote` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` enum('nominee','rank') NOT NULL DEFAULT 'rank',
  `value` int(11) DEFAULT NULL,
  `panel` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `voter` int(11) DEFAULT NULL,
  `entered_by` int(11) DEFAULT NULL,
  `entered_at` datetime DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `sv_evp` (`panel`,`entry`,`voter`),
  KEY `entry` (`entry`),
  KEY `voter` (`voter`)
) ENGINE=InnoDB AUTO_INCREMENT=67048 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep_award`
--

DROP TABLE IF EXISTS `sweep_award`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep_award` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) NOT NULL,
  `description` text DEFAULT NULL,
  `target` enum('entry','school','individual') DEFAULT NULL,
  `period` enum('annual','cumulative') DEFAULT NULL,
  `count` smallint(6) NOT NULL DEFAULT 0,
  `min_schools` smallint(6) NOT NULL DEFAULT 0,
  `min_entries` smallint(6) NOT NULL DEFAULT 0,
  `circuit` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep_award_event`
--

DROP TABLE IF EXISTS `sweep_award_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep_award_event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL,
  `abbr` varchar(15) DEFAULT NULL,
  `level` varchar(15) DEFAULT NULL,
  `sweep_set` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `sweep_set` (`sweep_set`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep_event`
--

DROP TABLE IF EXISTS `sweep_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep_event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sweep_set` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `event_type` enum('all','congress','debate','speech','wsdc','wudc') DEFAULT NULL,
  `event_level` enum('all','open','jv','novice','champ','es-open','es-novice','middle') DEFAULT NULL,
  `nsda_event_category` int(11) DEFAULT NULL,
  `sweep_award_event` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `sweep_set` (`sweep_set`),
  KEY `event` (`event`)
) ENGINE=InnoDB AUTO_INCREMENT=144641 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep_include`
--

DROP TABLE IF EXISTS `sweep_include`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep_include` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `parent` int(11) DEFAULT NULL,
  `child` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8156 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep_rule`
--

DROP TABLE IF EXISTS `sweep_rule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep_rule` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) DEFAULT NULL,
  `value` varchar(63) DEFAULT NULL,
  `place` smallint(6) DEFAULT NULL,
  `count` varchar(15) DEFAULT NULL,
  `rev_min` int(11) DEFAULT NULL,
  `count_round` int(11) DEFAULT NULL,
  `truncate` smallint(6) DEFAULT NULL,
  `sweep_set` int(11) DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `sweep_set` (`sweep_set`)
) ENGINE=InnoDB AUTO_INCREMENT=104250 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sweep_set`
--

DROP TABLE IF EXISTS `sweep_set`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sweep_set` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(31) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `sweep_award` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`),
  KEY `sweep_award` (`sweep_award`)
) ENGINE=InnoDB AUTO_INCREMENT=22204 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak`
--

DROP TABLE IF EXISTS `tiebreak`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tiebreak_set` (`tiebreak_set`)
) ENGINE=InnoDB AUTO_INCREMENT=682319 DEFAULT CHARSET=utf8;
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tiebreak_tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=145187 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tiebreak_set_setting`
--

DROP TABLE IF EXISTS `tiebreak_set_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tiebreak_set_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) DEFAULT NULL,
  `value` varchar(127) DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `tiebreak_set` int(11) DEFAULT NULL,
  `value_text` mediumtext DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tiebreak_set` (`tiebreak_set`)
) ENGINE=InnoDB AUTO_INCREMENT=159702 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `timeslot`
--

DROP TABLE IF EXISTS `timeslot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `timeslot` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=200448 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `topic`
--

DROP TABLE IF EXISTS `topic`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `topic` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tag` varchar(31) DEFAULT NULL,
  `source` varchar(15) DEFAULT NULL,
  `event_type` varchar(31) DEFAULT NULL,
  `topic_text` text DEFAULT NULL,
  `school_year` int(11) DEFAULT NULL,
  `sort_order` smallint(6) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created_at` datetime DEFAULT NULL,
  `created_by` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn`
--

DROP TABLE IF EXISTS `tourn`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL DEFAULT '',
  `city` varchar(31) DEFAULT NULL,
  `state` char(4) DEFAULT NULL,
  `country` char(4) DEFAULT NULL,
  `tz` varchar(31) DEFAULT NULL,
  `webname` varchar(64) DEFAULT NULL,
  `hidden` tinyint(1) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `reg_start` datetime DEFAULT NULL,
  `reg_end` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `start` (`start`)
) ENGINE=InnoDB AUTO_INCREMENT=19717 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_circuit`
--

DROP TABLE IF EXISTS `tourn_circuit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_circuit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `approved` tinyint(1) NOT NULL DEFAULT 0,
  `tourn` int(11) NOT NULL DEFAULT 0,
  `circuit` int(11) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_tourn_circuit` (`tourn`,`circuit`),
  KEY `tourn` (`tourn`),
  KEY `circuit` (`circuit`)
) ENGINE=InnoDB AUTO_INCREMENT=25883 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_fee`
--

DROP TABLE IF EXISTS `tourn_fee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_fee` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `amount` decimal(8,2) DEFAULT NULL,
  `reason` varchar(63) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=10201 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_ignore`
--

DROP TABLE IF EXISTS `tourn_ignore`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_ignore` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`),
  KEY `person` (`person`)
) ENGINE=InnoDB AUTO_INCREMENT=86926 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_setting`
--

DROP TABLE IF EXISTS `tourn_setting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_setting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) NOT NULL DEFAULT 0,
  `tag` varchar(31) NOT NULL DEFAULT '',
  `value` varchar(127) NOT NULL DEFAULT '',
  `value_text` mediumtext DEFAULT NULL,
  `value_date` datetime DEFAULT NULL,
  `setting` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_tourn_setting` (`tourn`,`tag`),
  KEY `tourn` (`tourn`),
  KEY `tags` (`tag`,`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=422712 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tourn_site`
--

DROP TABLE IF EXISTS `tourn_site`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tourn_site` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tourn` int(11) DEFAULT NULL,
  `site` int(11) NOT NULL DEFAULT 0,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  UNIQUE KEY `tourn_site_unique` (`tourn`,`site`),
  KEY `site` (`site`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=29394 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `webpage`
--

DROP TABLE IF EXISTS `webpage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `webpage` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(63) DEFAULT NULL,
  `content` mediumtext DEFAULT NULL,
  `published` tinyint(1) NOT NULL DEFAULT 0,
  `sitewide` tinyint(1) NOT NULL DEFAULT 0,
  `special` varchar(15) DEFAULT NULL,
  `page_order` smallint(6) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `parent` int(11) DEFAULT NULL,
  `last_editor` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=13399 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weekend`
--

DROP TABLE IF EXISTS `weekend`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weekend` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
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
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `tourn` (`tourn`)
) ENGINE=InnoDB AUTO_INCREMENT=1189 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-02-22 15:12:59
