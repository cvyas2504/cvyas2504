using FrontOfficeERP.API.Data;
using FrontOfficeERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeERP.API.Repositories;

public class DutyRosterRepository(AppDbContext context) : IDutyRosterRepository
{
    public Task<bool> ExistsAsync(int employeeId, DateTime dutyDate)
        => context.DutyRosters.AnyAsync(x => x.EmployeeId == employeeId && x.DutyDate.Date == dutyDate.Date);

    public async Task<DutyRoster> AddAsync(DutyRoster roster)
    {
        context.DutyRosters.Add(roster);
        await context.SaveChangesAsync();
        return roster;
    }

    public Task<List<DutyRoster>> GetMonthlyAsync(int year, int month)
        => context.DutyRosters
            .Include(x => x.Employee)
            .Include(x => x.ShiftType)
            .Where(x => x.DutyDate.Year == year && x.DutyDate.Month == month)
            .AsNoTracking()
            .ToListAsync();
}
