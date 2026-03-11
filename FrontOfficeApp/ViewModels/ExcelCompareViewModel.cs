using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FrontOfficeApp.Models;
using FrontOfficeApp.Services;

namespace FrontOfficeApp.ViewModels;

public partial class ExcelCompareViewModel(IExcelComparisonService comparisonService, IReportService reportService, ISessionService sessionService) : BaseViewModel
{
    [ObservableProperty] private string referenceFile = string.Empty;
    [ObservableProperty] private string dataFile = string.Empty;
    [ObservableProperty] private int selectedColumn = 1;
    [ObservableProperty] private string summaryText = string.Empty;
    [ObservableProperty] private bool showSummary;
    [ObservableProperty] private ComparisonSummary? currentSummary;

    public ObservableCollection<ComparisonResult> Results { get; } = new();

    [RelayCommand]
    private async Task PickReferenceAsync()
    {
        var result = await FilePicker.PickAsync();
        if (result != null) ReferenceFile = result.FullPath;
    }

    [RelayCommand]
    private async Task PickDataAsync()
    {
        var result = await FilePicker.PickAsync();
        if (result != null) DataFile = result.FullPath;
    }

    [RelayCommand]
    private async Task CompareAsync()
    {
        if (!File.Exists(ReferenceFile) || !File.Exists(DataFile)) return;
        IsBusy = true;
        Results.Clear();
        var (results, summary) = await comparisonService.CompareAsync(ReferenceFile, DataFile, SelectedColumn, sessionService.CurrentUser?.UserID ?? 0);
        foreach (var row in results) Results.Add(row);
        CurrentSummary = summary;
        SummaryText = $"Total Records: {summary.TotalRecords}\nMatched: {summary.Matched}\nMismatch: {summary.Mismatch}\nMissing: {summary.Missing}";
        ShowSummary = true;
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ExportExcelAsync()
    {
        var path = await reportService.ExportComparisonExcelAsync(Results);
        await Shell.Current.DisplayAlert("Export", $"Excel exported: {path}", "OK");
    }

    [RelayCommand]
    private async Task ExportPdfAsync()
    {
        if (CurrentSummary is null) return;
        var path = await reportService.ExportComparisonPdfAsync(CurrentSummary);
        await Shell.Current.DisplayAlert("Export", $"PDF exported: {path}", "OK");
    }
}
