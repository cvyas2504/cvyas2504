using FrontOfficeApp.Data;
using FrontOfficeApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeApp.Services;

public interface IDutyRosterService
{
    Task<List<DutyRoster>> GetByRangeAsync(DateTime start, DateTime end, string? department = null, int? employeeId = null);
    Task SaveAsync(DutyRoster roster);
    Task AutoRotateAsync(DateTime start, DateTime end, List<Employee> employees);
    Task CopyPreviousMonthAsync(int month, int year, string department);
}

public class DutyRosterService(AppDbContext db, ISessionService sessionService) : IDutyRosterService
{
    public async Task<List<DutyRoster>> GetByRangeAsync(DateTime start, DateTime end, string? department = null, int? employeeId = null)
    {
        var query = db.DutyRoster
            .Include(x => x.Employee)
            .Where(x => x.DutyDate >= start && x.DutyDate <= end);

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(x => x.Department == department);
        if (employeeId.HasValue)
            query = query.Where(x => x.EmployeeID == employeeId);

        return await query.OrderBy(x => x.DutyDate).ThenBy(x => x.EmployeeID).ToListAsync();
    }

    public async Task SaveAsync(DutyRoster roster)
    {
        roster.CreatedBy = sessionService.CurrentUser?.UserID ?? 0;
        if (roster.RosterID == 0) db.DutyRoster.Add(roster); else db.DutyRoster.Update(roster);
        await db.SaveChangesAsync();
    }

    public async Task AutoRotateAsync(DateTime start, DateTime end, List<Employee> employees)
    {
        var shifts = new[] { ShiftType.G, ShiftType.M, ShiftType.E, ShiftType.N, ShiftType.O };
        var days = (end.Date - start.Date).Days + 1;

        for (var i = 0; i < days; i++)
        {
            var date = start.Date.AddDays(i);
            for (var e = 0; e < employees.Count; e++)
            {
                db.DutyRoster.Add(new DutyRoster
                {
                    EmployeeID = employees[e].EmployeeID,
                    DutyDate = date,
                    Department = employees[e].Department,
                    DutyType = shifts[(i + e) % shifts.Length],
                    CreatedBy = sessionService.CurrentUser?.UserID ?? 0
                });
            }
        }

        await db.SaveChangesAsync();
    }

    public async Task CopyPreviousMonthAsync(int month, int year, string department)
    {
        var targetStart = new DateTime(year, month, 1);
        var previousStart = targetStart.AddMonths(-1);
        var previousEnd = previousStart.AddMonths(1).AddDays(-1);
        var previous = await db.DutyRoster
            .Where(x => x.Department == department && x.DutyDate >= previousStart && x.DutyDate <= previousEnd)
            .ToListAsync();

        foreach (var item in previous)
        {
            db.DutyRoster.Add(new DutyRoster
            {
                EmployeeID = item.EmployeeID,
                Department = item.Department,
                DutyDate = new DateTime(year, month, Math.Min(item.DutyDate.Day, DateTime.DaysInMonth(year, month))),
                DutyType = item.DutyType,
                CreatedBy = sessionService.CurrentUser?.UserID ?? 0
            });
        }

        await db.SaveChangesAsync();
    }
}
