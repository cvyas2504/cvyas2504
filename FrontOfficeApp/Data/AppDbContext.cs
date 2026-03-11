using FrontOfficeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<DutyRoster> DutyRoster => Set<DutyRoster>();
    public DbSet<ExcelComparison> ExcelComparisons => Set<ExcelComparison>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<DutyRoster>()
            .HasOne(d => d.Employee)
            .WithMany()
            .HasForeignKey(d => d.EmployeeID);
    }
}
