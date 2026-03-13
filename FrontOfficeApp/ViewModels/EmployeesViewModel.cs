using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class EmployeesViewModel(IEmployeeService employeeService, IAuthorizationService authorizationService) : BaseViewModel
{
    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private Employee selectedEmployee = new();
    [ObservableProperty] private bool canCreate;
    [ObservableProperty] private bool canDelete;
    [ObservableProperty] private bool canExport;

    public ObservableCollection<Employee> Employees { get; } = new();

    [RelayCommand]
    public async Task LoadAsync()
    {
        var permission = await authorizationService.GetPermissionAsync(ModuleType.Employee);
        CanCreate = permission?.CanCreate == true;
        CanDelete = permission?.CanDelete == true;
        CanExport = permission?.CanExport == true;

        Employees.Clear();
        var list = await employeeService.GetAsync(SearchText);
        foreach (var employee in list) Employees.Add(employee);
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (!CanCreate) return;
        await employeeService.SaveAsync(SelectedEmployee);
        SelectedEmployee = new Employee();
        await LoadAsync();
    }

    [RelayCommand]
    public async Task DeleteAsync(Employee employee)
    {
        if (!CanDelete) return;
        await employeeService.DeleteAsync(employee.EmployeeID);
        await LoadAsync();
    }
}
