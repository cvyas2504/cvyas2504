using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class ExcelComparison
{
    [Key] public int CompareID { get; set; }
    public string KeyValue { get; set; } = string.Empty;
    public string File1Value { get; set; } = string.Empty;
    public string File2Value { get; set; } = string.Empty;
    public MatchStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
