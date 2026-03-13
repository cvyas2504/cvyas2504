using CsvHelper;
using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;

namespace FrontOfficeApp.Services;

public interface IReportService
{
    Task<List<Report>> GetReportsAsync();
    Task<string> ExportComparisonExcelAsync(IEnumerable<ComparisonResult> results);
    Task<string> ExportComparisonPdfAsync(ComparisonSummary summary);
    Task<string> ExportComparisonCsvAsync(IEnumerable<ComparisonResult> results);
}

public class ReportService(AppDbContext db) : IReportService
{
    public Task<List<Report>> GetReportsAsync() => db.Reports.OrderByDescending(r => r.CreatedDate).ToListAsync();

    public Task<string> ExportComparisonExcelAsync(IEnumerable<ComparisonResult> results)
    {
        ExcelPackage.License.SetNonCommercialPersonal("FrontOfficeERP");
        var path = Path.Combine(FileSystem.AppDataDirectory, $"comparison_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Comparison");
        ws.Cells[1, 1].Value = "Key";
        ws.Cells[1, 2].Value = "File1 Value";
        ws.Cells[1, 3].Value = "File2 Value";
        ws.Cells[1, 4].Value = "Status";

        var row = 2;
        foreach (var result in results)
        {
            ws.Cells[row, 1].Value = result.Key;
            ws.Cells[row, 2].Value = result.File1Value;
            ws.Cells[row, 3].Value = result.File2Value;
            ws.Cells[row, 4].Value = result.Status.ToString();
            row++;
        }

        package.SaveAs(new FileInfo(path));
        return Task.FromResult(path);
    }

    public Task<string> ExportComparisonCsvAsync(IEnumerable<ComparisonResult> results)
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, $"comparison_{DateTime.Now:yyyyMMddHHmmss}.csv");
        using var writer = new StreamWriter(path);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(results);
        return Task.FromResult(path);
    }

    public Task<string> ExportComparisonPdfAsync(ComparisonSummary summary)
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, $"comparison_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        using var writer = new PdfWriter(path);
        using var pdf = new PdfDocument(writer);
        using var document = new Document(pdf);
        document.Add(new Paragraph("Excel Comparison Summary"));
        document.Add(new Paragraph($"Total Records: {summary.TotalRecords}"));
        document.Add(new Paragraph($"Matched: {summary.Matched}"));
        document.Add(new Paragraph($"Mismatch: {summary.Mismatch}"));
        document.Add(new Paragraph($"Missing in File1: {summary.MissingInFile1}"));
        document.Add(new Paragraph($"Missing in File2: {summary.MissingInFile2}"));
        return Task.FromResult(path);
    }
}
