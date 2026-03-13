using FrontOfficeApp.Models;
using FrontOfficeApp.Services;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Data;

public interface IDataSeeder
{
    Task SeedAsync();
}

public class DataSeeder(AppDbContext db, IPasswordHasher hasher) : IDataSeeder
{
    public async Task SeedAsync()
    {
        await db.Database.EnsureCreatedAsync();

        if (!await db.Roles.AnyAsync())
        {
            db.Roles.AddRange(
                new Role { RoleName = nameof(UserRole.Admin) },
                new Role { RoleName = nameof(UserRole.Manager) },
                new Role { RoleName = nameof(UserRole.Operator) },
                new Role { RoleName = nameof(UserRole.Viewer) }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Permissions.AnyAsync())
        {
            foreach (var role in await db.Roles.ToListAsync())
            {
                foreach (var module in Enum.GetValues<ModuleType>())
                {
                    db.Permissions.Add(new Permission
                    {
                        RoleID = role.RoleID,
                        Module = module,
                        CanCreate = role.RoleName is nameof(UserRole.Admin) or nameof(UserRole.Manager),
                        CanEdit = role.RoleName is nameof(UserRole.Admin) or nameof(UserRole.Manager),
                        CanDelete = role.RoleName == nameof(UserRole.Admin),
                        CanView = true,
                        CanExport = role.RoleName != nameof(UserRole.Viewer)
                    });
                }
            }
        }

        if (!await db.Users.AnyAsync())
        {
            var adminRole = await db.Roles.FirstAsync(x => x.RoleName == nameof(UserRole.Admin));
            var managerRole = await db.Roles.FirstAsync(x => x.RoleName == nameof(UserRole.Manager));
            var operatorRole = await db.Roles.FirstAsync(x => x.RoleName == nameof(UserRole.Operator));

            db.Users.AddRange(
                new User { Username = "admin", PasswordHash = hasher.Hash("Admin@123"), RoleID = adminRole.RoleID, IsActive = true },
                new User { Username = "manager", PasswordHash = hasher.Hash("Manager@123"), RoleID = managerRole.RoleID, IsActive = true },
                new User { Username = "operator", PasswordHash = hasher.Hash("Operator@123"), RoleID = operatorRole.RoleID, IsActive = true }
            );
        }

        if (!await db.Employees.AnyAsync())
        {
            db.Employees.AddRange(
                new Employee { EmployeeName = "Ravi", Department = "Front Office", Designation = "Executive", Email = "ravi@office.local", Phone = "555-011", Address = "HQ", JoiningDate = DateTime.Today.AddYears(-2), Status = true },
                new Employee { EmployeeName = "Suresh", Department = "Operations", Designation = "Coordinator", Email = "suresh@office.local", Phone = "555-012", Address = "Branch", JoiningDate = DateTime.Today.AddYears(-1), Status = true }
            );
        }

        await db.SaveChangesAsync();
    }
}
