-- Table structure for table `permission` --

DROP TABLE IF EXISTS `permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `permission` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL, 
  `tourn` int(11) DEFAULT NULL,
  `region` int(11) DEFAULT NULL,
  `chapter` int(11) DEFAULT NULL,
  `circuit` int(11) DEFAULT NULL,
  `judge_group` int(11) DEFAULT NULL,
  `tag` varchar(16),
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

insert into permission (tag, chapter, account, timestamp) 
	select "chapter", chapter, account, timestamp
	from chapter_admin
	where prefs != 1;

insert into permission (tag, chapter, account, timestamp) 
	select "chapter_prefs", chapter, account, timestamp
	from chapter_admin
	where prefs = 1;

insert into permission (tag, circuit, account, timestamp) 
	select "circuit", circuit, account, timestamp
	from circuit_admin;

insert into permission (tag, tourn, account, timestamp) 
	select "contact", tourn, account, timestamp
	from tourn_admin where tourn_admin.contact = 1;

insert into permission (tag, tourn, account, timestamp) 
	select "owner", tourn, account, timestamp
	from tourn_admin where tourn_admin.contact = 1;

insert into permission (tag, tourn, account, timestamp) 
	select "entry_only", tourn, account, timestamp
	from tourn_admin where tourn_admin.entry_only = 1;

insert into permission (tag, tourn, account, timestamp) 
	select "limited", tourn, account, timestamp
	from tourn_admin where tourn_admin.no_setup = 1;

insert into permission (tag, tourn, account, timestamp) 
	select "registration", tourn, account, timestamp
	from tourn_admin where tourn_admin.no_setup = 1;

insert into permission (tag, tourn, account, timestamp) 
	select "tabbing", tourn, account, timestamp
	from tourn_admin where tourn_admin.no_setup = 1;

insert into permission (tag, tourn, account, timestamp) 
	select "full_admin", tourn, account, timestamp
	from tourn_admin where tourn_admin.contact != 1 
		and tourn_admin.no_setup != 1
		and tourn_admin.entry_only != 1;

insert into permission (tag, region, account, timestamp) 
	select "region", region, account, timestamp
	from region_admin;


