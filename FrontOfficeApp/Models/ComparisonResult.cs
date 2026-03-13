namespace FrontOfficeApp.Models;

public class ComparisonResult
{
    public string Key { get; set; } = string.Empty;
    public string File1Value { get; set; } = string.Empty;
    public string File2Value { get; set; } = string.Empty;
    public MatchStatus Status { get; set; }
}

public class ComparisonSummary
{
    public int TotalRecords { get; set; }
    public int Matched { get; set; }
    public int Mismatch { get; set; }
    public int MissingInFile1 { get; set; }
    public int MissingInFile2 { get; set; }
}
