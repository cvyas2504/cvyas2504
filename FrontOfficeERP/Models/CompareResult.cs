namespace FrontOfficeERP.Models;

public class CompareResult
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string File1Value { get; set; } = string.Empty;
    public string File2Value { get; set; } = string.Empty;
}
