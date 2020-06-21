CREATE DATABASE `CarService` /*!40100 DEFAULT CHARACTER SET latin1 */;



CREATE TABLE `CarService`.`CarBrands` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BrandName` varchar(45) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`Clients` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `EmailAddress` varchar(100) NOT NULL,
  `Password` varchar(200) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
	UNIQUE INDEX `ui_Clients_EmailAddress_idx` (`EmailAddress`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`ClientCars` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `CarBrandId` int(11) NOT NULL,
  `LicensePlate` varchar(50) NOT NULL,
  `Mileage` int(11) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `fk_ClientCars_Clients_idx` (`ClientId`),
  KEY `fk_ClientCars_CarBrands_idx` (`CarBrandId`),
  CONSTRAINT `fk_ClientCars_CarBrands` FOREIGN KEY (`CarBrandId`) REFERENCES `CarBrands` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_ClientCars_Clients` FOREIGN KEY (`ClientId`) REFERENCES `Clients` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`EmployeeRoles` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmployeeRoleName` varchar(100) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`Employees` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `EmailAddress` varchar(100) NOT NULL,
  `Password` varchar(200) NOT NULL,
  `DateOfStart` datetime(6) NOT NULL,
  `EmployeeRoleId` int(11) NOT NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
	UNIQUE INDEX `ui_Employees_EmailAddress_idx` (`EmailAddress`),
  KEY `fk_Employees_EmployeeRoles_idx` (`EmployeeRoleId`),
  CONSTRAINT `fk_Employees_EmployeeRoles` FOREIGN KEY (`EmployeeRoleId`) REFERENCES `EmployeeRoles` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`Inspections` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `CarId` int(11) NOT NULL,
  `Mileage` int(11) NOT NULL,
  `DateTimeOfInspection` datetime(6) NOT NULL,
  `Description` varchar(500) NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `fk_Inspections_Clients_idx` (`ClientId`),
  KEY `fk_Inspections_ClientCars_idx` (`CarId`),
  CONSTRAINT `fk_Inspections_ClientCars` FOREIGN KEY (`CarId`) REFERENCES `ClientCars` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_Inspections_Clients` FOREIGN KEY (`ClientId`) REFERENCES `Clients` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`InspectionEmployees` (
  `InspectionId` int(11) NOT NULL,
  `EmployeeId` int(11) NOT NULL,
	UNIQUE INDEX `ui_InspectionEmployees_InspectionId_EmployeeId_idx` (`InspectionId`, `EmployeeId`),
  KEY `fk_InspectionEmployees_Inspections_idx` (`InspectionId`),
  KEY `fk_InspectionEmployees_Employees_idx` (`EmployeeId`),
  CONSTRAINT `fk_InspectionEmployees_Inspections_idx` FOREIGN KEY (`InspectionId`) REFERENCES `Inspections` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_InspectionEmployees_Employees_idx` FOREIGN KEY (`EmployeeId`) REFERENCES `Employees` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`Invoices` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `InspectionId` int(11) NOT NULL,
  `InvoiceDate` datetime(6) NOT NULL,
  `InvoiceSum` decimal(16,4) NOT NULL,
  `Description` varchar(500) NULL,
  `Archived` bit NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `fk_Invoices_Inspections_idx` (`InspectionId`),
  CONSTRAINT `fk_Invoices_Inspections_idx` FOREIGN KEY (`InspectionId`) REFERENCES `Inspections` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `CarService`.`Schedules` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DateBegin` datetime(6) NOT NULL,
  `DateEnd` datetime(6) NOT NULL,
  `EmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Schedules_Employees_idx` (`EmployeeId`),
  CONSTRAINT `fk_Schedules_Employees` FOREIGN KEY (`EmployeeId`) REFERENCES `Employees` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;