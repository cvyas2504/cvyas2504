using ClosedXML.Excel;
using FrontOfficeApp.Data;
using FrontOfficeApp.Models;

namespace FrontOfficeApp.Services;

public interface IExcelComparisonService
{
    Task<(List<ComparisonResult> results, ComparisonSummary summary)> CompareAsync(string referencePath, string dataPath, int columnIndex, int userId);
}

public class ExcelComparisonService(AppDbContext db) : IExcelComparisonService
{
    public async Task<(List<ComparisonResult> results, ComparisonSummary summary)> CompareAsync(string referencePath, string dataPath, int columnIndex, int userId)
    {
        return await Task.Run(async () =>
        {
            var reference = ReadValues(referencePath, columnIndex);
            var data = ReadValues(dataPath, columnIndex);
            var dataLookup = data.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

            var results = new List<ComparisonResult>(reference.Count);
            foreach (var refVal in reference)
            {
                if (!dataLookup.TryGetValue(refVal, out var count))
                {
                    var partial = data.FirstOrDefault(d => d.Contains(refVal, StringComparison.OrdinalIgnoreCase) || refVal.Contains(d, StringComparison.OrdinalIgnoreCase));
                    results.Add(new ComparisonResult
                    {
                        ReferenceValue = refVal,
                        DataValue = partial,
                        MatchStatus = string.IsNullOrWhiteSpace(partial) ? MatchStatus.Missing : MatchStatus.Partial,
                        Remarks = string.IsNullOrWhiteSpace(partial) ? "Missing in data file" : "Partial match"
                    });
                    continue;
                }

                results.Add(new ComparisonResult
                {
                    ReferenceValue = refVal,
                    DataValue = refVal,
                    MatchStatus = count > 1 ? MatchStatus.Duplicate : MatchStatus.Exact,
                    Remarks = count > 1 ? "Duplicate in data file" : "Exact match"
                });
            }

            var summary = new ComparisonSummary
            {
                TotalRecords = reference.Count,
                Matched = results.Count(r => r.MatchStatus is MatchStatus.Exact or MatchStatus.Duplicate),
                Mismatch = results.Count(r => r.MatchStatus is MatchStatus.Partial or MatchStatus.Mismatch),
                Missing = results.Count(r => r.MatchStatus == MatchStatus.Missing)
            };

            db.ExcelComparisons.Add(new ExcelComparison
            {
                UserID = userId,
                ReferenceFile = Path.GetFileName(referencePath),
                DataFile = Path.GetFileName(dataPath),
                TotalRecords = summary.TotalRecords,
                Matched = summary.Matched,
                Mismatch = summary.Mismatch,
                Missing = summary.Missing
            });
            await db.SaveChangesAsync();
            return (results, summary);
        });
    }

    private static List<string> ReadValues(string path, int col)
    {
        using var workbook = new XLWorkbook(path);
        var ws = workbook.Worksheets.First();
        return ws.RowsUsed().Skip(1)
            .Select(r => r.Cell(col).GetString().Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Take(100000)
            .ToList();
    }
}
