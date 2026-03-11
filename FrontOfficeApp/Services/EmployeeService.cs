using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetAsync(string? search = null);
    Task SaveAsync(Employee employee);
    Task DeleteAsync(int id);
}

public class EmployeeService(AppDbContext db) : IEmployeeService
{
    public async Task<List<Employee>> GetAsync(string? search = null)
    {
        var query = db.Employees.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e => e.Name.Contains(search) || e.Department.Contains(search) || e.Email.Contains(search));
        return await query.OrderBy(e => e.Name).ToListAsync();
    }

    public async Task SaveAsync(Employee employee)
    {
        if (employee.EmployeeID == 0) db.Employees.Add(employee); else db.Employees.Update(employee);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await db.Employees.FindAsync(id);
        if (entity is null) return;
        db.Employees.Remove(entity);
        await db.SaveChangesAsync();
    }
}
