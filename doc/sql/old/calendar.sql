
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `calendar` (
  `calendar_id` int(11) NOT NULL AUTO_INCREMENT,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL,
  `title` varchar(100) NOT NULL,
  `reg_start` datetime DEFAULT NULL,
  `reg_end` datetime DEFAULT NULL,
  `state` varchar(7) NOT NULL,
  `country` varchar(7) NOT NULL,
  `timezone` varchar(3) NOT NULL,
  `status_code` varchar(3) NOT NULL,
  `hidden` tinyint(4) NOT NULL DEFAULT '0',
  `location` varchar(100) DEFAULT NULL,
  `contact` varchar(100) DEFAULT NULL,
  `url` varchar(100) NOT NULL,
  `tabroom_id` int(11) DEFAULT NULL,
  `jot_id` int(11) DEFAULT NULL,
  `source` char(1) NOT NULL,
  PRIMARY KEY (`calendar_id`)
) ENGINE=InnoDB AUTO_INCREMENT=270 DEFAULT CHARSET=utf8;
