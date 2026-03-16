namespace FrontOfficeERP.API.Models;

public class ShiftType
{
    public int ShiftId { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public ICollection<DutyRoster> DutyRosters { get; set; } = new List<DutyRoster>();
}
