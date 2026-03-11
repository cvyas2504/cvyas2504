using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class Report
{
    [Key] public int ReportID { get; set; }
    public string ReportType { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
