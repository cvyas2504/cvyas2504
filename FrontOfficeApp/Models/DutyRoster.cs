using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class DutyRoster
{
    [Key] public int RosterID { get; set; }
    public int EmployeeID { get; set; }
    public DateTime Date { get; set; }
    public ShiftType ShiftType { get; set; }
    public string Department { get; set; } = string.Empty;
    public Employee? Employee { get; set; }
}
