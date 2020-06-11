CREATE DATABASE `CarService` /*!40100 DEFAULT CHARACTER SET latin1 */;



CREATE TABLE `carservice`.`CarBrand` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BrandName` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Client` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`ClientCar` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `CarBrandId` int(11) NOT NULL,
  `Mileage` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_ClientCar_Client_idx` (`ClientId`),
  KEY `fk_ClientCar_CarBrand_idx` (`CarBrandId`),
  CONSTRAINT `fk_ClientCar_CarBrand` FOREIGN KEY (`CarBrandId`) REFERENCES `CarBrand` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_ClientCar_Client` FOREIGN KEY (`ClientId`) REFERENCES `Client` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Employee` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `DateOfStart` date NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`InspectionHours` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ClientId` int(11) NOT NULL,
  `CarId` int(11) NOT NULL,
  `DateTimeOfInspection` date NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_InspectionHours_Client_idx` (`ClientId`),
  KEY `fk_InspectionHours_ClientCars_idx` (`CarId`),
  CONSTRAINT `fk_InspectionHours_ClientCars` FOREIGN KEY (`CarId`) REFERENCES `ClientCar` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_InspectionHours_Client` FOREIGN KEY (`ClientId`) REFERENCES `Client` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;



CREATE TABLE `carservice`.`Schedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ForDate` date NOT NULL,
  `HourBegin` decimal(6,2) NOT NULL,
  `HourEnd` decimal(6,2) NOT NULL,
  `EmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Schedule_Employee_idx` (`EmployeeId`),
  CONSTRAINT `fk_Schedule_1` FOREIGN KEY (`EmployeeId`) REFERENCES `Employee` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;