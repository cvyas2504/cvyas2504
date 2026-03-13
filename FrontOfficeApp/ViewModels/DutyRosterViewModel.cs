using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class DutyRosterViewModel(IDutyRosterService rosterService, IEmployeeService employeeService) : BaseViewModel
{
    [ObservableProperty] private DateTime startDate = DateTime.Today;
    [ObservableProperty] private DateTime endDate = DateTime.Today.AddDays(7);
    [ObservableProperty] private string selectedDutyType = nameof(ShiftType.G);
    [ObservableProperty] private DutyRoster selectedRoster = new() { DutyDate = DateTime.Today, DutyType = ShiftType.G };
    public ObservableCollection<DutyRoster> Rosters { get; } = new();

    [RelayCommand]
    private async Task LoadAsync()
    {
        Rosters.Clear();
        var items = await rosterService.GetByRangeAsync(StartDate, EndDate);
        foreach (var item in items) Rosters.Add(item);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Enum.TryParse<ShiftType>(SelectedDutyType, out var duty))
            SelectedRoster.DutyType = duty;

        await rosterService.SaveAsync(SelectedRoster);
        SelectedRoster = new DutyRoster { DutyDate = DateTime.Today, DutyType = ShiftType.G };
        await LoadAsync();
    }

    [RelayCommand]
    private async Task AutoRotateAsync()
    {
        var employees = await employeeService.GetAsync();
        await rosterService.AutoRotateAsync(StartDate, EndDate, employees);
        await LoadAsync();
    }
}
