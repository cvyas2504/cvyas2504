using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Services;

public interface IDutyRosterService
{
    Task<List<DutyRoster>> GetByRangeAsync(DateTime start, DateTime end);
    Task SaveAsync(DutyRoster roster);
    Task AutoRotateAsync(DateTime start, DateTime end, List<Employee> employees);
}

public class DutyRosterService(AppDbContext db) : IDutyRosterService
{
    public Task<List<DutyRoster>> GetByRangeAsync(DateTime start, DateTime end) =>
        db.DutyRoster.Include(r => r.Employee).Where(r => r.Date >= start && r.Date <= end).OrderBy(r => r.Date).ToListAsync();

    public async Task SaveAsync(DutyRoster roster)
    {
        if (roster.RosterID == 0) db.DutyRoster.Add(roster); else db.DutyRoster.Update(roster);
        await db.SaveChangesAsync();
    }

    public async Task AutoRotateAsync(DateTime start, DateTime end, List<Employee> employees)
    {
        var shifts = Enum.GetValues<ShiftType>().Where(s => s != ShiftType.Off).ToArray();
        var days = (end - start).Days + 1;
        for (var i = 0; i < days; i++)
        {
            var date = start.AddDays(i);
            for (var e = 0; e < employees.Count; e++)
            {
                db.DutyRoster.Add(new DutyRoster
                {
                    EmployeeID = employees[e].EmployeeID,
                    Date = date,
                    Department = employees[e].Department,
                    ShiftType = shifts[(i + e) % shifts.Length]
                });
            }
        }
        await db.SaveChangesAsync();
    }
}
