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
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new User { Username = "admin", PasswordHash = hasher.Hash("Admin@123"), Role = UserRole.Admin },
                new User { Username = "manager", PasswordHash = hasher.Hash("Manager@123"), Role = UserRole.Manager },
                new User { Username = "frontdesk", PasswordHash = hasher.Hash("Front@123"), Role = UserRole.FrontOfficeUser }
            );
        }

        if (!await db.Employees.AnyAsync())
        {
            db.Employees.AddRange(
                new Employee { Name = "Ava Collins", Department = "Front Office", Designation = "Executive", Email = "ava@office.local", Phone = "555-011" },
                new Employee { Name = "Noah Reed", Department = "Operations", Designation = "Coordinator", Email = "noah@office.local", Phone = "555-012" }
            );
        }

        await db.SaveChangesAsync();
    }
}
