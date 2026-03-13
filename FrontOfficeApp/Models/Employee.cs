using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontOfficeApp.Models;

public class Employee
{
    [Key] public int EmployeeID { get; set; }
    [Required] public string EmployeeName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; } = DateTime.UtcNow;
    public bool Status { get; set; } = true;

    [NotMapped]
    public string Name
    {
        get => EmployeeName;
        set => EmployeeName = value;
    }
}
