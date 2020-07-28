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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

