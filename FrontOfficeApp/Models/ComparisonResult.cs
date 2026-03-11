namespace FrontOfficeApp.Models;

public class ComparisonResult
{
    public string ReferenceValue { get; set; } = string.Empty;
    public string DataValue { get; set; } = string.Empty;
    public MatchStatus MatchStatus { get; set; }
    public string Remarks { get; set; } = string.Empty;
}

public class ComparisonSummary
{
    public int TotalRecords { get; set; }
    public int Matched { get; set; }
    public int Mismatch { get; set; }
    public int Missing { get; set; }
}
