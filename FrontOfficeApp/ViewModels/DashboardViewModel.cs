using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class DashboardViewModel(IEmployeeService employeeService, IDutyRosterService rosterService, ISessionService sessionService, IAuthService authService) : BaseViewModel
{
    [ObservableProperty] private int totalEmployees;
    [ObservableProperty] private int todaysDutySchedule;
    [ObservableProperty] private int excelComparisonsToday;
    [ObservableProperty] private string weeklyReports = "0";
    [ObservableProperty] private string systemNotifications = "All systems healthy";

    [RelayCommand]
    private async Task LoadAsync()
    {
        var employees = await employeeService.GetAsync();
        TotalEmployees = employees.Count;
        TodaysDutySchedule = (await rosterService.GetByRangeAsync(DateTime.Today, DateTime.Today)).Count;
    }

    [RelayCommand]
    private async Task NavigateAsync(string route) => await Shell.Current.GoToAsync(route);

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await authService.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }

    public bool CanManageUsers => sessionService.CurrentUser?.Role == Models.UserRole.Admin;
}
