# ************************************************************
# Sequel Ace SQL dump
# Version 20050
#
# https://sequel-ace.com/
# https://github.com/Sequel-Ace/Sequel-Ace
#
# Host: 127.0.0.1 (MySQL 8.0.30)
# Database: employee
# Generation Time: 2023-08-18 09:11:56 +0000
# ************************************************************


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
SET NAMES utf8mb4;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE='NO_AUTO_VALUE_ON_ZERO', SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table admin
# ------------------------------------------------------------

DROP TABLE IF EXISTS `admin`;

CREATE TABLE `admin` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` varchar(255) NOT NULL,
  `salt` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `admin` WRITE;
/*!40000 ALTER TABLE `admin` DISABLE KEYS */;

INSERT INTO `admin` (`id`, `username`, `password`, `salt`)
VALUES
	(1,'wahyu','17B4EFB078A26CFA20E727D0B32D33D21BA54FD5BC218DCCF5A849D587EA2F3043C641CBF300687A605C243E5020EE7D662FAB4D21E016AA1E1E28B627836EFB','3576FB12BA35FBE32333F5D7D40FAADACF8C155BC9329F4125E89E7C3E60790A2DACCF44F26CA047E29004F3C123CF61B5CCD3D0E97CD2A80FE4BACFA40CD5CC');

/*!40000 ALTER TABLE `admin` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table employees
# ------------------------------------------------------------

DROP TABLE IF EXISTS `employees`;

CREATE TABLE `employees` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `company` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `phone` varchar(255) DEFAULT NULL,
  `address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `employees` WRITE;
/*!40000 ALTER TABLE `employees` DISABLE KEYS */;

INSERT INTO `employees` (`id`, `company`, `name`, `phone`, `address`)
VALUES
	(2,'string1','string','string','string'),
	(3,'string1','string1','string','string'),
	(5,'string','string','string','string'),
	(7,'string','string','string','string'),
	(8,'string','string','string','string');

/*!40000 ALTER TABLE `employees` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table project_employee
# ------------------------------------------------------------

DROP TABLE IF EXISTS `project_employee`;

CREATE TABLE `project_employee` (
  `ProjectsId` int unsigned DEFAULT NULL,
  `EmployeesId` int unsigned DEFAULT NULL,
  KEY `project_id` (`ProjectsId`),
  KEY `employee_id` (`EmployeesId`),
  CONSTRAINT `project_employee_ibfk_1` FOREIGN KEY (`ProjectsId`) REFERENCES `projects` (`id`) ON DELETE CASCADE,
  CONSTRAINT `project_employee_ibfk_2` FOREIGN KEY (`EmployeesId`) REFERENCES `employees` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `project_employee` WRITE;
/*!40000 ALTER TABLE `project_employee` DISABLE KEYS */;

INSERT INTO `project_employee` (`ProjectsId`, `EmployeesId`)
VALUES
	(2,3),
	(2,5),
	(2,7),
	(2,3),
	(2,5),
	(2,7),
	(2,3),
	(2,5),
	(2,7);

/*!40000 ALTER TABLE `project_employee` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table projects
# ------------------------------------------------------------

DROP TABLE IF EXISTS `projects`;

CREATE TABLE `projects` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  `description` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `projects` WRITE;
/*!40000 ALTER TABLE `projects` DISABLE KEYS */;

INSERT INTO `projects` (`id`, `name`, `description`)
VALUES
	(2,'string','string'),
	(4,'string','string'),
	(5,'string','string');

/*!40000 ALTER TABLE `projects` ENABLE KEYS */;
UNLOCK TABLES;



/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
