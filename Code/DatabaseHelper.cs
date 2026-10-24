using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace EmployeeManagement.App_Code
{
    /// <summary>
    /// Centralized ADO.NET data-access helper for SQLite operations.
    /// </summary>
    public static class DatabaseHelper
    {
        // C# 5 Uyumlu klasik property yapısı
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;
            }
        }

        /// <summary>
        /// Validates admin credentials against the Admins table.
        /// Returns admin details when successful; otherwise null.
        /// </summary>
        public static AdminUser AuthenticateAdmin(string username, string password)
        {
            DatabaseInitializer.Initialize();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            const string sql = @"
                SELECT AdminID, Username, Password, DepartmentID
                FROM Admins
                WHERE Username = @Username AND Password = @Password
                LIMIT 1;";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.Add(new SQLiteParameter("@Username", DbType.String) { Value = username.Trim() });
                command.Parameters.Add(new SQLiteParameter("@Password", DbType.String) { Value = password });

                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new AdminUser
                    {
                        AdminId = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        DepartmentId = reader.GetInt32(3)
                    };
                }
            }
        }

        /// <summary>
        /// Retrieves all employee records ordered by EmployeeID.
        /// </summary>
        public static List<EmployeeRecord> GetAllEmployees()
        {
            DatabaseInitializer.Initialize();

            const string sql = @"
                SELECT EmployeeID, FullName, Department, Position, Salary, Email
                FROM Employees
                ORDER BY EmployeeID;";

            List<EmployeeRecord> employees = new List<EmployeeRecord>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(MapEmployee(reader));
                    }
                }
            }

            return employees;
        }

        /// <summary>
        /// Retrieves a single employee record by primary key.
        /// </summary>
        public static EmployeeRecord GetEmployeeById(int employeeId)
        {
            DatabaseInitializer.Initialize();

            const string sql = @"
                SELECT EmployeeID, FullName, Department, Position, Salary, Email
                FROM Employees
                WHERE EmployeeID = @EmployeeID
                LIMIT 1;";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.Add(new SQLiteParameter("@EmployeeID", DbType.Int32) { Value = employeeId });

                connection.Open();

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return MapEmployee(reader);
                }
            }
        }

        /// <summary>
        /// Persists modified employee details to the database.
        /// </summary>
        public static bool UpdateEmployee(EmployeeRecord employee)
        {
            DatabaseInitializer.Initialize();

            const string sql = @"
                UPDATE Employees
                SET FullName   = @FullName,
                    Department = @Department,
                    Position   = @Position,
                    Salary     = @Salary,
                    Email      = @Email
                WHERE EmployeeID = @EmployeeID;";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.Parameters.Add(new SQLiteParameter("@EmployeeID", DbType.Int32) { Value = employee.EmployeeId });
                command.Parameters.Add(new SQLiteParameter("@FullName", DbType.String) { Value = employee.FullName });
                command.Parameters.Add(new SQLiteParameter("@Department", DbType.String) { Value = employee.Department });
                command.Parameters.Add(new SQLiteParameter("@Position", DbType.String) { Value = employee.Position });
                command.Parameters.Add(new SQLiteParameter("@Salary", DbType.Decimal) { Value = employee.Salary });
                command.Parameters.Add(new SQLiteParameter("@Email", DbType.String) { Value = employee.Email });

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        private static EmployeeRecord MapEmployee(SQLiteDataReader reader)
        {
            return new EmployeeRecord
            {
                EmployeeId = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Department = reader.GetString(2),
                Position = reader.GetString(3),
                Salary = Convert.ToDecimal(reader.GetValue(4), CultureInfo.InvariantCulture),
                Email = reader.GetString(5)
            };
        }
    }

    public class AdminUser
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public int DepartmentId { get; set; }

        public string DepartmentName
        {
            get
            {
                switch (DepartmentId)
                {
                    case 1: return "HR";
                    case 2: return "Finance";
                    case 3: return "IT";
                    default: return "General";
                }
            }
        }

        public string ThemeCssClass
        {
            get
            {
                switch (DepartmentId)
                {
                    case 1: return "theme-hr";
                    case 2: return "theme-finance theme-fin";
                    case 3: return "theme-it";
                    default: return "theme-hr";
                }
            }
        }

        public string ThemeStylesheet
        {
            get
            {
                switch (DepartmentId)
                {
                    case 1: return "~/Content/themes/theme-hr.css";
                    case 2: return "~/Content/themes/theme-finance.css";
                    case 3: return "~/Content/themes/theme-it.css";
                    default: return "~/Content/themes/theme-hr.css";
                }
            }
        }
    }

    public class EmployeeRecord
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public string Email { get; set; }
    }
}