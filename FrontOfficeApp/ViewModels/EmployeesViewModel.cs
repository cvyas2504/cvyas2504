using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class EmployeesViewModel(IEmployeeService employeeService, ISessionService sessionService) : BaseViewModel
{
    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private Employee selectedEmployee = new();
    public ObservableCollection<Employee> Employees { get; } = new();
    public bool IsAdmin => sessionService.CurrentUser?.Role == UserRole.Admin;

    [RelayCommand]
    public async Task LoadAsync()
    {
        Employees.Clear();
        var list = await employeeService.GetAsync(SearchText);
        foreach (var employee in list) Employees.Add(employee);
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (!IsAdmin) return;
        await employeeService.SaveAsync(SelectedEmployee);
        SelectedEmployee = new Employee();
        await LoadAsync();
    }

    [RelayCommand]
    public async Task DeleteAsync(Employee employee)
    {
        if (!IsAdmin) return;
        await employeeService.DeleteAsync(employee.EmployeeID);
        await LoadAsync();
    }
}
