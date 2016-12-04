-- MySQL dump 10.15  Distrib 10.0.27-MariaDB, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: tabroom
-- ------------------------------------------------------
-- Server version	10.0.27-MariaDB-0ubuntu0.16.04.1
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = '' */ ;
/*!50003 DROP TRIGGER IF EXISTS `tabroom.insert_entry_active` */;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`tabroom`@`localhost`*/ /*!50003 trigger `insert_entry_active`
	before insert on `entry`

	FOR EACH ROW
		BEGIN

			IF 
				NEW.dropped = 1 
				OR NEW.waitlist = 1 
				OR NEW.dq = 1 
				OR NEW.unconfirmed = 1
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
/*!50003 DROP TRIGGER IF EXISTS `tabroom.update_entry_active` */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`tabroom`@`localhost`*/ /*!50003 trigger `update_entry_active`
	before update on `entry`

	FOR EACH ROW
		BEGIN

			IF 
				NEW.dropped = 1 
				OR NEW.waitlist = 1 
				OR NEW.dq = 1 
				OR NEW.unconfirmed = 1
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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2016-12-04 11:43:55
