using FrontOfficeERP.Models;
using FrontOfficeERP.Services;
using System.Collections.ObjectModel;

namespace FrontOfficeERP.ViewModels;

public class EmployeeViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Employee> Employees { get; } = new();

    public EmployeeViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public void LoadEmployees()
    {
        Employees.Clear();
        foreach (var employee in _databaseService.GetEmployees())
        {
            Employees.Add(employee);
        }
    }

    public void AddEmployee(Employee employee)
    {
        _databaseService.AddEmployee(employee);
        LoadEmployees();
    }
}
