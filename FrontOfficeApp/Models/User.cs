using System.ComponentModel.DataAnnotations;

namespace FrontOfficeApp.Models;

public class User
{
    [Key] public int UserID { get; set; }
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string PasswordHash { get; set; } = string.Empty;
    public int RoleID { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Role? Role { get; set; }
}

public class Role
{
    [Key] public int RoleID { get; set; }
    [Required] public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}

public class Permission
{
    [Key] public int PermissionID { get; set; }
    public int RoleID { get; set; }
    public ModuleType Module { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanView { get; set; }
    public bool CanExport { get; set; }

    public Role? Role { get; set; }
}
