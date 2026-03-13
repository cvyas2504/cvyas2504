using System.Globalization;
using CsvHelper;
using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using OfficeOpenXml;

namespace FrontOfficeApp.Services;

public interface IExcelComparisonService
{
    Task<(List<ComparisonResult> results, ComparisonSummary summary)> CompareAsync(string file1Path, string file2Path, int columnIndex, int userId);
}

public class ExcelComparisonService(AppDbContext db) : IExcelComparisonService
{
    public async Task<(List<ComparisonResult> results, ComparisonSummary summary)> CompareAsync(string file1Path, string file2Path, int columnIndex, int userId)
    {
        ExcelPackage.License.SetNonCommercialPersonal("FrontOfficeERP");
        var file1 = await ReadValuesAsync(file1Path, columnIndex);
        var file2 = await ReadValuesAsync(file2Path, columnIndex);

        var file2Lookup = file2.GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

        var results = new List<ComparisonResult>();

        foreach (var key in file1)
        {
            if (file2Lookup.TryGetValue(key, out var count))
            {
                results.Add(new ComparisonResult { Key = key, File1Value = key, File2Value = key, Status = count > 1 ? MatchStatus.Mismatch : MatchStatus.Match });
            }
            else
            {
                results.Add(new ComparisonResult { Key = key, File1Value = key, File2Value = string.Empty, Status = MatchStatus.MissingInFile2 });
            }
        }

        foreach (var key in file2.Where(v => !file1.Contains(v, StringComparer.OrdinalIgnoreCase)))
        {
            results.Add(new ComparisonResult { Key = key, File1Value = string.Empty, File2Value = key, Status = MatchStatus.MissingInFile1 });
        }

        var summary = new ComparisonSummary
        {
            TotalRecords = results.Count,
            Matched = results.Count(x => x.Status == MatchStatus.Match),
            Mismatch = results.Count(x => x.Status is MatchStatus.Mismatch or MatchStatus.Difference),
            MissingInFile1 = results.Count(x => x.Status == MatchStatus.MissingInFile1),
            MissingInFile2 = results.Count(x => x.Status == MatchStatus.MissingInFile2)
        };

        db.ExcelCompareResults.AddRange(results.Select(x => new ExcelComparison
        {
            KeyValue = x.Key,
            File1Value = x.File1Value,
            File2Value = x.File2Value,
            Status = x.Status,
            CreatedDate = DateTime.UtcNow
        }));
        await db.SaveChangesAsync();

        return (results, summary);
    }

    private static Task<List<string>> ReadValuesAsync(string path, int col)
    {
        var ext = Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".csv" => Task.FromResult(ReadCsv(path, col)),
            ".xlsx" => Task.FromResult(ReadXlsx(path, col)),
            ".xls" => throw new NotSupportedException("Legacy .xls file needs conversion to .xlsx in this baseline."),
            _ => throw new NotSupportedException("Unsupported file format")
        };
    }

    private static List<string> ReadCsv(string path, int col)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var rows = new List<string>();
        while (csv.Read())
        {
            if (csv.Parser.Row == 1) continue;
            rows.Add(csv.GetField(col - 1)?.Trim() ?? string.Empty);
        }
        return rows.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    }

    private static List<string> ReadXlsx(string path, int col)
    {
        using var package = new ExcelPackage(new FileInfo(path));
        var ws = package.Workbook.Worksheets.First();
        var list = new List<string>();
        for (var row = 2; row <= ws.Dimension.End.Row; row++)
        {
            var value = ws.Cells[row, col].Text?.Trim();
            if (!string.IsNullOrWhiteSpace(value)) list.Add(value);
        }

        return list;
    }
}
