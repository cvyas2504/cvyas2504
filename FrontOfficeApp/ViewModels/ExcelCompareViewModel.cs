using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class ExcelCompareViewModel(IExcelComparisonService comparisonService, IReportService reportService, ISessionService sessionService) : BaseViewModel
{
    [ObservableProperty] private string file1Path = string.Empty;
    [ObservableProperty] private string file2Path = string.Empty;
    [ObservableProperty] private int selectedColumn = 1;
    [ObservableProperty] private string summaryText = string.Empty;

    public ObservableCollection<ComparisonResult> Results { get; } = new();

    [RelayCommand]
    private async Task PickFile1Async()
    {
        var result = await FilePicker.PickAsync();
        if (result != null) File1Path = result.FullPath;
    }

    [RelayCommand]
    private async Task PickFile2Async()
    {
        var result = await FilePicker.PickAsync();
        if (result != null) File2Path = result.FullPath;
    }

    [RelayCommand]
    private async Task CompareAsync()
    {
        if (!File.Exists(File1Path) || !File.Exists(File2Path)) return;
        IsBusy = true;
        Results.Clear();
        var (results, summary) = await comparisonService.CompareAsync(File1Path, File2Path, SelectedColumn, sessionService.CurrentUser?.UserID ?? 0);
        foreach (var row in results) Results.Add(row);
        SummaryText = $"Total: {summary.TotalRecords} | Match: {summary.Matched} | Mismatch: {summary.Mismatch} | Missing File1: {summary.MissingInFile1} | Missing File2: {summary.MissingInFile2}";
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ExportExcelAsync() => await Shell.Current.DisplayAlert("Export", $"Excel exported: {await reportService.ExportComparisonExcelAsync(Results)}", "OK");

    [RelayCommand]
    private async Task ExportCsvAsync() => await Shell.Current.DisplayAlert("Export", $"CSV exported: {await reportService.ExportComparisonCsvAsync(Results)}", "OK");

    [RelayCommand]
    private async Task ExportPdfAsync()
    {
        var summary = new ComparisonSummary
        {
            TotalRecords = Results.Count,
            Matched = Results.Count(x => x.Status == MatchStatus.Match),
            Mismatch = Results.Count(x => x.Status == MatchStatus.Mismatch),
            MissingInFile1 = Results.Count(x => x.Status == MatchStatus.MissingInFile1),
            MissingInFile2 = Results.Count(x => x.Status == MatchStatus.MissingInFile2)
        };
        await Shell.Current.DisplayAlert("Export", $"PDF exported: {await reportService.ExportComparisonPdfAsync(summary)}", "OK");
    }
}
