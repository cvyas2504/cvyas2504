namespace FrontOfficeERP.Models;

public class DutyRoster
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public string Shift { get; set; } = string.Empty;
}
