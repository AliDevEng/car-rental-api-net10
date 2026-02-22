-- =============================================
-- CarRental Database Verification Script
-- Run this in MySQL Workbench to verify everything
-- =============================================

USE mr_rent;

-- 1. Show all tables
SHOW TABLES;

-- 2. Verify all 6 tables exist
SELECT 
    'Expected Tables' AS Status,
    6 AS Expected,
    COUNT(*) AS Actual,
    CASE WHEN COUNT(*) = 6 THEN '✅ PASS' ELSE '❌ FAIL' END AS Result
FROM information_schema.tables 
WHERE table_schema = 'mr_rent';

-- 3. Check each table structure
DESCRIBE Admins;
DESCRIBE Cars;
DESCRIBE CarsCategories;
DESCRIBE Customers;
DESCRIBE Payments;
DESCRIBE Rentals;

-- 4. Verify seeded categories (should be 4)
SELECT 
    'Seeded Categories' AS Status,
    4 AS Expected,
    COUNT(*) AS Actual,
    CASE WHEN COUNT(*) = 4 THEN '✅ PASS' ELSE '❌ FAIL' END AS Result
FROM CarsCategories;

-- 5. Show seeded category data
SELECT * FROM CarsCategories ORDER BY Id;

-- 6. Verify Foreign Key Relationships
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    CONSTRAINT_NAME,
    REFERENCED_TABLE_NAME,
    REFERENCED_COLUMN_NAME
FROM information_schema.KEY_COLUMN_USAGE
WHERE TABLE_SCHEMA = 'mr_rent'
    AND REFERENCED_TABLE_NAME IS NOT NULL
ORDER BY TABLE_NAME, COLUMN_NAME;

-- 7. Verify Indexes
SELECT 
    TABLE_NAME,
    INDEX_NAME,
    COLUMN_NAME,
    NON_UNIQUE
FROM information_schema.STATISTICS
WHERE TABLE_SCHEMA = 'mr_rent'
    AND INDEX_NAME != 'PRIMARY'
ORDER BY TABLE_NAME, INDEX_NAME;

-- 8. Check Migration History
SELECT * FROM __EFMigrationsHistory;

-- =============================================
-- Summary Report
-- =============================================
SELECT '✅ Database Verification Complete' AS Status;

SELECT 
    'Tables' AS Item,
    (SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'mr_rent') AS Count,
    '6 expected' AS Expected;

SELECT 
    'Foreign Keys' AS Item,
    COUNT(*) AS Count,
    '2 expected (Cars->Categories, Rentals->Cars)' AS Expected
FROM information_schema.KEY_COLUMN_USAGE
WHERE TABLE_SCHEMA = 'mr_rent' AND REFERENCED_TABLE_NAME IS NOT NULL;

SELECT 
    'Categories' AS Item,
    COUNT(*) AS Count,
    '4 expected (Budget, Economy, SUV, Transport)' AS Expected
FROM CarsCategories;
