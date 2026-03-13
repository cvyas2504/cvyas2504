using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class DashboardViewModel(IEmployeeService employeeService, IDutyRosterService rosterService, IAuthorizationService authorizationService, IAuthService authService) : BaseViewModel
{
    [ObservableProperty] private int totalEmployees;
    [ObservableProperty] private int morningShiftCount;
    [ObservableProperty] private int eveningShiftCount;
    [ObservableProperty] private int nightShiftCount;
    [ObservableProperty] private int offCount;

    [ObservableProperty] private bool canViewEmployees;
    [ObservableProperty] private bool canViewRoster;
    [ObservableProperty] private bool canViewExcel;
    [ObservableProperty] private bool canViewReports;

    [RelayCommand]
    private async Task LoadAsync()
    {
        var employees = await employeeService.GetAsync();
        TotalEmployees = employees.Count;

        var today = await rosterService.GetByRangeAsync(DateTime.Today, DateTime.Today);
        MorningShiftCount = today.Count(x => x.DutyType == ShiftType.M);
        EveningShiftCount = today.Count(x => x.DutyType == ShiftType.E);
        NightShiftCount = today.Count(x => x.DutyType == ShiftType.N);
        OffCount = today.Count(x => x.DutyType == ShiftType.O);

        CanViewEmployees = await authorizationService.CanViewAsync(ModuleType.Employee);
        CanViewRoster = await authorizationService.CanViewAsync(ModuleType.DutyRoster);
        CanViewExcel = await authorizationService.CanViewAsync(ModuleType.ExcelCompare);
        CanViewReports = await authorizationService.CanViewAsync(ModuleType.Reports);
    }

    [RelayCommand]
    private async Task NavigateAsync(string route) => await Shell.Current.GoToAsync(route);

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await authService.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
