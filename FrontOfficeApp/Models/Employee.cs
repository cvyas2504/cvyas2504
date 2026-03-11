using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class Employee
{
    [Key] public int EmployeeID { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; } = DateTime.UtcNow;
    public bool Status { get; set; } = true;
}
