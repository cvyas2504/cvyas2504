namespace FrontOfficeERP.API.Models;

public class ExcelCompareLog
{
    public int CompareId { get; set; }
    public string File1Name { get; set; } = string.Empty;
    public string File2Name { get; set; } = string.Empty;
    public DateTime ComparedDate { get; set; } = DateTime.UtcNow;
    public string ResultSummary { get; set; } = string.Empty;
}
