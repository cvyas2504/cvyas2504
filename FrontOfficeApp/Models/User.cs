using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class User
{
    [Key] public int UserID { get; set; }
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    [Required] public UserRole Role { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
