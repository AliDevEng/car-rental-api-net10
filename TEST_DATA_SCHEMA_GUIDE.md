# 🗄️ CarRental Database Schema - Complete Test Data Guide

**Database:** `mr_rent`  
**Purpose:** Generate realistic test data for a car rental system  
**Target:** MySQL 8.0+

---

## 📊 Table Schemas & Relationships

### **1. CarsCategories** (Already has seed data)

**Purpose:** Store car category types

```sql
CREATE TABLE CarsCategories (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL
);
```

**Existing Data (DO NOT MODIFY):**
```sql
-- Already seeded via migration:
-- Id=1, Name='Budget'
-- Id=2, Name='Economy'
-- Id=3, Name='SUV'
-- Id=4, Name='Transport'
```

**Business Rules:**
- ✅ Already populated with 4 categories
- ❌ Do NOT add more categories
- ✅ Use existing IDs (1-4) when creating Cars

---

### **2. Admins** (Need test data)

**Purpose:** Store admin/staff user accounts

```sql
CREATE TABLE Admins (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,  -- Will be hashed with BCrypt later
    Role VARCHAR(50) NOT NULL,       -- 'ADMIN' or 'SUPER_ADMIN'
    CreatedAt DATETIME(6) NOT NULL
);
```

**Column Details:**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Unique admin identifier |
| Email | VARCHAR(255) | NOT NULL, should be UNIQUE | Admin login email |
| Password | VARCHAR(255) | NOT NULL | Plain text for now, BCrypt later |
| Role | VARCHAR(50) | NOT NULL | 'ADMIN' or 'SUPER_ADMIN' |
| CreatedAt | DATETIME(6) | NOT NULL | Account creation timestamp |

**Test Data Requirements:**
- Need 2-3 admin accounts
- At least 1 SUPER_ADMIN
- At least 1 regular ADMIN
- Unique email addresses
- Strong passwords (will implement hashing in Iteration 3)

**Example Format:**
```sql
INSERT INTO Admins (Email, Password, Role, CreatedAt) VALUES
('admin@mrrent.com', 'Admin123!', 'SUPER_ADMIN', NOW()),
('staff@mrrent.com', 'Staff123!', 'ADMIN', NOW());
```

---

### **3. Customers** (Need test data)

**Purpose:** Store customer/renter accounts

```sql
CREATE TABLE Customers (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Phone VARCHAR(20),
    Address VARCHAR(200),
    City VARCHAR(50),
    PostalCode VARCHAR(20),
    Country VARCHAR(50),
    CreatedAt DATETIME(6) NOT NULL
);
```

**Column Details:**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Unique customer ID |
| Email | VARCHAR(255) | NOT NULL, should be UNIQUE | Customer login email |
| Password | VARCHAR(255) | NOT NULL | Plain text for now |
| FirstName | VARCHAR(50) | NULL | Customer first name |
| LastName | VARCHAR(50) | NULL | Customer last name |
| Phone | VARCHAR(20) | NULL | Contact number |
| Address | VARCHAR(200) | NULL | Street address |
| City | VARCHAR(50) | NULL | City name |
| PostalCode | VARCHAR(20) | NULL | Postal/ZIP code |
| Country | VARCHAR(50) | NULL | Country name |
| CreatedAt | DATETIME(6) | NOT NULL | Registration date |

**Test Data Requirements:**
- Need 5-10 customers
- Mix of complete and partial profiles
- Diverse locations
- Realistic names and contact info
- Some for active rentals, some for past rentals

**Example Format:**
```sql
INSERT INTO Customers (Email, Password, FirstName, LastName, Phone, Address, City, PostalCode, Country, CreatedAt) VALUES
('john.doe@gmail.com', 'Password123!', 'John', 'Doe', '+46701234567', 'Storgatan 1', 'Stockholm', '11122', 'Sweden', NOW()),
('jane.smith@gmail.com', 'Password123!', 'Jane', 'Smith', '+46709876543', 'Kungsgatan 5', 'Gothenburg', '41103', 'Sweden', NOW());
```

---

### **4. Cars** (Need test data - MOST IMPORTANT)

**Purpose:** Store car inventory

```sql
CREATE TABLE Cars (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CategoryId INT NOT NULL,
    Brand VARCHAR(255) NOT NULL,
    Model VARCHAR(255) NOT NULL,
    Year INT NOT NULL,
    RegNr VARCHAR(255) NOT NULL UNIQUE,
    Fuel VARCHAR(20) NOT NULL,           -- 'Petrol', 'Diesel', 'Electric', 'Hybrid'
    Transmission VARCHAR(20) NOT NULL,   -- 'Manual', 'Automatic'
    Seats INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,        -- Price per day
    Status VARCHAR(20) NOT NULL,         -- 'Available', 'Rented', 'Maintenance'
    CreatedAt DATETIME(6) NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES CarsCategories(Id)
);
```

**Column Details:**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Unique car ID |
| CategoryId | INT | FK → CarsCategories.Id | 1=Budget, 2=Economy, 3=SUV, 4=Transport |
| Brand | VARCHAR(255) | NOT NULL | Car manufacturer (VW, Toyota, BMW) |
| Model | VARCHAR(255) | NOT NULL | Car model name |
| Year | INT | NOT NULL | Manufacturing year (2018-2024) |
| RegNr | VARCHAR(255) | NOT NULL, UNIQUE | License plate (ABC123, XYZ789) |
| Fuel | VARCHAR(20) | NOT NULL | Petrol, Diesel, Electric, Hybrid |
| Transmission | VARCHAR(20) | NOT NULL | Manual or Automatic |
| Seats | INT | NOT NULL | Number of seats (2-9) |
| Price | DECIMAL(18,2) | NOT NULL | Rental price per day (SEK) |
| Status | VARCHAR(20) | NOT NULL | Available, Rented, Maintenance |
| CreatedAt | DATETIME(6) | NOT NULL | Added to inventory date |

**Test Data Requirements:**
- **Budget (CategoryId=1):** 3 cars, Price: 300-500 SEK/day, older models
- **Economy (CategoryId=2):** 5 cars, Price: 500-800 SEK/day, standard sedans
- **SUV (CategoryId=3):** 7 cars, Price: 900-1500 SEK/day, larger vehicles
- **Transport (CategoryId=4):** 5 cars, Price: 1000-2000 SEK/day, vans/trucks

**Total:** 20 cars as per project description

**Example Format:**
```sql
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
-- Budget Cars (3)
(1, 'Volkswagen', 'Polo', 2018, 'ABC123', 'Petrol', 'Manual', 5, 350.00, 'Available', NOW()),
(1, 'Ford', 'Fiesta', 2019, 'DEF456', 'Petrol', 'Manual', 5, 400.00, 'Available', NOW()),
(1, 'Skoda', 'Fabia', 2020, 'GHI789', 'Diesel', 'Manual', 5, 450.00, 'Rented', NOW()),

-- Economy Cars (5)
(2, 'Toyota', 'Corolla', 2021, 'JKL012', 'Hybrid', 'Automatic', 5, 650.00, 'Available', NOW()),
(2, 'Volkswagen', 'Golf', 2022, 'MNO345', 'Petrol', 'Automatic', 5, 700.00, 'Available', NOW());
```

**Car Brand/Model Suggestions:**
- **Budget:** VW Polo, Ford Fiesta, Skoda Fabia, Hyundai i20
- **Economy:** Toyota Corolla, VW Golf, Honda Civic, Mazda 3
- **SUV:** Volvo XC60, BMW X3, Toyota RAV4, Nissan Qashqai
- **Transport:** Mercedes Sprinter, Ford Transit, VW Transporter

---

### **5. Rentals** (Need test data)

**Purpose:** Store booking/rental records

```sql
CREATE TABLE Rentals (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    CustomerId INT NOT NULL,
    CarId INT NOT NULL,
    PaymentId INT NULL,                  -- NULL until payment created
    RentalDate DATETIME(6) NOT NULL,     -- Date rental was created/booked
    StartDate DATETIME(6) NOT NULL,      -- Rental start date
    EndDate DATETIME(6) NOT NULL,        -- Rental end date
    BookingNumber VARCHAR(255) NOT NULL UNIQUE,
    Status VARCHAR(20) NOT NULL,         -- 'PENDING', 'ACTIVE', 'COMPLETED', 'CANCELLED'
    CreatedAt DATETIME(6) NOT NULL,
    FOREIGN KEY (CarId) REFERENCES Cars(Id)
);
```

**Column Details:**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Unique rental ID |
| CustomerId | INT | NOT NULL | FK → Customers.Id |
| CarId | INT | FK → Cars.Id | Which car was rented |
| PaymentId | INT | NULL | FK → Payments.Id (set after payment) |
| RentalDate | DATETIME(6) | NOT NULL | When booking was made |
| StartDate | DATETIME(6) | NOT NULL | Rental period start |
| EndDate | DATETIME(6) | NOT NULL | Rental period end |
| BookingNumber | VARCHAR(255) | NOT NULL, UNIQUE | Format: RNT-20240222-001 |
| Status | VARCHAR(20) | NOT NULL | PENDING, ACTIVE, COMPLETED, CANCELLED |
| CreatedAt | DATETIME(6) | NOT NULL | Record creation timestamp |

**Test Data Requirements:**
- 5-10 rentals with various statuses
- Mix of past, current, and future rentals
- Different rental durations (1 day to 2 weeks)
- Some with payments, some pending

**Status Flow:**
1. **PENDING** - Just booked, awaiting payment
2. **ACTIVE** - Paid, currently rented
3. **COMPLETED** - Rental finished, car returned
4. **CANCELLED** - Booking cancelled

**Example Format:**
```sql
INSERT INTO Rentals (CustomerId, CarId, PaymentId, RentalDate, StartDate, EndDate, BookingNumber, Status, CreatedAt) VALUES
(1, 3, NULL, NOW(), '2024-02-25 10:00:00', '2024-02-28 10:00:00', 'RNT-20240222-001', 'ACTIVE', NOW()),
(2, 5, NULL, NOW(), '2024-03-01 09:00:00', '2024-03-05 09:00:00', 'RNT-20240222-002', 'PENDING', NOW());
```

**Booking Number Format:**
- `RNT-YYYYMMDD-XXX`
- Example: `RNT-20240222-001`, `RNT-20240222-002`

---

### **6. Payments** (Need test data)

**Purpose:** Store payment transactions

```sql
CREATE TABLE Payments (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    RentalId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    Method VARCHAR(50) NOT NULL,         -- 'Card', 'Cash', 'Swish'
    Status VARCHAR(20) NOT NULL,         -- 'PENDING', 'SUCCESS', 'FAILED'
    PaymentDate DATETIME(6) NOT NULL,
    TransactionId VARCHAR(100),          -- Payment gateway transaction ID
    CreatedAt DATETIME(6) NOT NULL
);
```

**Column Details:**
| Column | Type | Constraints | Description |
|--------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Unique payment ID |
| RentalId | INT | NOT NULL | FK → Rentals.Id |
| Amount | DECIMAL(18,2) | NOT NULL | Total payment amount |
| Method | VARCHAR(50) | NOT NULL | Card, Cash, Swish, Invoice |
| Status | VARCHAR(20) | NOT NULL | PENDING, SUCCESS, FAILED |
| PaymentDate | DATETIME(6) | NOT NULL | When payment was made |
| TransactionId | VARCHAR(100) | NULL | External payment ID |
| CreatedAt | DATETIME(6) | NOT NULL | Record creation |

**Test Data Requirements:**
- Create payments for ACTIVE and COMPLETED rentals
- Mix of payment methods
- Mostly SUCCESS status
- Calculate Amount = (EndDate - StartDate) × Car.Price

**Amount Calculation:**
```
Amount = NumberOfDays × DailyPrice
Example: 3 days × 650 SEK = 1950 SEK
```

**Example Format:**
```sql
INSERT INTO Payments (RentalId, Amount, Method, Status, PaymentDate, TransactionId, CreatedAt) VALUES
(1, 1050.00, 'Card', 'SUCCESS', NOW(), 'TXN-ABC123456', NOW()),
(2, 2600.00, 'Swish', 'SUCCESS', NOW(), 'TXN-XYZ789012', NOW());
```

---

## 📋 Data Insertion Order (IMPORTANT!)

**Insert in this exact order to avoid FK constraint errors:**

1. ✅ **CarsCategories** - Already exists (skip)
2. **Admins** - No dependencies
3. **Customers** - No dependencies
4. **Cars** - Depends on CarsCategories
5. **Rentals** - Depends on Customers + Cars (PaymentId = NULL initially)
6. **Payments** - Depends on Rentals
7. **Update Rentals** - Set PaymentId after Payments created

---

## 🎯 Test Data Requirements Summary

| Table | Count | Priority | Notes |
|-------|-------|----------|-------|
| CarsCategories | 4 | ✅ Done | Already seeded |
| Admins | 2-3 | Medium | 1 SUPER_ADMIN, 1-2 ADMIN |
| Customers | 5-10 | High | Mix of profiles |
| Cars | 20 | **Critical** | 3 Budget, 5 Economy, 7 SUV, 5 Transport |
| Rentals | 5-10 | High | Various statuses |
| Payments | 5-8 | Medium | For ACTIVE/COMPLETED rentals |

---

## 💡 Realistic Data Guidelines

### **Cars:**
- Use real car brands and models
- Year: 2018-2024
- RegNr: Swedish format (ABC123, XYZ789)
- Price: Realistic daily rates in SEK
- Status: Mostly 'Available', some 'Rented'

### **Customers:**
- Swedish or international names
- Swedish phone format: +467XXXXXXXX
- Mix of cities: Stockholm, Gothenburg, Malmö
- Realistic addresses

### **Rentals:**
- Mix of timeframes:
  - Past (completed)
  - Current (active)
  - Future (pending)
- Booking numbers sequential

### **Payments:**
- Amounts match rental duration × car price
- Transaction IDs: TXN-XXXXXXXXX format
- Most should be SUCCESS status

---

## 📝 Example Complete Test Dataset

**Copy-paste this as a starting point and expand:**

```sql
USE mr_rent;

-- 1. Admins
INSERT INTO Admins (Email, Password, Role, CreatedAt) VALUES
('admin@mrrent.com', 'SuperAdmin123!', 'SUPER_ADMIN', NOW()),
('manager@mrrent.com', 'Manager123!', 'ADMIN', NOW());

-- 2. Customers
INSERT INTO Customers (Email, Password, FirstName, LastName, Phone, Address, City, PostalCode, Country, CreatedAt) VALUES
('john.doe@gmail.com', 'Password123!', 'John', 'Doe', '+46701234567', 'Storgatan 1', 'Stockholm', '11122', 'Sweden', NOW()),
('jane.smith@gmail.com', 'Password123!', 'Jane', 'Smith', '+46709876543', 'Kungsgatan 5', 'Gothenburg', '41103', 'Sweden', NOW()),
('mike.johnson@hotmail.com', 'Password123!', 'Mike', 'Johnson', '+46708765432', 'Drottninggatan 10', 'Malmö', '21143', 'Sweden', NOW());

-- 3. Cars (20 total)
-- Budget (3)
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(1, 'Volkswagen', 'Polo', 2018, 'ABC123', 'Petrol', 'Manual', 5, 350.00, 'Available', NOW()),
(1, 'Ford', 'Fiesta', 2019, 'DEF456', 'Petrol', 'Manual', 5, 400.00, 'Available', NOW()),
(1, 'Skoda', 'Fabia', 2020, 'GHI789', 'Diesel', 'Manual', 5, 450.00, 'Rented', NOW());

-- Economy (5)
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(2, 'Toyota', 'Corolla', 2021, 'JKL012', 'Hybrid', 'Automatic', 5, 650.00, 'Available', NOW()),
(2, 'Volkswagen', 'Golf', 2022, 'MNO345', 'Petrol', 'Automatic', 5, 700.00, 'Available', NOW()),
(2, 'Honda', 'Civic', 2023, 'PQR678', 'Hybrid', 'Automatic', 5, 750.00, 'Rented', NOW()),
(2, 'Mazda', '3', 2022, 'STU901', 'Petrol', 'Automatic', 5, 680.00, 'Available', NOW()),
(2, 'Hyundai', 'i30', 2021, 'VWX234', 'Diesel', 'Automatic', 5, 620.00, 'Available', NOW());

-- SUV (7)
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(3, 'Volvo', 'XC60', 2023, 'YZA567', 'Hybrid', 'Automatic', 5, 1200.00, 'Available', NOW()),
(3, 'BMW', 'X3', 2022, 'BCD890', 'Diesel', 'Automatic', 5, 1350.00, 'Rented', NOW()),
(3, 'Toyota', 'RAV4', 2024, 'EFG123', 'Hybrid', 'Automatic', 5, 1100.00, 'Available', NOW()),
(3, 'Nissan', 'Qashqai', 2021, 'HIJ456', 'Petrol', 'Automatic', 5, 950.00, 'Available', NOW()),
(3, 'Audi', 'Q5', 2023, 'KLM789', 'Diesel', 'Automatic', 5, 1400.00, 'Available', NOW()),
(3, 'Mercedes', 'GLC', 2022, 'NOP012', 'Diesel', 'Automatic', 5, 1500.00, 'Available', NOW()),
(3, 'Volkswagen', 'Tiguan', 2023, 'QRS345', 'Petrol', 'Automatic', 5, 1050.00, 'Available', NOW());

-- Transport (5)
INSERT INTO Cars (CategoryId, Brand, Model, Year, RegNr, Fuel, Transmission, Seats, Price, Status, CreatedAt) VALUES
(4, 'Mercedes', 'Sprinter', 2022, 'TUV678', 'Diesel', 'Manual', 3, 1800.00, 'Available', NOW()),
(4, 'Ford', 'Transit', 2021, 'WXY901', 'Diesel', 'Manual', 3, 1600.00, 'Rented', NOW()),
(4, 'Volkswagen', 'Transporter', 2023, 'ZAB234', 'Diesel', 'Automatic', 9, 1700.00, 'Available', NOW()),
(4, 'Renault', 'Master', 2020, 'CDE567', 'Diesel', 'Manual', 3, 1500.00, 'Available', NOW()),
(4, 'Peugeot', 'Boxer', 2022, 'FGH890', 'Diesel', 'Manual', 3, 1650.00, 'Available', NOW());

-- 4. Rentals
INSERT INTO Rentals (CustomerId, CarId, PaymentId, RentalDate, StartDate, EndDate, BookingNumber, Status, CreatedAt) VALUES
(1, 3, NULL, '2024-02-20 10:00:00', '2024-02-22 10:00:00', '2024-02-25 10:00:00', 'RNT-20240220-001', 'ACTIVE', '2024-02-20 10:00:00'),
(2, 6, NULL, '2024-02-21 14:00:00', '2024-02-23 09:00:00', '2024-02-26 09:00:00', 'RNT-20240221-001', 'ACTIVE', '2024-02-21 14:00:00'),
(3, 8, NULL, '2024-02-15 11:00:00', '2024-02-17 10:00:00', '2024-02-20 10:00:00', 'RNT-20240215-001', 'COMPLETED', '2024-02-15 11:00:00');

-- 5. Payments
INSERT INTO Payments (RentalId, Amount, Method, Status, PaymentDate, TransactionId, CreatedAt) VALUES
(1, 1350.00, 'Card', 'SUCCESS', '2024-02-20 10:15:00', 'TXN-ABC123456', '2024-02-20 10:15:00'),
(2, 2250.00, 'Swish', 'SUCCESS', '2024-02-21 14:10:00', 'TXN-DEF789012', '2024-02-21 14:10:00'),
(3, 4050.00, 'Card', 'SUCCESS', '2024-02-15 11:05:00', 'TXN-GHI345678', '2024-02-15 11:05:00');

-- 6. Update Rentals with PaymentId
UPDATE Rentals SET PaymentId = 1 WHERE Id = 1;
UPDATE Rentals SET PaymentId = 2 WHERE Id = 2;
UPDATE Rentals SET PaymentId = 3 WHERE Id = 3;
```

---

## ✅ Validation Checklist

After inserting test data, verify:

- [ ] 4 categories exist
- [ ] 2-3 admins created
- [ ] 5-10 customers created
- [ ] **20 cars created** (3+5+7+5 distribution)
- [ ] 5-10 rentals created
- [ ] Payments match active/completed rentals
- [ ] All foreign keys valid
- [ ] No duplicate RegNr, Email, BookingNumber
- [ ] Cars marked 'Rented' have active rentals
- [ ] Rental amounts = days × car price

---

**Use this document to generate comprehensive, realistic test data for the CarRental system!**
