using FrontOfficeERP.Database;
using FrontOfficeERP.Models;
using FrontOfficeERP.Services;

ERPDatabase.Initialize();

var databaseService = new DatabaseService();
var reportService = new ReportService();

var employee = new Employee
{
    Name = "Asha Patel",
    Department = "Front Office",
    Designation = "Reception Executive",
    Shift = "Morning",
    Phone = "9000000000",
    Email = "asha@example.com",
    JoinDate = DateTime.Today
};

databaseService.AddEmployee(employee);

var employees = databaseService.GetEmployees();
reportService.GenerateEmployeeReport(employees);

Console.WriteLine("ERP initialized and employee report generated.");
