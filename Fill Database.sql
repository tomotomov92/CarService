INSERT INTO `CarService`.`CarBrands` (`BrandName`, `Archived`)
VALUES
('Alfa Romeo', 0),
('Aston Martin', 1),
('Audi', 0),
('Bentley', 0),
('Benz', 1),
('BMW', 0),
('Bugatti', 0),
('Cadillac', 0),
('Chevrolet', 0),
('Chrysler', 0),
('Citroen', 0),
('Corvette', 0),
('DAF', 1),
('Dacia', 0),
('Daewoo', 1),
('Daihatsu', 0),
('Datsun', 1),
('De Lorean', 1),
('Dino', 0),
('Dodge', 0),
('Farboud', 1),
('Ferrari', 0),
('Fiat', 0),
('Ford', 0),
('Honda', 0),
('Hummer', 0),
('Hyundai', 0),
('Jaguar', 0),
('Jeep', 0),
('KIA', 0),
('Koenigsegg', 1),
('Lada', 0),
('Lamborghini', 0),
('Lancia', 0),
('Land Rover', 0),
('Lexus', 0),
('Ligier', 1),
('Lincoln', 1),
('Lotus', 0),
('Martini', 1),
('Maserati', 0),
('Maybach', 0),
('Mazda', 0),
('McLaren', 0),
('Mercedes', 0),
('Mercedes-Benz', 0),
('Mini', 0),
('Mitsubishi', 0),
('Nissan', 0),
('Noble', 0),
('Opel', 0),
('Peugeot', 0),
('Pontiac', 0),
('Porsche', 0),
('Renault', 0),
('Rolls-Royce', 0),
('Rover', 0),
('Saab', 0),
('Seat', 0),
('Skoda', 0),
('Smart', 0),
('Spyker', 0),
('Subaru', 0),
('Suzuki', 0),
('Toyota', 0),
('Vauxhall', 0),
('Volkswagen', 0),
('Volvo', 0);
/*END*/


INSERT INTO `CarService`.`EmployeeRoles` (`EmployeeRoleName`)
VALUES
('Owner'),
('Mechanic'),
('Customer Support');
/*END*/


INSERT INTO `CarService`.`Employees` (`FirstName`, `LastName`, `EmailAddress`, `Password`, `DateOfStart`, `EmployeeRoleId`, `RequirePasswordChange`, `Archived`)
VALUES
('Tomo', 'Tomov', 't.tomov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2020-01-01', 1, 0, 0),
('Staiko', 'Metodiev', 's.metodiev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2019-03-15', 2, 0, 0),
('Atanas', 'Atanasov', 'a.atanasov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2000-12-25', 2, 0, 0),
('Nikolay', 'Stoyanov', 'n.stoyanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2012-12-12', 2, 0, 0),
('Bojana', 'Krusteva', 'b.krusteva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2020-05-30', 2, 0, 0),
('Velina', 'Spasova', 'v.spasova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2008-10-10', 2, 0, 1),
('Mihaela', 'Kalcheva', 'm.kalcheva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2007-09-27', 2, 0, 1),
('Spasimir', 'Kirilov', 's.kirilov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2013-03-03', 2, 0, 1),
('Plamen', 'Panaiotov', 'p.panaiotov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2015-08-20', 2, 0, 0),
('Dafina', 'Marinova', 'd.marinova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2015-08-20', 2, 0, 1),
('Dimitar', 'Iliev', 'd.iliev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2020-04-04', 2, 0, 1),
('Venelin', 'Dimitrov', 'v.dimitrov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2016-11-11', 2, 0, 0),
('Bojidar', 'Stefanov', 'b.stefanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2019-12-30', 2, 0, 0),
('Kaloyan', 'Yanev', 'k.yanev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2009-07-19', 2, 0, 0),
('Georgi', 'Ivandjikov', 'g.ivandjikov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2009-07-06', 2, 0, 0),
('Veselin', 'Nikolov', 'v.nikolov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2010-06-06', 2, 0, 0),
('Veselina', 'Veleva', 'v.veleva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2011-06-22', 2, 0, 0),
('Kameliya', 'Roynova', 'k.roynova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2020-02-24', 3, 0, 1),
('Nikol', 'Buteva', 'n.buteva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2008-10-18', 3, 0, 0),
('Teodora', 'Mandieva', 't.mandieva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2013-06-20', 3, 0, 0),
('S', 'Ivanov', 's.ivanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', '2013-06-20', 3, 0, 0);
/*END*/


INSERT INTO `CarService`.`Schedules` (`DateBegin`, `DateEnd`, `EmployeeId`)
VALUES
('2020-01-12 08:30:00.000000', '2020-01-12 17:30:00.000000', 1),
('2020-01-30 08:30:00.000000', '2020-01-30 17:30:00.000000', 2),
('2020-01-19 08:30:00.000000', '2020-01-19 17:30:00.000000', 3),
('2020-02-06 08:30:00.000000', '2020-02-06 17:30:00.000000', 4),
('2020-02-20 08:30:00.000000', '2020-02-20 17:30:00.000000', 5),
('2020-03-18 08:30:00.000000', '2020-03-18 17:30:00.000000', 9),
('2020-04-16 08:30:00.000000', '2020-04-16 17:30:00.000000', 10),
('2020-04-11 08:30:00.000000', '2020-04-11 17:30:00.000000', 11),
('2020-04-21 08:30:00.000000', '2020-04-21 17:30:00.000000', 12),
('2020-05-22 08:30:00.000000', '2020-05-22 17:30:00.000000', 13),
('2020-05-25 08:30:00.000000', '2020-05-25 17:30:00.000000', 14),
('2020-05-05 08:30:00.000000', '2020-05-05 17:30:00.000000', 15),
('2020-06-08 08:30:00.000000', '2020-06-08 17:30:00.000000', 16),
('2020-06-01 08:30:00.000000', '2020-06-01 17:30:00.000000', 17),
('2020-01-22 08:30:00.000000', '2020-01-22 17:30:00.000000', 19),
('2020-01-29 08:30:00.000000', '2020-01-29 17:30:00.000000', 20),
('2020-01-16 08:30:00.000000', '2020-01-16 17:30:00.000000', 1),
('2020-02-13 08:30:00.000000', '2020-02-13 17:30:00.000000', 2),
('2020-02-18 08:30:00.000000', '2020-02-18 17:30:00.000000', 3),
('2020-02-25 08:30:00.000000', '2020-02-25 17:30:00.000000', 4),
('2020-03-22 08:30:00.000000', '2020-03-22 17:30:00.000000', 5),
('2020-04-22 08:30:00.000000', '2020-04-22 17:30:00.000000', 9),
('2020-04-17 08:30:00.000000', '2020-04-17 17:30:00.000000', 10);
/*END*/


INSERT INTO `CarService`.`Clients` (`FirstName`, `LastName`, `EmailAddress`, `Password`, `RequirePasswordChange`, `Activated`, `Archived`)
VALUES
('Vasil', 'Yanev', 'v.yanev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Tomo', 'Tomov', 't.tomov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Zlatelina', 'Kazakova', 'z.kazakova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Mirela', 'Bineva', 'm.bineva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Rusi', 'Lambov', 'r.lambov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Georgi', 'Ivanov', 'g.ivanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Spas', 'Atanasov', 's.atanasov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Radoslav', 'Hristov', 'r.hristov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Ioana', 'Iosifova', 'i.iosifova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Nikolina', 'Spasova', 'n.spasova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Elena', 'Konstantinova', 'e.konstantinova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Elitsa', 'Leontieva', 'e.leontieva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Mariya', 'Mihailova', 'm.mihailova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Ralitsa', 'Bojkova', 'r.bojkova@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Teodora', 'Maneva', 't.maneva@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Martin', 'Ivanov', 'm.ivanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Tihomir', 'Gospodinov', 't.gospodinov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Yasen', 'Georgiev', 'y.georgiev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Hristo', 'Prodanov', 'h.prodanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Milko', 'Kalaidjiev', 'm.kalaidjiev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('Peter', 'Velev', 'p.velev@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0),
('S', 'Ivanov', 's.ivanov@email.com', 'GV9i7dQKHZsMZAcQRZbWwp4rQI7MHMlk6AM3aRerH9A=', 0, 1, 0);
/*END*/


INSERT INTO `CarService`.`ClientCars` (`ClientId`, `CarBrandId`, `LicensePlate`, `Mileage`, `Archived`)
VALUES
(1, 1, 'HP16-WHR', 100000, 0),
(2, 35, 'HV58-SIM', 39000, 0),
(3, 44, 'JY26-XHR', 2000, 0),
(4, 12, 'PA32-GNN', 4000, 0),
(5, 3, 'FJ73-VSZ', 55000, 0),
(6, 65, 'RF36-GVZ', 99999, 0),
(7, 55, 'UR19-UJY', 65000, 0),
(8, 7, 'EY86-GYA', 546794, 0),
(9, 12, 'ZZ22-UZP', 547950, 0),
(10, 4, 'FR62-AXU', 238501, 0),
(11, 66, 'UX55-TKS', 45680, 0),
(12, 54, 'FR11-STU', 20000, 0),
(13, 43, 'WE61-ALV', 456780, 0),
(14, 50, 'RC05-QLQ', 123456, 0),
(15, 14, 'EL51-EQA', 654321, 0),
(16, 57, 'QA91-WQL', 1000, 0),
(17, 9, 'GQ45-WEF', 60, 0),
(18, 10, 'PZ56-NJD', 500000, 0),
(19, 11, 'LU88-JHO', 689, 0),
(20, 44, 'LN94-LIY', 436, 0),
(21, 66, 'ZY10-WSB', 190, 0),
(1, 33, 'LU33-ICI', 777, 0),
(2, 22, 'GP93-MBL', 546, 0),
(3, 68, 'TV25-HUK', 1000, 0),
(4, 32, 'GV08-RUS', 5000, 0),
(5, 39, 'HF32-RPF', 64438, 0),
(6, 19, 'CE83-YIV', 94836, 0),
(7, 20, 'CL99-EYG', 54986, 0),
(8, 47, 'FS38-SWS', 41599, 0),
(9, 56, 'ZU43-VHZ', 650763, 0),
(10, 30, 'DV93-WGG', 400954, 0),
(11, 1, 'XP05-DUB', 200, 0),
(12, 4, 'AG02-AFZ', 70, 0),
(13, 7, 'FX59-PQP', 100, 0),
(14, 8, 'UB56-VJW', 500, 0),
(15, 39, 'NF19-NED', 300000, 0),
(16, 24, 'DL38-BEH', 43578, 0),
(17, 68, 'RO30-MOK', 10293, 0),
(18, 60, 'HY98-BTP', 98654, 0),
(19, 43, 'QC71-CPH', 135790, 0);
/*END*/


INSERT INTO `CarService`.`Inspections` (`ClientID`, `CarID`, `Mileage`, `DateTimeOfInspection`, `Archived`)
VALUES
(1, 1, 100000, '2020-01-12', 0),
(2, 2, 39000, '2020-01-30', 0),
(3, 3, 2000, '2020-01-19', 0),
(4, 4, 4000, '2020-02-06', 0),
(5, 5, 55000, '2020-02-20', 0),
(6, 6, 99999, '2020-02-29', 0),
(7, 7, 65000, '2020-03-23', 0),
(8, 8, 546794, '2020-03-27', 0),
(9, 9, 547950, '2020-03-18', 0),
(10, 10, 238501, '2020-04-16', 0),
(11, 11, 45680, '2020-04-11', 0),
(12, 12, 20000, '2020-04-21', 0),
(13, 13, 456780, '2020-05-22', 0),
(14, 14, 123456, '2020-05-25', 0),
(15, 15, 654321, '2020-05-05', 0),
(16, 16, 1000, '2020-06-08', 0),
(17, 17, 60, '2020-06-01', 0),
(18, 18, 500000, '2020-06-30', 0),
(19, 19, 689, '2020-01-22', 0),
(20, 20, 436, '2020-01-29', 0),
(21, 21, 190, '2020-01-16', 0),
(1, 22, 777, '2020-02-13', 0),
(2, 23, 546, '2020-02-18', 0),
(3, 24, 1000, '2020-02-25', 0),
(4, 25, 5000, '2020-03-22', 0),
(5, 26, 64438, '2020-03-28', 0),
(6, 27, 94836, '2020-03-15', 0),
(7, 28, 54986, '2020-04-30', 0),
(8, 29, 41599, '2020-04-22', 0),
(9, 30, 650763, '2020-04-17', 0);
/*END*/


INSERT INTO `CarService`.`InspectionEmployees` (`InspectionId`, `EmployeeId`)
VALUES
(1, 2),
(1, 9),
(1, 5),
(4, 3),
(4, 12),
(9, 4),
(9, 2),
(9, 9),
(9, 16),
(13, 15),
(13, 16),
(4, 16);
/*END*/


INSERT INTO `CarService`.`Invoices` (`InspectionId`, `InvoiceDate`, `InvoiceSum`, `Description`, `Archived`)
VALUES
(1, '2020-01-12', 13.6, 'Invoice for Inspection 1', 0),
(4, '2020-01-30', 505.88, 'The brakes needed change', 0),
(9, '2020-01-19', 69.66, NULL, 0),
(13, '2020-02-06', 1333.61, NULL, 0),
(4, '2020-02-20', 2213.16, NULL, 0);
/*END*/