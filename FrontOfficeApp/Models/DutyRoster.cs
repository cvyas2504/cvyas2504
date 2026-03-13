using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontOfficeApp.Models;

public class DutyRoster
{
    [Key] public int RosterID { get; set; }
    public int EmployeeID { get; set; }
    public DateTime DutyDate { get; set; }
    public ShiftType DutyType { get; set; }
    public string Department { get; set; } = string.Empty;
    public int CreatedBy { get; set; }

    public Employee? Employee { get; set; }

    [NotMapped]
    public DateTime Date
    {
        get => DutyDate;
        set => DutyDate = value;
    }

    [NotMapped]
    public ShiftType ShiftType
    {
        get => DutyType;
        set => DutyType = value;
    }
}
