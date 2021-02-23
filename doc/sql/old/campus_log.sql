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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
