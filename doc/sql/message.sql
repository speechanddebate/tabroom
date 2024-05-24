
DROP TABLE IF EXISTS `message`;

/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `message` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `subject` varchar(255) NULL,
  `body` text NULL,
  `url` varchar(511) NULL,
  `sender_string` varchar(255) NULL,
  `person` int(11) NOT NULL,
  `tourn` int(11) NULL default 0,
  `sender` int(11) NULL,
  `email` int(11) NULL,
  `created_at` datetime NULL,
  `read_at` datetime NULL,
  `deleted_at` datetime NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8;

alter table message ADD CONSTRAINT fk_person FOREIGN KEY (person) REFERENCES person(id) ON UPDATE CASCADE ON DELETE CASCADE;

create INDEX person on message(person);
create INDEX sender on message(sender);

