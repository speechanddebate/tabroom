-- Table structure for table `follower`
--

DROP TABLE IF EXISTS `follower`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `follower` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cell` int(11) DEFAULT NULL, 
  `domain` varchar(64) DEFAULT NULL,
  `email` varchar(64) DEFAULT NULL,
  `follower` int(11) DEFAULT NULL,
  `type` varchar(8) DEFAULT NULL,
  `created_at` datetime NOT NULL,
  `updated_at` datetime NOT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `account` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

delete from follow_judge where timestamp < "2015-04-01 00:00:00";

insert into follower (cell, domain, email, judge, follower, timestamp, type) 
	select cell, domain, email, judge, follower, timestamp, "judge"
	from follow_judge;

delete from follow_entry where timestamp < "2015-04-01 00:00:00";

insert into follower (cell, domain, email, entry, follower,timestamp, type) 
	select cell, domain, email, entry, follower, timestamp, "entry"
	from follow_entry;

delete from follow_school where timestamp < "2015-04-01 00:00:00";

insert into follower (school, follower,timestamp, type) 
	select school, follower, timestamp, "school"
	from follow_school;

delete from follow_account where timestamp < "2015-04-01 00:00:00";

insert into follower (account, follower, timestamp, type) 
	select account, follower, timestamp, "school"
	from follow_account;

