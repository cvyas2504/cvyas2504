using FrontOfficeERP.Models;
using Microsoft.Data.Sqlite;

namespace FrontOfficeERP.Services;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string databaseFile = "erp.db3")
    {
        _connectionString = $"Data Source={databaseFile}";
    }

    public void AddEmployee(Employee employee)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Employees (Name, Department, Designation, Shift, Phone, Email, JoinDate)
            VALUES ($name, $department, $designation, $shift, $phone, $email, $joinDate);";
        command.Parameters.AddWithValue("$name", employee.Name);
        command.Parameters.AddWithValue("$department", employee.Department);
        command.Parameters.AddWithValue("$designation", employee.Designation);
        command.Parameters.AddWithValue("$shift", employee.Shift);
        command.Parameters.AddWithValue("$phone", employee.Phone);
        command.Parameters.AddWithValue("$email", employee.Email);
        command.Parameters.AddWithValue("$joinDate", employee.JoinDate.ToString("yyyy-MM-dd"));
        command.ExecuteNonQuery();
    }

    public List<Employee> GetEmployees()
    {
        var employees = new List<Employee>();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Department, Designation, Shift, Phone, Email, JoinDate FROM Employees;";

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            employees.Add(new Employee
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Department = reader.GetString(2),
                Designation = reader.GetString(3),
                Shift = reader.GetString(4),
                Phone = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                Email = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                JoinDate = DateTime.Parse(reader.GetString(7))
            });
        }

        return employees;
    }
}
