namespace FrontOfficeERP.API.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public ICollection<DutyRoster> DutyRosters { get; set; } = new List<DutyRoster>();
}
