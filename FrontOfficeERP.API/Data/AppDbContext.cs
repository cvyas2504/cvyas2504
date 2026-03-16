using FrontOfficeERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeERP.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<ShiftType> ShiftTypes => Set<ShiftType>();
    public DbSet<DutyRoster> DutyRosters => Set<DutyRoster>();
    public DbSet<ExcelCompareLog> ExcelCompareLogs => Set<ExcelCompareLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasIndex(x => x.RoleName).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        modelBuilder.Entity<DutyRoster>()
            .HasIndex(x => new { x.EmployeeId, x.DutyDate })
            .IsUnique();

        modelBuilder.Entity<Permission>()
            .HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<DutyRoster>()
            .HasOne(r => r.Employee)
            .WithMany(e => e.DutyRosters)
            .HasForeignKey(r => r.EmployeeId);

        modelBuilder.Entity<DutyRoster>()
            .HasOne(r => r.ShiftType)
            .WithMany(s => s.DutyRosters)
            .HasForeignKey(r => r.ShiftId);

        modelBuilder.Entity<ShiftType>().HasData(
            new ShiftType { ShiftId = 1, ShiftName = "General" },
            new ShiftType { ShiftId = 2, ShiftName = "Morning" },
            new ShiftType { ShiftId = 3, ShiftName = "Evening" },
            new ShiftType { ShiftId = 4, ShiftName = "Night" });
    }
}
