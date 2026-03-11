using FrontOfficeERP.Models;
using OfficeOpenXml;

namespace FrontOfficeERP.Services;

public class ExcelCompareService
{
    public List<CompareResult> Compare(string file1Path, string file2Path)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var results = new List<CompareResult>();

        using var p1 = new ExcelPackage(new FileInfo(file1Path));
        using var p2 = new ExcelPackage(new FileInfo(file2Path));

        var s1 = p1.Workbook.Worksheets[0];
        var s2 = p2.Workbook.Worksheets[0];

        var maxRows = Math.Max(s1.Dimension.End.Row, s2.Dimension.End.Row);
        var maxCols = Math.Max(s1.Dimension.End.Column, s2.Dimension.End.Column);

        for (var r = 1; r <= maxRows; r++)
        {
            for (var c = 1; c <= maxCols; c++)
            {
                var v1 = s1.Cells[r, c].Text;
                var v2 = s2.Cells[r, c].Text;

                if (!string.Equals(v1, v2, StringComparison.Ordinal))
                {
                    results.Add(new CompareResult
                    {
                        Row = r,
                        Column = c,
                        File1Value = v1,
                        File2Value = v2
                    });
                }
            }
        }

        return results;
    }
}
