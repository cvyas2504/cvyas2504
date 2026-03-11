using Microsoft.Data.Sqlite;

namespace FrontOfficeERP.Database;

public static class ERPDatabase
{
    public const string DatabaseFile = "erp.db3";

    public static void Initialize()
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseFile}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Employees (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Department TEXT NOT NULL,
                Designation TEXT NOT NULL,
                Shift TEXT NOT NULL,
                Phone TEXT,
                Email TEXT,
                JoinDate TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Departments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DepartmentName TEXT NOT NULL UNIQUE
            );

            CREATE TABLE IF NOT EXISTS DutyRoster (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmployeeId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                Shift TEXT NOT NULL,
                FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)
            );

            CREATE TABLE IF NOT EXISTS Attendance (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EmployeeId INTEGER NOT NULL,
                Date TEXT NOT NULL,
                CheckIn TEXT,
                CheckOut TEXT,
                FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)
            );

            CREATE TABLE IF NOT EXISTS Visitors (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Purpose TEXT,
                VisitDate TEXT NOT NULL,
                EmployeeToMeet TEXT
            );

            CREATE TABLE IF NOT EXISTS ExcelCompareReports (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                File1 TEXT NOT NULL,
                File2 TEXT NOT NULL,
                CompareDate TEXT NOT NULL,
                Result TEXT NOT NULL
            );";

        command.ExecuteNonQuery();
    }
}
