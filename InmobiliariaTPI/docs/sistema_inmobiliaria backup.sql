-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: sistema_inmobiliaria
-- ------------------------------------------------------
-- Server version	8.0.46

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

-- Crear la base de datos si no existe
CREATE DATABASE IF NOT EXISTS sistema_inmobiliaria;
USE sistema_inmobiliaria;
--
-- Table structure for table `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmueble` (
  `id_inmueble` int NOT NULL AUTO_INCREMENT,
  `direccion` varchar(200) NOT NULL,
  `cupo_maximo` int NOT NULL,
  `coordenadas` varchar(100) DEFAULT NULL,
  `precio_por_dia` decimal(10,2) NOT NULL,
  `imagen_portada` varchar(255) DEFAULT NULL,
  `disponible` tinyint(1) DEFAULT '1',
  `porcentaje_reserva` decimal(5,2) DEFAULT '0.00',
  `fecha_creacion` datetime DEFAULT CURRENT_TIMESTAMP,
  `id_propietario` int NOT NULL,
  `id_tipo_inmueble` int NOT NULL,
  PRIMARY KEY (`id_inmueble`),
  KEY `fk_inmueble_propietario` (`id_propietario`),
  KEY `fk_inmueble_tipo` (`id_tipo_inmueble`),
  CONSTRAINT `fk_inmueble_propietario` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id_propietario`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_inmueble_tipo` FOREIGN KEY (`id_tipo_inmueble`) REFERENCES `tipo_inmueble` (`id_tipo_inmueble`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
/*!40000 ALTER TABLE `inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilino` (
  `id_inquilino` int NOT NULL AUTO_INCREMENT,
  `dni` varchar(20) NOT NULL,
  `nombre_completo` varchar(150) NOT NULL,
  `email` varchar(150) NOT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `direccion` varchar(200) DEFAULT NULL,
  `fecha_registro` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_inquilino`),
  UNIQUE KEY `dni` (`dni`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (1,'11111111','Atilia Dis','ati123@gmail.com','2657332255','Calle falsa 123','2026-08-20 15:02:38'),(2,'12345678','Vati Dis','vatata77@gmail.com','2657155195','Calle falsa 44','2026-08-20 15:03:15');
/*!40000 ALTER TABLE `inquilino` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pago` (
  `id_pago` int NOT NULL AUTO_INCREMENT,
  `concepto` varchar(200) NOT NULL,
  `fecha_pago` date NOT NULL,
  `importe` decimal(10,2) NOT NULL,
  `estado` enum('ACTIVO','ANULADO') DEFAULT 'ACTIVO',
  `fecha_creacion` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_anulacion` datetime DEFAULT NULL,
  `id_reserva` int NOT NULL,
  `id_usuario_creador` int NOT NULL,
  `id_usuario_anulacion` int DEFAULT NULL,
  PRIMARY KEY (`id_pago`),
  KEY `fk_pago_reserva` (`id_reserva`),
  KEY `fk_pago_usuario_creador` (`id_usuario_creador`),
  KEY `fk_pago_usuario_anulacion` (`id_usuario_anulacion`),
  CONSTRAINT `fk_pago_reserva` FOREIGN KEY (`id_reserva`) REFERENCES `reserva` (`id_reserva`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_pago_usuario_anulacion` FOREIGN KEY (`id_usuario_anulacion`) REFERENCES `usuario` (`id_usuario`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_pago_usuario_creador` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietario`
--

DROP TABLE IF EXISTS `propietario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietario` (
  `id_propietario` int NOT NULL AUTO_INCREMENT,
  `nombre_completo` varchar(150) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `email` varchar(150) NOT NULL,
  `telefono` varchar(50) DEFAULT NULL,
  `direccion` varchar(200) DEFAULT NULL,
  `fecha_registro` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_propietario`),
  UNIQUE KEY `dni` (`dni`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (1,'Fabrizio DIsidoro','41084990','fabrizioandres98@gmail.com','2657586587','Av. 25 de Mayo 546','2026-08-20 13:23:06'),(2,'Eugenia DIsidoro','40183443','eugeniab@gmail.com','2657585832','Dr. Tallaferro 350','2026-08-20 15:00:11');
/*!40000 ALTER TABLE `propietario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reserva`
--

DROP TABLE IF EXISTS `reserva`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reserva` (
  `id_reserva` int NOT NULL AUTO_INCREMENT,
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date NOT NULL,
  `fecha_fin_original` date NOT NULL,
  `monto_por_dia` decimal(10,2) NOT NULL,
  `estado` enum('ACTIVA','FINALIZADA','CANCELADA') DEFAULT 'ACTIVA',
  `fecha_creacion` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_terminacion` datetime DEFAULT NULL,
  `multa_aplicada` decimal(10,2) DEFAULT '0.00',
  `id_inquilino` int NOT NULL,
  `id_inmueble` int NOT NULL,
  `id_usuario_creador` int NOT NULL,
  `id_usuario_terminacion` int DEFAULT NULL,
  PRIMARY KEY (`id_reserva`),
  KEY `fk_reserva_inquilino` (`id_inquilino`),
  KEY `fk_reserva_inmueble` (`id_inmueble`),
  KEY `fk_reserva_usuario_creador` (`id_usuario_creador`),
  KEY `fk_reserva_usuario_terminacion` (`id_usuario_terminacion`),
  CONSTRAINT `fk_reserva_inmueble` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id_inmueble`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_reserva_inquilino` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id_inquilino`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_reserva_usuario_creador` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuario` (`id_usuario`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_reserva_usuario_terminacion` FOREIGN KEY (`id_usuario_terminacion`) REFERENCES `usuario` (`id_usuario`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reserva`
--

LOCK TABLES `reserva` WRITE;
/*!40000 ALTER TABLE `reserva` DISABLE KEYS */;
/*!40000 ALTER TABLE `reserva` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipo_inmueble`
--

DROP TABLE IF EXISTS `tipo_inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipo_inmueble` (
  `id_tipo_inmueble` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `descripcion` text,
  PRIMARY KEY (`id_tipo_inmueble`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_inmueble`
--

LOCK TABLES `tipo_inmueble` WRITE;
/*!40000 ALTER TABLE `tipo_inmueble` DISABLE KEYS */;
/*!40000 ALTER TABLE `tipo_inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `email` varchar(150) NOT NULL,
  `password` varchar(255) NOT NULL,
  `rol` enum('ADMINISTRADOR','EMPLEADO') NOT NULL,
  `nombre_completo` varchar(150) NOT NULL,
  `avatar` varchar(255) DEFAULT NULL,
  `fecha_creacion` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_ultima_modificacion` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-08-20 15:11:09
