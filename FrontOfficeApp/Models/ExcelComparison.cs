using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class ExcelComparison
{
    [Key] public int ComparisonID { get; set; }
    public int UserID { get; set; }
    public string ReferenceFile { get; set; } = string.Empty;
    public string DataFile { get; set; } = string.Empty;
    public int TotalRecords { get; set; }
    public int Matched { get; set; }
    public int Mismatch { get; set; }
    public int Missing { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}
