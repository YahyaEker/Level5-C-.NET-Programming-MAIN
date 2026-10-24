-- ============================================================
-- Employee Information Management System
-- SQLite Schema Reference (auto-created in C# on App_Start)
-- ============================================================

CREATE TABLE IF NOT EXISTS Admins (
    AdminID INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    DepartmentID INTEGER NOT NULL CHECK (DepartmentID IN (1, 2, 3))
);

CREATE TABLE IF NOT EXISTS Employees (
    EmployeeID INTEGER PRIMARY KEY,
    FullName TEXT NOT NULL,
    Department TEXT NOT NULL,
    Position TEXT NOT NULL,
    Salary REAL NOT NULL,
    Email TEXT NOT NULL UNIQUE
);

-- Sample data (seeded automatically by DatabaseInitializer.cs)
-- HR:      hr_admin  / Hr@2024
-- Finance: fin_admin / Fin@2024
-- IT:      it_admin  / It@2024
