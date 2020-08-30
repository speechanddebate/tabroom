
alter table sweep_event add sweep_award_event int after event_level;

CREATE TABLE `sweep_award_event` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(63) NOT NULL,
  `abbr` varchar(15),
  `level` varchar(15),
  `sweep_set` int(11),
  `timestamp` timestamp,
  PRIMARY KEY (`id`),
  KEY (`sweep_set`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

