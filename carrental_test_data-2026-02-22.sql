-- ============================================================================
-- CarRental System - Complete Test Data
-- Database: mr_rent
-- Purpose: Generate realistic test data for car rental system testing
-- NOTE: This script DELETES existing data and inserts fresh test data
-- ============================================================================

USE mr_rent;

-- ============================================================================
-- DELETE EXISTING DATA (in reverse order of foreign key dependencies)
-- ============================================================================

-- Delete payments first (depends on rentals)
DELETE FROM Payments;

-- Delete rentals (depends on customers and cars)
DELETE FROM Rentals;

-- Delete cars (depends on categories)
DELETE FROM Cars;

-- Delete customers (no dependencies)
DELETE FROM Customers;

-- Delete admins (no dependencies)
DELETE FROM Admins;

-- Reset auto-increment counters
ALTER TABLE Admins AUTO_INCREMENT = 1;
ALTER TABLE Customers AUTO_INCREMENT = 1;
ALTER TABLE Cars AUTO_INCREMENT = 1;
ALTER TABLE Rentals AUTO_INCREMENT = 1;
ALTER TABLE Payments AUTO_INCREMENT = 1;

-- ============================================================================
-- 1. INSERT ADMINS (No dependencies)
-- ============================================================================
INSERT INTO Admins (Email, Password, Role, CreatedAt) VALUES
('admin@mrrent.com', 'SuperAdmin123!', 'SUPER_ADMIN', NOW()),
('manager@mrrent.com', 'Manager123!', 'ADMIN', NOW()),
('staff@mrrent.com', 'Staff123!', 'ADMIN', NOW());

-- ============================================================================
-- 2. INSERT CUSTOMERS (No dependencies)
-- ============================================================================
INSERT INTO Customers (Email, Password, FirstName, LastName, Phone, Address, City, PostalCode, Country, CreatedAt) VALUES
('john.doe@gmail.com', 'Password123!', 'John', 'Doe', '+46701234567', 'Storgatan 1', 'Stockholm', '11122', 'Sweden', NOW()),
('jane.smith@gmail.com', 'Password123!', 'Jane', 'Smith', '+46709876543', 'Kungsgatan 5', 'Gothenburg', '41103', 'Sweden', NOW()),
('mike.johnson@hotmail.com', 'Password123!', 'Mike', 'Johnson', '+46708765432', 'Drottninggatan 10', 'Malmö', '21143', 'Sweden', NOW()),
('anna.wilson@yahoo.com', 'Password123!', 'Anna', 'Wilson', '+46707654321', 'Norrmalmstorg 1', 'Stockholm', '11147', 'Sweden', NOW()),
('lars.andersson@outlook.com', 'Password123!', 'Lars', 'Andersson', '+46706543210', 'Avenyn 12', 'Gothenburg', '41104', 'Sweden', NOW()),
('emma.bergstrom@gmail.com', 'Password123!', 'Emma', 'Bergstrom', '+46705432109', 'Strandvägen 8', 'Stockholm', '11455', 'Sweden', NOW()),
('karl.nilsson@hotmail.com', 'Password123!', 'Karl', 'Nilsson', '+46704321098', 'Norra Hamngatan 18', 'Malmö', '21117', 'Sweden', NOW()),
('lisa.dahlgren@yahoo.com', 'Password123!', 'Lisa', 'Dahlgren', '+46703210987', 'Sofiegatan 3', 'Gothenburg', '41127', 'Sweden', NOW()),
('peter.lundgren@gmail.com', 'Password123!', 'Peter', 'Lundgren', '+46702109876', 'Västra Storgatan 1', 'Örebro', '70183', 'Sweden', NOW()),
('sarah.johansson@outlook.com', 'Password123!', 'Sarah', 'Johansson', '+46701098765', 'Österlånggatan 33', 'Stockholm', '11130', 'Sweden', NOW());

-- ============================================================================
-- 3. INSERT CARS (20 total - Depends on CarsCategories)
-- ============================================================================

-- Budget Cars (3) - CategoryId=1
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(1, 'Volkswagen', 'Polo', 2018, 'ABC123', 'Petrol', 'Manual', 5, 350.00, 'Available', NOW()),
(1, 'Ford', 'Fiesta', 2019, 'DEF456', 'Petrol', 'Manual', 5, 400.00, 'Available', NOW()),
(1, 'Skoda', 'Fabia', 2020, 'GHI789', 'Diesel', 'Manual', 5, 450.00, 'Available', NOW());

-- Economy Cars (5) - CategoryId=2
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(2, 'Toyota', 'Corolla', 2021, 'JKL012', 'Hybrid', 'Automatic', 5, 650.00, 'Available', NOW()),
(2, 'Volkswagen', 'Golf', 2022, 'MNO345', 'Petrol', 'Automatic', 5, 700.00, 'Available', NOW()),
(2, 'Honda', 'Civic', 2023, 'PQR678', 'Hybrid', 'Automatic', 5, 750.00, 'Rented', NOW()),
(2, 'Mazda', '3', 2022, 'STU901', 'Petrol', 'Automatic', 5, 680.00, 'Available', NOW()),
(2, 'Hyundai', 'i30', 2021, 'VWX234', 'Diesel', 'Automatic', 5, 620.00, 'Available', NOW());

-- SUV Cars (7) - CategoryId=3
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(3, 'Volvo', 'XC60', 2023, 'YZA567', 'Hybrid', 'Automatic', 5, 1200.00, 'Available', NOW()),
(3, 'BMW', 'X3', 2022, 'BCD890', 'Diesel', 'Automatic', 5, 1350.00, 'Rented', NOW()),
(3, 'Toyota', 'RAV4', 2024, 'EFG123', 'Hybrid', 'Automatic', 5, 1100.00, 'Available', NOW()),
(3, 'Nissan', 'Qashqai', 2021, 'HIJ456', 'Petrol', 'Automatic', 5, 950.00, 'Available', NOW()),
(3, 'Audi', 'Q5', 2023, 'KLM789', 'Diesel', 'Automatic', 5, 1400.00, 'Available', NOW()),
(3, 'Mercedes', 'GLC', 2022, 'NOP012', 'Diesel', 'Automatic', 5, 1500.00, 'Available', NOW()),
(3, 'Volkswagen', 'Tiguan', 2023, 'QRS345', 'Petrol', 'Automatic', 5, 1050.00, 'Available', NOW());

-- Transport Vehicles (5) - CategoryId=4
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(4, 'Mercedes', 'Sprinter', 2022, 'TUV678', 'Diesel', 'Manual', 3, 1800.00, 'Available', NOW()),
(4, 'Ford', 'Transit', 2021, 'WXY901', 'Diesel', 'Manual', 3, 1600.00, 'Rented', NOW()),
(4, 'Volkswagen', 'Transporter', 2023, 'ZAB234', 'Diesel', 'Automatic', 9, 1700.00, 'Available', NOW()),
(4, 'Renault', 'Master', 2020, 'CDE567', 'Diesel', 'Manual', 3, 1500.00, 'Available', NOW()),
(4, 'Peugeot', 'Boxer', 2022, 'FGH890', 'Diesel', 'Manual', 3, 1650.00, 'Available', NOW());

-- ============================================================================
-- 4. INSERT RENTALS (Depends on Customers + Cars, PaymentId = NULL initially)
-- ============================================================================
INSERT INTO Rentals (CustomerId, CarId, PaymentId, RentalDate, StartDate, EndDate, BookingNumber, Status, CreatedAt) VALUES
-- Active rentals (ongoing - current/recent)
(1, 3, NULL, '2026-02-20 10:00:00', '2026-02-22 10:00:00', '2026-02-25 10:00:00', 'RNT-20260220-001', 'ACTIVE', NOW()),
(2, 6, NULL, '2026-02-21 14:00:00', '2026-02-23 09:00:00', '2026-02-26 09:00:00', 'RNT-20260221-001', 'ACTIVE', NOW()),
(3, 16, NULL, '2026-02-19 11:00:00', '2026-02-24 10:00:00', '2026-02-27 10:00:00', 'RNT-20260219-001', 'ACTIVE', NOW()),

-- Pending rentals (future - April 2026)
(4, 4, NULL, '2026-03-01 09:00:00', '2026-04-05 10:00:00', '2026-04-08 10:00:00', 'RNT-20260301-001', 'PENDING', NOW()),
(5, 8, NULL, '2026-03-05 15:00:00', '2026-04-10 09:00:00', '2026-04-14 09:00:00', 'RNT-20260305-001', 'PENDING', NOW()),

-- Completed rentals (past - February 2026 and earlier)
(6, 1, NULL, '2026-02-10 10:00:00', '2026-02-12 10:00:00', '2026-02-15 10:00:00', 'RNT-20260210-001', 'COMPLETED', NOW()),
(7, 2, NULL, '2026-02-05 14:00:00', '2026-02-07 09:00:00', '2026-02-10 09:00:00', 'RNT-20260205-001', 'COMPLETED', NOW()),
(8, 5, NULL, '2026-02-01 11:00:00', '2026-02-03 10:00:00', '2026-02-06 10:00:00', 'RNT-20260201-001', 'COMPLETED', NOW()),
(9, 9, NULL, '2026-01-28 09:00:00', '2026-01-30 14:00:00', '2026-02-02 14:00:00', 'RNT-20260128-001', 'COMPLETED', NOW()),
(10, 12, NULL, '2026-01-15 10:00:00', '2026-01-17 10:00:00', '2026-01-21 10:00:00', 'RNT-20260115-001', 'COMPLETED', NOW());

-- ============================================================================
-- 5. INSERT PAYMENTS (Depends on Rentals)
-- ============================================================================
-- Amount calculation: Number of days × Car daily price

-- Active Rentals Payments
INSERT INTO Payments (RentalId, Amount, Method, Status, PaymentDate, TransactionId, CreatedAt) VALUES
(1, 1350.00, 'Card', 'SUCCESS', '2026-02-20 10:15:00', 'TXN-ABC123456', NOW()),
(2, 2250.00, 'Swish', 'SUCCESS', '2026-02-21 14:10:00', 'TXN-DEF789012', NOW()),
(3, 8100.00, 'Card', 'SUCCESS', '2026-02-19 11:05:00', 'TXN-GHI345678', NOW()),

-- Pending Rentals Payments
(4, 2600.00, 'Card', 'PENDING', '2026-03-01 09:15:00', 'TXN-JKL901234', NOW()),
(5, 3100.00, 'Swish', 'PENDING', '2026-03-05 15:10:00', 'TXN-MNO567890', NOW()),

-- Completed Rentals Payments
(6, 1050.00, 'Card', 'SUCCESS', '2026-02-10 10:15:00', 'TXN-PQR234567', NOW()),
(7, 1200.00, 'Cash', 'SUCCESS', '2026-02-05 14:10:00', 'TXN-STU890123', NOW()),
(8, 2040.00, 'Card', 'SUCCESS', '2026-02-01 11:05:00', 'TXN-VWX456789', NOW()),
(9, 4100.00, 'Swish', 'SUCCESS', '2026-01-28 09:15:00', 'TXN-YZA123012', NOW()),
(10, 5400.00, 'Card', 'SUCCESS', '2026-01-15 10:15:00', 'TXN-BCD789345', NOW());

-- ============================================================================
-- 6. UPDATE RENTALS with PaymentId (Link rentals to payments)
-- ============================================================================
UPDATE Rentals SET PaymentId = 1 WHERE Id = 1;
UPDATE Rentals SET PaymentId = 2 WHERE Id = 2;
UPDATE Rentals SET PaymentId = 3 WHERE Id = 3;
UPDATE Rentals SET PaymentId = 4 WHERE Id = 4;
UPDATE Rentals SET PaymentId = 5 WHERE Id = 5;
UPDATE Rentals SET PaymentId = 6 WHERE Id = 6;
UPDATE Rentals SET PaymentId = 7 WHERE Id = 7;
UPDATE Rentals SET PaymentId = 8 WHERE Id = 8;
UPDATE Rentals SET PaymentId = 9 WHERE Id = 9;
UPDATE Rentals SET PaymentId = 10 WHERE Id = 10;

-- ============================================================================
-- VERIFICATION QUERIES (Run these to validate your data)
-- ============================================================================

-- Check all admins
SELECT 'Admins' as Table_Name, COUNT(*) as Record_Count FROM Admins;

-- Check all customers
SELECT 'Customers' as Table_Name, COUNT(*) as Record_Count FROM Customers;

-- Check all cars by category
SELECT 
    cc.Name as Category,
    COUNT(c.Id) as Car_Count
FROM Cars c
JOIN CarsCategories cc ON c.CategoryId = cc.Id
GROUP BY cc.Name;

-- Check all rentals by status
SELECT 
    Status,
    COUNT(*) as Rental_Count
FROM Rentals
GROUP BY Status;

-- Check all payments by status
SELECT 
    Status,
    COUNT(*) as Payment_Count,
    SUM(Amount) as Total_Amount
FROM Payments
GROUP BY Status;

-- Check cars that are currently rented (should match active rentals)
SELECT c.Brand, c.Model, c.RegNr, c.Status
FROM Cars c
WHERE c.Status = 'Rented'
ORDER BY c.Brand;

-- Summary statistics
SELECT 
    (SELECT COUNT(*) FROM Admins) as Total_Admins,
    (SELECT COUNT(*) FROM Customers) as Total_Customers,
    (SELECT COUNT(*) FROM Cars) as Total_Cars,
    (SELECT COUNT(*) FROM Rentals) as Total_Rentals,
    (SELECT COUNT(*) FROM Payments) as Total_Payments;
