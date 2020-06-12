CREATE DATABASE `CarService` /*!40100 DEFAULT CHARACTER SET latin1 */;



CREATE TABLE `carservice`.`CarBrand` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BrandName` varchar(45) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Client` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `EmailAddress` varchar(100) NOT NULL,
  `Password` varchar(200) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
	UNIQUE INDEX `EmailAddress` (`EmailAddress`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`ClientCar` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `CarBrandId` int(11) NOT NULL,
  `LicensePlate` varchar(50) NOT NULL,
  `Mileage` int(11) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `fk_ClientCar_Client_idx` (`ClientId`),
  KEY `fk_ClientCar_CarBrand_idx` (`CarBrandId`),
  CONSTRAINT `fk_ClientCar_CarBrand` FOREIGN KEY (`CarBrandId`) REFERENCES `CarBrand` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_ClientCar_Client` FOREIGN KEY (`ClientId`) REFERENCES `Client` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`EmployeeRoles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmployeeRoleName` varchar(100) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Employee` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `EmailAddress` varchar(100) NOT NULL,
  `Password` varchar(200) NOT NULL,
  `DateOfStart` datetime(6) NOT NULL,
  `EmployeeRoleId` int(11) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
	UNIQUE INDEX `EmailAddress` (`EmailAddress`),
  KEY `fk_Employee_EmployeeRoles_idx` (`EmployeeRoleId`),
  CONSTRAINT `fk_Employee_EmployeeRoles` FOREIGN KEY (`EmployeeRoleId`) REFERENCES `EmployeeRoles` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`InspectionHours` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `CarId` int(11) NOT NULL,
  `Mileage` int(11) NOT NULL,
  `DateTimeOfInspection` datetime(6) NOT NULL,
  `Description` varchar(500) NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `fk_InspectionHours_Client_idx` (`ClientId`),
  KEY `fk_InspectionHours_ClientCars_idx` (`CarId`),
  CONSTRAINT `fk_InspectionHours_ClientCars` FOREIGN KEY (`CarId`) REFERENCES `ClientCar` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_InspectionHours_Client` FOREIGN KEY (`ClientId`) REFERENCES `Client` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Invoices` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `InvoiceId` int(11) NOT NULL,
  `InvoiceDate` datetime(6) NOT NULL,
  `InvoiceSum` decimal(16,4) NOT NULL,
  `Description` varchar(500) NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `fk_Invoices_InspectionHours_idx` (`InvoiceId`),
  CONSTRAINT `fk_Invoices_InspectionHours_idx` FOREIGN KEY (`InvoiceId`) REFERENCES `InspectionHours` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Schedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DateBegin` datetime(6) NOT NULL,
  `DateEnd` datetime(6) NOT NULL,
  `EmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Schedule_Employee_idx` (`EmployeeId`),
  CONSTRAINT `fk_Schedule_Employee` FOREIGN KEY (`EmployeeId`) REFERENCES `Employee` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;