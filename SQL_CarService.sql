CREATE DATABASE `CarService` /*!40100 DEFAULT CHARACTER SET latin1 */;
/*END*/

CREATE TABLE `CarService`.`CarBrand` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `BrandName` varchar(45) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `CarService`.`CarBrand` (`BrandName`)
VALUES
('Alfa Romeo'),
('Aston Martin'),
('Audi'),
('Bentley'),
('Benz'),
('BMW'),
('Bugatti'),
('Cadillac'),
('Chevrolet'),
('Chrysler'),
('Citroen'),
('Corvette'),
('DAF'),
('Dacia'),
('Daewoo'),
('Daihatsu'),
('Datsun'),
('De Lorean'),
('Dino'),
('Dodge'),
('Farboud'),
('Ferrari'),
('Fiat'),
('Ford'),
('Honda'),
('Hummer'),
('Hyundai'),
('Jaguar'),
('Jeep'),
('KIA'),
('Koenigsegg'),
('Lada'),
('Lamborghini'),
('Lancia'),
('Land Rover'),
('Lexus'),
('Ligier'),
('Lincoln'),
('Lotus'),
('Martini'),
('Maserati'),
('Maybach'),
('Mazda'),
('McLaren'),
('Mercedes'),
('Mercedes-Benz'),
('Mini'),
('Mitsubishi'),
('Nissan'),
('Noble'),
('Opel'),
('Peugeot'),
('Pontiac'),
('Porsche'),
('Renault'),
('Rolls-Royce'),
('Rover'),
('Saab'),
('Seat'),
('Skoda'),
('Smart'),
('Spyker'),
('Subaru'),
('Suzuki'),
('Toyota'),
('Vauxhall'),
('Volkswagen'),
('Volvo');
/*END*/

CREATE TABLE `CarService`.`Client` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `CarService`.`Client` (`FirstName`, `LastName`)
VALUES
('Vasil', 'Yanev'),
('Tomo', 'Tomov'),
('Zlatelina', 'Kazakova'),
('Mirela', 'Bineva'),
('Rusi', 'Lambov'),
('Georgi', 'Ivanov'),
('Spas', 'Atanasov'),
('Radoslav', 'Hristov'),
('Ioana', 'Iosifova'),
('Nikolina', 'Spasova'),
('Elena', 'Konstantinova'),
('Elitsa', 'Leontieva'),
('Mariya', 'Mihailova'),
('Ralitsa', 'Bojkova'),
('Teodora', 'Maneva'),
('Martin', 'Ivanov'),
('Tihomir', 'Gospodinov'),
('Yasen', 'Georgiev'),
('Hristo', 'Prodanov'),
('Milko', 'Kalaidjiev'),
('Peter', 'Velev');
/*END*/

CREATE TABLE `CarService`.`ClientCar` (
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

INSERT INTO `CarService`.`ClientCar` (`ClientId`, `CarBrandId`, `Mileage`)
VALUES
(1, 1, 100000),
(2, 35, 39000),
(3, 44, 2000),
(4, 12, 4000),
(5, 3, 55000),
(6, 65, 99999),
(7, 55, 65000),
(8, 7, 546794),
(9, 12, 547950),
(10, 4, 238501),
(11, 66, 45680),
(12, 54, 20000),
(13, 43, 456780),
(14, 50, 123456),
(15, 14, 654321),
(16, 57, 1000),
(17, 9, 60),
(18, 10, 500000),
(19, 11, 689),
(20, 44, 436),
(21, 66, 190),
(1, 33, 777),
(2, 22, 546),
(3, 68, 1000),
(4, 32, 5000),
(5, 39, 64438),
(6, 19, 94836),
(7, 20, 54986),
(8, 47, 41599),
(9, 56, 650763),
(10, 30, 400954),
(11, 1, 200),
(12, 4, 70),
(13, 7, 100),
(14, 8, 500),
(15, 39, 300000),
(16, 24, 43578),
(17, 68, 10293),
(18, 60, 98654),
(19, 43, 135790);
/*END*/

CREATE TABLE `CarService`.`Employee` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `DateOfStart` date NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `CarService`.`Employee` (`FirstName`, `LastName`, `DateOfStart`)
VALUES
('Spas', 'Ivanov', '2020-01-01'),
('Staiko', 'Metodiev', '2019-03-15'),
('Atanas', 'Atanasov', '2000-12-25'),
('Nikolay', 'Stoyanov', '2012-12-12'),
('Bojana', 'Krusteva', '2020-05-30'),
('Velina', 'Spasova', '2008-10-10'),
('Mihaela', 'Kalcheva', '2007-09-27'),
('Spasimir', 'Kirilov', '2013-03-03'),
('Plamen', 'Panaiotov', '2015-08-20'),
('Dafina', 'Marinova', '2015-08-20'),
('Dimitar', 'Iliev', '2020-04-04'),
('Venelin', 'Dimitrov', '2016-11-11'),
('Bojidar', 'Stefanov', '2019-12-30'),
('Kaloyan', 'Yanev', '2009-07-19'),
('Georgi', 'Ivandjikov', '2009-07-06'),
('Veselin', 'Nikolov', '2010-06-06'),
('Veselina', 'Veleva', '2011-06-22'),
('Kameliya', 'Roynova', '2020-02-24'),
('Nikol', 'Buteva', '2008-10-18'),
('Teodora', 'Mandieva', '2013-06-20');
/*END*/

CREATE TABLE `CarService`.`InspectionHours` (
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

INSERT INTO `CarService`.`InspectionHours` (`ClientID`, `CarID`, `DateTimeOfInspection`)
VALUES
(1, 1, '2020-01-12'),
(2, 2, '2020-01-30'),
(3, 3, '2020-01-19'),
(4, 4, '2020-02-06'),
(5, 5, '2020-02-20'),
(6, 6, '2020-02-29'),
(7, 7, '2020-03-23'),
(8, 8, '2020-03-27'),
(9, 9, '2020-03-18'),
(10, 10, '2020-04-16'),
(11, 11, '2020-04-11'),
(12, 12, '2020-04-21'),
(13, 13, '2020-05-22'),
(14, 14, '2020-05-25'),
(15, 15, '2020-05-05'),
(16, 16, '2020-06-08'),
(17, 17, '2020-06-01'),
(18, 18, '2020-06-30'),
(19, 19, '2020-01-22'),
(20, 20, '2020-01-29'),
(21, 21, '2020-01-16'),
(1, 22, '2020-02-13'),
(2, 23, '2020-02-18'),
(3, 24, '2020-02-25'),
(4, 25, '2020-03-22'),
(5, 26, '2020-03-28'),
(6, 27, '2020-03-15'),
(7, 28, '2020-04-30'),
(8, 29, '2020-04-22'),
(9, 30, '2020-04-17');
/*END*/

CREATE TABLE `CarService`.`Schedule` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ForDate` date NOT NULL,
  `HourBegin` decimal NOT NULL,
  `HourEnd` decimal NOT NULL,
  `EmployeeId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_Schedule_Employee_idx` (`EmployeeId`),
  CONSTRAINT `fk_Schedule_1` FOREIGN KEY (`EmployeeId`) REFERENCES `Employee` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `CarService`.`Schedule` (`ForDate`, `HourBegin`, `HourEnd`, `EmployeeId`)
VALUES
('2020-01-12', 8.3, 17.3, 1),
('2020-01-30', 8.3, 17.3, 2),
('2020-01-19', 8.3, 17.3, 3),
('2020-02-06', 8.3, 17.3, 4),
('2020-02-20', 8.3, 17.3, 5),
('2020-02-29', 8.3, 17.3, 6),
('2020-03-23', 8.3, 17.3, 7),
('2020-03-27', 8.3, 17.3, 8),
('2020-03-18', 8.3, 17.3, 9),
('2020-04-16', 8.3, 17.3, 10),
('2020-04-11', 8.3, 17.3, 11),
('2020-04-21', 8.3, 17.3, 12),
('2020-05-22', 8.3, 17.3, 13),
('2020-05-25', 8.3, 17.3, 14),
('2020-05-05', 8.3, 17.3, 15),
('2020-06-08', 8.3, 17.3, 16),
('2020-06-01', 8.3, 17.3, 17),
('2020-06-30', 8.3, 17.3, 18),
('2020-01-22', 8.3, 17.3, 19),
('2020-01-29', 8.3, 17.3, 20),
('2020-01-16', 8.3, 17.3, 1),
('2020-02-13', 8.3, 17.3, 2),
('2020-02-18', 8.3, 17.3, 3),
('2020-02-25', 8.3, 17.3, 4),
('2020-03-22', 8.3, 17.3, 5),
('2020-03-28', 8.3, 17.3, 6),
('2020-03-15', 8.3, 17.3, 7),
('2020-04-30', 8.3, 17.3, 8),
('2020-04-22', 8.3, 17.3, 9),
('2020-04-17', 8.3, 17.3, 10);