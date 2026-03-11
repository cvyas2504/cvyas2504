using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class ReportsViewModel(IReportService reportService) : BaseViewModel
{
    public ObservableCollection<Report> Reports { get; } = new();

    [RelayCommand]
    private async Task LoadAsync()
    {
        Reports.Clear();
        var reports = await reportService.GetReportsAsync();
        foreach (var report in reports) Reports.Add(report);
    }
}
