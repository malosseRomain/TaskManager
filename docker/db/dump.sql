-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: taskmaster
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Comments`
--

DROP TABLE IF EXISTS `Comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Comments` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TaskItemId` int NOT NULL,
  `AuteurId` int NOT NULL,
  `Date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Contenu` text NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `TaskItemId` (`TaskItemId`),
  KEY `AuteurId` (`AuteurId`),
  CONSTRAINT `Comments_ibfk_1` FOREIGN KEY (`TaskItemId`) REFERENCES `TaskItems` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `Comments_ibfk_2` FOREIGN KEY (`AuteurId`) REFERENCES `Persons` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Comments`
--

LOCK TABLES `Comments` WRITE;
/*!40000 ALTER TABLE `Comments` DISABLE KEYS */;
INSERT INTO `Comments` VALUES (1,1,2,'2024-03-02 00:00:00','Les nouvelles images sont prêtes à être uploadées'),(2,1,1,'2024-03-03 00:00:00','J\'ai commencé la mise à jour du contenu'),(3,2,3,'2024-03-06 00:00:00','L\'agenda est en cours de préparation'),(4,3,4,'2024-03-11 00:00:00','Le design du système de notification est validé'),(5,4,5,'2024-03-13 00:00:00','Première passe de revue de code effectuée'),(6,5,1,'2024-03-16 00:00:00','La sauvegarde est planifiée pour demain'),(7,6,2,'2024-03-19 00:00:00','J\'ai commencé la documentation des endpoints principaux');
/*!40000 ALTER TABLE `Comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Persons`
--

DROP TABLE IF EXISTS `Persons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Persons` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  `Prenom` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Persons`
--

LOCK TABLES `Persons` WRITE;
/*!40000 ALTER TABLE `Persons` DISABLE KEYS */;
INSERT INTO `Persons` VALUES (1,'Dupont','Jean','jean.dupont@example.com'),(2,'Martin','Sophie','sophie.martin@example.com'),(3,'Bernard','Pierre','pierre.bernard@example.com'),(4,'Petit','Marie','marie.petit@example.com'),(5,'Robert','Thomas','thomas.robert@example.com');
/*!40000 ALTER TABLE `Persons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SubTasks`
--

DROP TABLE IF EXISTS `SubTasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SubTasks` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TaskItemId` int NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Statut` enum('Afaire','EnCours','Terminee','Annulee') NOT NULL,
  `Echeance` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `TaskItemId` (`TaskItemId`),
  CONSTRAINT `SubTasks_ibfk_1` FOREIGN KEY (`TaskItemId`) REFERENCES `TaskItems` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SubTasks`
--

LOCK TABLES `SubTasks` WRITE;
/*!40000 ALTER TABLE `SubTasks` DISABLE KEYS */;
INSERT INTO `SubTasks` VALUES (1,1,'Mettre à jour les images du slider','Afaire','2024-03-10 00:00:00'),(2,1,'Corriger les liens brisés','Terminee','2024-03-12 00:00:00'),(3,2,'Préparer l\'agenda de la réunion','EnCours','2024-03-15 00:00:00'),(4,2,'Créer la présentation PowerPoint','Afaire','2024-03-18 00:00:00'),(5,3,'Concevoir le système de notification','Afaire','2024-03-15 00:00:00'),(6,3,'Implémenter les notifications par email','Afaire','2024-03-20 00:00:00'),(7,4,'Vérifier les normes de codage','EnCours','2024-03-15 00:00:00'),(8,4,'Tester les cas limites','Afaire','2024-03-17 00:00:00'),(9,5,'Sauvegarder la base actuelle','Terminee','2024-03-20 00:00:00'),(10,5,'Migrer les données','Afaire','2024-03-25 00:00:00'),(11,6,'Documenter les endpoints','Afaire','2024-03-20 00:00:00'),(12,6,'Ajouter des exemples d\'utilisation','Afaire','2024-03-22 00:00:00');
/*!40000 ALTER TABLE `SubTasks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `TaskItemEtiquettes`
--

DROP TABLE IF EXISTS `TaskItemEtiquettes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TaskItemEtiquettes` (
  `TaskItemId` int NOT NULL,
  `Etiquette` varchar(100) NOT NULL,
  PRIMARY KEY (`TaskItemId`,`Etiquette`),
  CONSTRAINT `TaskItemEtiquettes_ibfk_1` FOREIGN KEY (`TaskItemId`) REFERENCES `TaskItems` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `TaskItemEtiquettes`
--

LOCK TABLES `TaskItemEtiquettes` WRITE;
/*!40000 ALTER TABLE `TaskItemEtiquettes` DISABLE KEYS */;
INSERT INTO `TaskItemEtiquettes` VALUES (1,'Frontend'),(1,'Maintenance'),(2,'Client'),(2,'Présentation'),(3,'Backend'),(3,'Nouvelle fonctionnalité'),(4,'Code review'),(4,'Qualité'),(5,'Infrastructure'),(5,'Migration'),(6,'API'),(6,'Documentation');
/*!40000 ALTER TABLE `TaskItemEtiquettes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `TaskItems`
--

DROP TABLE IF EXISTS `TaskItems`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TaskItems` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `DateCreation` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Echeance` datetime DEFAULT NULL,
  `Statut` enum('Afaire','EnCours','Terminee','Annulee') NOT NULL,
  `Priorite` enum('Basse','Moyenne','Haute','Critique') NOT NULL,
  `AuteurId` int NOT NULL,
  `RealisateurId` int NOT NULL,
  `Categorie` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `AuteurId` (`AuteurId`),
  KEY `RealisateurId` (`RealisateurId`),
  CONSTRAINT `TaskItems_ibfk_1` FOREIGN KEY (`AuteurId`) REFERENCES `Persons` (`Id`),
  CONSTRAINT `TaskItems_ibfk_2` FOREIGN KEY (`RealisateurId`) REFERENCES `Persons` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `TaskItems`
--

LOCK TABLES `TaskItems` WRITE;
/*!40000 ALTER TABLE `TaskItems` DISABLE KEYS */;
INSERT INTO `TaskItems` VALUES (1,'Mise à jour du site web','Mettre à jour le contenu et les images du site principal','2024-03-01 00:00:00','2024-03-15 00:00:00','Afaire','Moyenne',1,2,'Développement'),(2,'Préparation réunion client','Préparer la présentation et les documents pour la réunion du 20 mars','2024-03-05 00:00:00','2024-03-19 00:00:00','EnCours','Haute',2,3,'Administratif'),(3,'Développement nouvelle fonctionnalité','Implémenter le système de notifications','2024-03-10 00:00:00','2024-03-25 00:00:00','Afaire','Haute',3,4,'Développement'),(4,'Revue de code','Faire une revue complète du code de la dernière version','2024-03-12 00:00:00','2024-03-18 00:00:00','EnCours','Basse',4,1,'Qualité'),(5,'Migration base de données','Migrer les données vers le nouveau serveur','2024-03-15 00:00:00','2024-03-30 00:00:00','Afaire','Haute',5,3,'Infrastructure'),(6,'Documentation API','Mettre à jour la documentation de l\'API REST','2024-03-18 00:00:00','2024-03-22 00:00:00','Afaire','Moyenne',1,5,'Documentation');
/*!40000 ALTER TABLE `TaskItems` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-09 10:40:20
