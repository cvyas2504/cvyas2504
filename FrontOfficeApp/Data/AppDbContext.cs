using FrontOfficeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<DutyRoster> DutyRoster => Set<DutyRoster>();
    public DbSet<ExcelComparison> ExcelCompareResults => Set<ExcelComparison>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleID);

        modelBuilder.Entity<Permission>()
            .HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleID);

        modelBuilder.Entity<DutyRoster>()
            .HasOne(d => d.Employee)
            .WithMany()
            .HasForeignKey(d => d.EmployeeID);

        modelBuilder.Entity<DutyRoster>()
            .HasIndex(d => new { d.EmployeeID, d.DutyDate })
            .IsUnique();
    }
}
