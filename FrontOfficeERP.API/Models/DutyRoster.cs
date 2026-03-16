namespace FrontOfficeERP.API.Models;

public class DutyRoster
{
    public int RosterId { get; set; }
    public int EmployeeId { get; set; }
    public int ShiftId { get; set; }
    public DateTime DutyDate { get; set; }
    public Employee? Employee { get; set; }
    public ShiftType? ShiftType { get; set; }
}
