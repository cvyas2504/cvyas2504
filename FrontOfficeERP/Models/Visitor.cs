namespace FrontOfficeERP.Models;

public class Visitor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string EmployeeToMeet { get; set; } = string.Empty;
}
