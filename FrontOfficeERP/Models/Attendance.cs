namespace FrontOfficeERP.Models;

public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
}
