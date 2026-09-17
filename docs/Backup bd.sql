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
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmueble`
--

LOCK TABLES `inmueble` WRITE;
/*!40000 ALTER TABLE `inmueble` DISABLE KEYS */;
INSERT INTO `inmueble` VALUES (1,'Las Heras 426',2,'2',200.00,NULL,1,20.00,'2026-09-02 22:54:45',1,1),(32,'Av. Corrientes 1234, CABA',4,'-34.6037,-58.3816',15000.00,NULL,1,30.00,'2026-09-10 12:42:36',9,1),(33,'Av. Santa Fe 2345, CABA',2,'-34.5950,-58.3920',12000.00,NULL,1,30.00,'2026-09-10 12:42:36',9,2),(34,'Av. Rivadavia 3456, CABA',6,'-34.6100,-58.4000',20000.00,NULL,1,30.00,'2026-09-10 12:42:36',10,1),(35,'Av. Callao 4567, CABA',3,'-34.6050,-58.3950',18000.00,NULL,1,30.00,'2026-09-10 12:42:36',10,3),(36,'Av. Belgrano 5678, CABA',4,'-34.6120,-58.4050',16000.00,NULL,1,30.00,'2026-09-10 12:42:36',11,1),(37,'Av. Independencia 6789, CABA',2,'-34.6180,-58.4100',11000.00,NULL,1,30.00,'2026-09-10 12:42:36',11,2),(38,'Av. San Juan 7890, CABA',5,'-34.6220,-58.4150',19000.00,NULL,1,30.00,'2026-09-10 12:42:36',12,1),(39,'Av. Caseros 8901, CABA',3,'-34.6280,-58.4200',14000.00,NULL,1,30.00,'2026-09-10 12:42:36',12,2),(40,'Av. Brasil 9012, CABA',4,'-34.6330,-58.4250',17000.00,NULL,1,30.00,'2026-09-10 12:42:36',13,1),(41,'Av. Garay 1012, CABA',2,'-34.6380,-58.4300',13000.00,NULL,1,30.00,'2026-09-10 12:42:36',13,3),(42,'Av. Directorio 2345, CABA',3,'-34.6400,-58.4350',14500.00,NULL,1,30.00,'2026-09-10 12:42:36',14,1),(43,'Av. Eva Perón 3456, CABA',5,'-34.6450,-58.4400',21000.00,NULL,1,30.00,'2026-09-10 12:42:36',14,2),(44,'Av. San Martín 4567, CABA',2,'-34.6500,-58.4450',11500.00,NULL,1,30.00,'2026-09-10 12:42:36',15,3),(45,'Av. Cabildo 5678, CABA',4,'-34.6550,-58.4500',16500.00,NULL,1,30.00,'2026-09-10 12:42:36',15,1),(46,'Av. Congreso 6789, CABA',6,'-34.6600,-58.4550',22000.00,NULL,1,30.00,'2026-09-10 12:42:36',16,2),(47,'Av. Monroe 7890, CABA',3,'-34.6650,-58.4600',15500.00,NULL,1,30.00,'2026-09-10 12:42:36',16,1),(48,'Av. del Libertador 8901, CABA',2,'-34.6700,-58.4650',12500.00,NULL,1,30.00,'2026-09-10 12:42:36',17,3),(49,'Av. Figueroa Alcorta 9012, CABA',5,'-34.6750,-58.4700',19500.00,NULL,1,30.00,'2026-09-10 12:42:36',17,1),(50,'Av. Alvear 1012, CABA',4,'-34.6800,-58.4750',18500.00,NULL,0,30.00,'2026-09-10 12:42:36',18,2),(51,'Av. Costanera 1112, CABA',3,'-34.6850,-58.4800',17500.00,NULL,0,30.00,'2026-09-10 12:42:36',18,1),(52,'Calle 123',2,'2222',499.99,NULL,1,50.00,'2026-09-14 20:08:43',15,1),(53,'asdasdas',1,NULL,111.00,'https://img.magnific.com/foto-gratis/acogedor-salon-interior-ventana-panoramica_1262-12322.jpg?semt=ais_hybrid&w=740&q=80',1,11.00,'2026-09-17 12:23:27',1,1);
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
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilino`
--

LOCK TABLES `inquilino` WRITE;
/*!40000 ALTER TABLE `inquilino` DISABLE KEYS */;
INSERT INTO `inquilino` VALUES (1,'11111111','Atilia Dis','ati123@gmail.com','2657332255','Calle falsa 123','2026-08-20 15:02:38'),(2,'12345678','Vati Dis','vatata77@gmail.com','2657155195','Calle falsa 44','2026-08-20 15:03:15'),(4,'30123456','Juan Pérez','juan.perez@email.com','11-5555-1001','Av. Corrientes 1234, CABA','2026-09-10 12:38:30'),(5,'31234567','María González','maria.gonzalez@email.com','11-5555-1002','Av. Santa Fe 2345, CABA','2026-09-10 12:38:30'),(6,'32345678','Carlos Rodríguez','carlos.rodriguez@email.com','11-5555-1003','Av. Rivadavia 3456, CABA','2026-09-10 12:38:30'),(7,'33456789','Ana Martínez','ana.martinez@email.com','11-5555-1004','Av. Callao 4567, CABA','2026-09-10 12:38:30'),(8,'34567890','Luis Fernández','luis.fernandez@email.com','11-5555-1005','Av. Belgrano 5678, CABA','2026-09-10 12:38:30'),(9,'35678901','Laura López','laura.lopez@email.com','11-5555-1006','Av. Independencia 6789, CABA','2026-09-10 12:38:30'),(10,'36789012','Pedro Sánchez','pedro.sanchez@email.com','11-5555-1007','Av. San Juan 7890, CABA','2026-09-10 12:38:30'),(11,'37890123','Sofía Ramírez','sofia.ramirez@email.com','11-5555-1008','Av. Caseros 8901, CABA','2026-09-10 12:38:30'),(12,'38901234','Diego Torres','diego.torres@email.com','11-5555-1009','Av. Brasil 9012, CABA','2026-09-10 12:38:30'),(13,'39012345','Valentina Díaz','valentina.diaz@email.com','11-5555-1010','Av. Garay 1012, CABA','2026-09-10 12:38:30'),(14,'41011111','Martín Acosta','martin.acosta@email.com','11-6666-2001','Av. Rivadavia 2001, CABA','2026-09-10 12:45:39'),(15,'41022222','Lucía Benítez','lucia.benitez@email.com','11-6666-2002','Av. Corrientes 2002, CABA','2026-09-10 12:45:39'),(16,'41033333','Federico Cabrera','federico.cabrera@email.com','11-6666-2003','Av. Santa Fe 2003, CABA','2026-09-10 12:45:39'),(17,'41044444','Camila Duarte','camila.duarte@email.com','11-6666-2004','Av. Callao 2004, CABA','2026-09-10 12:45:39'),(18,'41055555','Nicolás Escobar','nicolas.escobar@email.com','11-6666-2005','Av. Belgrano 2005, CABA','2026-09-10 12:45:39'),(19,'41066666','Florencia Figueroa','florencia.figueroa@email.com','11-6666-2006','Av. Independencia 2006, CABA','2026-09-10 12:45:39'),(20,'41077777','Tomás Gutiérrez','tomas.gutierrez@email.com','11-6666-2007','Av. San Juan 2007, CABA','2026-09-10 12:45:39'),(21,'41088888','Julieta Herrera','julieta.herrera@email.com','11-6666-2008','Av. Caseros 2008, CABA','2026-09-10 12:45:39'),(22,'41099999','Agustín Ibarra','agustin.ibarra@email.com','11-6666-2009','Av. Brasil 2009, CABA','2026-09-10 12:45:39'),(23,'41100000','Milagros Juárez','milagros.juarez@email.com','11-6666-2010','Av. Garay 2010, CABA','2026-09-10 12:45:39'),(24,'41111111','Santiago Krause','santiago.krause@email.com','11-6666-2011','Av. Directorio 2011, CABA','2026-09-10 12:45:39'),(25,'41122222','Valentina Ledesma','valentina.ledesma@email.com','11-6666-2012','Av. Eva Perón 2012, CABA','2026-09-10 12:45:39'),(26,'41133333','Matías Molina','matias.molina@email.com','11-6666-2013','Av. San Martín 2013, CABA','2026-09-10 12:45:39'),(27,'41144444','Catalina Navarro','catalina.navarro@email.com','11-6666-2014','Av. Cabildo 2014, CABA','2026-09-10 12:45:39'),(28,'41155555','Joaquín Olivera','joaquin.olivera@email.com','11-6666-2015','Av. Congreso 2015, CABA','2026-09-10 12:45:39'),(29,'41166666','Renata Peralta','renata.peralta@email.com','11-6666-2016','Av. Monroe 2016, CABA','2026-09-10 12:45:39'),(30,'41177777','Facundo Quiroga','facundo.quiroga@email.com','11-6666-2017','Av. del Libertador 2017, CABA','2026-09-10 12:45:39'),(31,'41188888','Emilia Ríos','emilia.rios@email.com','11-6666-2018','Av. Figueroa Alcorta 2018, CABA','2026-09-10 12:45:39'),(32,'41199999','Bautista Sosa','bautista.sosa@email.com','11-6666-2019','Av. Alvear 2019, CABA','2026-09-10 12:45:39'),(33,'41200000','Delfina Toledo','delfina.toledo@email.com','11-6666-2020','Av. Costanera 2020, CABA','2026-09-10 12:45:39');
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
  `estado` tinyint DEFAULT '1',
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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
INSERT INTO `pago` VALUES (3,'Seña','2026-09-16',20000.00,1,'2026-09-16 13:27:22',NULL,23,3,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietario`
--

LOCK TABLES `propietario` WRITE;
/*!40000 ALTER TABLE `propietario` DISABLE KEYS */;
INSERT INTO `propietario` VALUES (1,'Fabrizio DIsidoro','41084990','fabrizioandres98@gmail.com','2657586587','Av. 25 de Mayo 546','2026-08-20 13:23:06'),(9,'Roberto Gómez','20111111','roberto.gomez@email.com','11-4444-1001','Av. Libertador 1000, CABA','2026-09-10 12:40:31'),(10,'Patricia Fernández','20222222','patricia.fernandez@email.com','11-4444-1002','Av. Alvear 2000, CABA','2026-09-10 12:40:31'),(11,'Héctor Martínez','20333333','hector.martinez@email.com','11-4444-1003','Av. del Libertador 3000, CABA','2026-09-10 12:40:31'),(12,'Silvia López','20444444','silvia.lopez@email.com','11-4444-1004','Av. Figueroa Alcorta 4000, CABA','2026-09-10 12:40:31'),(13,'Daniel Ramírez','20555555','daniel.ramirez@email.com','11-4444-1005','Av. Costanera 5000, CABA','2026-09-10 12:40:31'),(14,'Alejandra Torres','20666666','alejandra.torres@email.com','11-4444-1006','Av. Cabildo 6000, CABA','2026-09-10 12:40:31'),(15,'Fernando Ruiz','20777777','fernando.ruiz@email.com','11-4444-1007','Av. Directorio 7000, CABA','2026-09-10 12:40:31'),(16,'Claudia Vega','20888888','claudia.vega@email.com','11-4444-1008','Av. Rivadavia 8000, CABA','2026-09-10 12:40:31'),(17,'Gustavo Mendoza','20999999','gustavo.mendoza@email.com','11-4444-1009','Av. Eva Perón 9000, CABA','2026-09-10 12:40:31'),(18,'Verónica Castro','30000000','veronica.castro@email.com','11-4444-1010','Av. San Martín 10000, CABA','2026-09-10 12:40:31');
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
  `fecha_fin_original` date DEFAULT NULL,
  `monto_por_dia` decimal(10,2) NOT NULL,
  `estado` enum('Activa','Finalizada','Cancelada') DEFAULT 'Activa',
  `fecha_creacion` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_terminacion` datetime DEFAULT NULL,
  `multa_aplicada` decimal(10,2) DEFAULT '0.00',
  `id_inquilino` int NOT NULL,
  `id_inmueble` int NOT NULL,
  `id_usuario_creador` int DEFAULT NULL,
  `id_usuario_terminacion` int DEFAULT NULL,
  PRIMARY KEY (`id_reserva`),
  KEY `fk_reserva_inquilino` (`id_inquilino`),
  KEY `fk_reserva_inmueble` (`id_inmueble`),
  KEY `fk_reserva_usuario_creador` (`id_usuario_creador`),
  KEY `fk_reserva_usuario_terminacion` (`id_usuario_terminacion`),
  KEY `id_usuario_creador_idx` (`id_usuario_creador`,`id_usuario_terminacion`),
  KEY `idx_reserva_inquilino` (`id_inquilino`),
  KEY `idx_reserva_inmueble` (`id_inmueble`),
  KEY `idx_reserva_usuario_creador` (`id_usuario_creador`),
  KEY `idx_reserva_usuario_terminacion` (`id_usuario_terminacion`),
  CONSTRAINT `id_inmueble` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id_inmueble`),
  CONSTRAINT `id_inquilino` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id_inquilino`),
  CONSTRAINT `id_usuario_creador` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `id_usuario_terminacion` FOREIGN KEY (`id_usuario_terminacion`) REFERENCES `usuario` (`id_usuario`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reserva`
--

LOCK TABLES `reserva` WRITE;
/*!40000 ALTER TABLE `reserva` DISABLE KEYS */;
INSERT INTO `reserva` VALUES (22,'2026-09-16','2026-09-30',NULL,20000.00,'Finalizada','2026-09-16 13:26:25','2026-09-16 00:00:00',140000.00,4,34,3,3),(23,'2026-10-01','2026-10-06',NULL,20000.00,'Activa','2026-09-16 13:27:05',NULL,0.00,4,34,3,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipo_inmueble`
--

LOCK TABLES `tipo_inmueble` WRITE;
/*!40000 ALTER TABLE `tipo_inmueble` DISABLE KEYS */;
INSERT INTO `tipo_inmueble` VALUES (1,'Departamento','Unidad en edificio (piso).'),(2,'Casa','Propiedad individual con terreno.'),(3,'Monoambiente','Un solo ambiente (living/dormitorio juntos).');
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
  `rol` enum('Administrador','Empleado') NOT NULL,
  `nombre_completo` varchar(150) NOT NULL,
  `avatar` varchar(255) DEFAULT NULL,
  `fecha_creacion` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_ultima_modificacion` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (3,'admin@inmobiliaria.com','$2a$11$NyY7a2.cJBWoUKz45GJMVuGlqjK34nUMM.DZk7kbp57eyXnfiKhEW','Administrador','Admin Principal',NULL,'2026-09-08 12:42:45','2026-09-17 00:48:00'),(8,'empleadouno@inmobiliaria.com','$2a$11$Se9i8SIPXdjMdsbtQipoW.zxwak.x57XKlXrJhif82yFZj5fR2ikW','Empleado','Empleado Uno',NULL,'2026-09-17 18:30:09','2026-09-17 18:30:08');
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

-- Dump completed on 2026-09-17 20:27:10
