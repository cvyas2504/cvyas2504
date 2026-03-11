using ClosedXML.Excel;
using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Services;

public interface IReportService
{
    Task<List<Report>> GetReportsAsync();
    Task<string> ExportComparisonExcelAsync(IEnumerable<ComparisonResult> results);
    Task<string> ExportComparisonPdfAsync(ComparisonSummary summary);
}

public class ReportService(AppDbContext db) : IReportService
{
    public Task<List<Report>> GetReportsAsync() => db.Reports.OrderByDescending(r => r.CreatedDate).ToListAsync();

    public Task<string> ExportComparisonExcelAsync(IEnumerable<ComparisonResult> results)
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, $"comparison_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Comparison");
        ws.Cell(1, 1).Value = "Reference Value";
        ws.Cell(1, 2).Value = "Data Value";
        ws.Cell(1, 3).Value = "Match Status";
        ws.Cell(1, 4).Value = "Remarks";
        var row = 2;
        foreach (var result in results)
        {
            ws.Cell(row, 1).Value = result.ReferenceValue;
            ws.Cell(row, 2).Value = result.DataValue;
            ws.Cell(row, 3).Value = result.MatchStatus.ToString();
            ws.Cell(row, 4).Value = result.Remarks;
            row++;
        }
        wb.SaveAs(path);
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
        document.Add(new Paragraph($"Missing: {summary.Missing}"));
        return Task.FromResult(path);
    }
}
