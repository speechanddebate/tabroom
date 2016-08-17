
CREATE TABLE `survey` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(127) DEFAULT NULL,
  `type` varchar(15) DEFAULT NULL,
  `condition` varchar(31) DEFAULT NULL,
  `start` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uk_survey_name` (`tourn`,`circuit`,`name`),
  KEY `tourn` (`tourn`),
  KEY `category` (`category`),
  KEY `event` (`event`),
  KEY `circuit` (`circuit`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `survey_question` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `order` smallint(6) NOT NULL DEFAULT 0,
  `question` text DEFAULT NULL,
  `explanation` text DEFAULT NULL,
  `answer_format` varchar(15) DEFAULT NULL,
  `survey` int(11) DEFAULT NULL,
  `event` int(11) DEFAULT NULL,
  `category` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `survey` (`survey`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

CREATE TABLE `survey_response` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `start` datetime DEFAULT NULL,
  `survey` int(11) DEFAULT NULL,
  `person` int(11) DEFAULT NULL,
  `tourn` int(11) DEFAULT NULL,
  `judge` int(11) DEFAULT NULL,
  `entry` int(11) DEFAULT NULL,
  `school` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `survey` (`survey`),
  KEY `person` (`person`),
  KEY `tourn` (`tourn`),
  KEY `judge` (`judge`),
  KEY `entry` (`entry`),
  KEY `school` (`school`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;


CREATE TABLE `survey_answer` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `value` text, 
  `survey_response` int(11) DEFAULT NULL,
  `survey_question` int(11) DEFAULT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `survey_response` (`survey_response`),
  KEY `survey_question` (`survey_question`),
  KEY `id` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;


