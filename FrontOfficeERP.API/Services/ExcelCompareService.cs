using FrontOfficeERP.API.Data;
using FrontOfficeERP.API.DTOs;
using FrontOfficeERP.API.Models;

namespace FrontOfficeERP.API.Services;

public class ExcelCompareService(AppDbContext context) : IExcelCompareService
{
    public async Task<ExcelCompareResultDto> CompareAsync(ExcelCompareRequestDto request)
    {
        var differences = new List<ExcelDifferenceDto>();
        var left = request.File1Rows.ToDictionary(x => x["Key"], x => x);
        var right = request.File2Rows.ToDictionary(x => x["Key"], x => x);

        foreach (var key in left.Keys.Except(right.Keys))
            differences.Add(new ExcelDifferenceDto(key, "Removed", "Record only in file1"));

        foreach (var key in right.Keys.Except(left.Keys))
            differences.Add(new ExcelDifferenceDto(key, "Added", "Record only in file2"));

        foreach (var key in left.Keys.Intersect(right.Keys))
        {
            if (!left[key].OrderBy(k => k.Key).SequenceEqual(right[key].OrderBy(k => k.Key)))
            {
                differences.Add(new ExcelDifferenceDto(key, "Modified", "Row values changed"));
            }
        }

        var summary = $"Added: {differences.Count(x => x.ChangeType == "Added")}, Removed: {differences.Count(x => x.ChangeType == "Removed")}, Modified: {differences.Count(x => x.ChangeType == "Modified")}";

        context.ExcelCompareLogs.Add(new ExcelCompareLog
        {
            File1Name = request.File1Name,
            File2Name = request.File2Name,
            ResultSummary = summary,
            ComparedDate = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return new ExcelCompareResultDto(summary, differences);
    }
}
