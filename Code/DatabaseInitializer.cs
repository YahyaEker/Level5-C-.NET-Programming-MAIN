using System;
using System.Data.SQLite;
using System.IO;
using System.Web;

namespace EmployeeManagement.App_Code
{
    public static class DatabaseInitializer
    {
        private static readonly object SyncRoot = new object();
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;

            lock (SyncRoot)
            {
                if (_initialized) return;
                EnsureDataDirectory();
                CreateSchemaAndSeedData();
                _initialized = true;
            }
        }

        private static void EnsureDataDirectory()
        {
            HttpContext context = HttpContext.Current;
            string appDataPath;

            if (context != null)
            {
                appDataPath = context.Server.MapPath("~/App_Data");
            }
            else
            {
                appDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            }

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            AppDomain.CurrentDomain.SetData("DataDirectory", appDataPath);
        }

        private static void CreateSchemaAndSeedData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    pragma.ExecuteNonQuery();
                }

                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Admins (
                        AdminID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL,
                        DepartmentID INTEGER NOT NULL CHECK (DepartmentID IN (1, 2, 3))
                    );");

                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        EmployeeID INTEGER PRIMARY KEY,
                        FullName TEXT NOT NULL,
                        Department TEXT NOT NULL,
                        Position TEXT NOT NULL,
                        Salary REAL NOT NULL,
                        Email TEXT NOT NULL UNIQUE
                    );");

                try
                {
                    using (SQLiteCommand cmdCheck = new SQLiteCommand("SELECT COUNT(*) FROM Admins;", connection))
                    {
                        long count = Convert.ToInt64(cmdCheck.ExecuteScalar());
                        if (count == 0)
                        {
                            InsertAdmin(connection, "hr_admin", "Hr@2024", 1);
                            InsertAdmin(connection, "fin_admin", "Fin@2024", 2);
                            InsertAdmin(connection, "it_admin", "It@2024", 3);

                            InsertEmployee(connection, 1001, "Alice Johnson", "Human Resources", "HR Manager", 72000.00m, "alice.johnson@company.com");
                            InsertEmployee(connection, 1002, "Robert Chen", "Finance", "Financial Analyst", 68000.00m, "robert.chen@company.com");
                            InsertEmployee(connection, 1003, "Sarah Williams", "Information Technology", "Software Developer", 85000.00m, "sarah.williams@company.com");
                            InsertEmployee(connection, 1004, "Michael Brown", "Finance", "Accountant", 61000.00m, "michael.brown@company.com");
                            InsertEmployee(connection, 1005, "Emily Davis", "Human Resources", "Recruiter", 55000.00m, "emily.davis@company.com");
                        }
                    }
                }
                catch (Exception)
                {
                    // Hata yutulur, çökme engellenir.
                }
            }
        }

        private static void InsertAdmin(SQLiteConnection connection, string username, string password, int departmentId)
        {
            const string sql = @"
                INSERT INTO Admins (Username, Password, DepartmentID)
                VALUES (@Username, @Password, @DepartmentID);";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@DepartmentID", departmentId);
                command.ExecuteNonQuery();
            }
        }

        private static void InsertEmployee(SQLiteConnection connection, int employeeId, string fullName, string department, string position, decimal salary, string email)
        {
            const string sql = @"
                INSERT INTO Employees (EmployeeID, FullName, Department, Position, Salary, Email)
                VALUES (@EmployeeID, @FullName, @Department, @Position, @Salary, @Email);";

            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@Department", department);
                command.Parameters.AddWithValue("@Position", position);
                command.Parameters.AddWithValue("@Salary", salary);
                command.Parameters.AddWithValue("@Email", email);
                command.ExecuteNonQuery();
            }
        }

        private static void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}