namespace FrontOfficeERP.API.Models;

public class Permission
{
    public int PermissionId { get; set; }
    public int RoleId { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public Role? Role { get; set; }
}
