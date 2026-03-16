using FrontOfficeERP.API.Data;
using FrontOfficeERP.API.DTOs;
using FrontOfficeERP.API.Models;
using FrontOfficeERP.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeERP.API.Services;

public class DutyRosterService(IDutyRosterRepository repository, AppDbContext context) : IDutyRosterService
{
    public async Task<DutyRosterDto> CreateAsync(DutyRosterCreateDto request)
    {
        if (await repository.ExistsAsync(request.EmployeeId, request.DutyDate))
        {
            throw new InvalidOperationException("Shift already assigned for this employee on this date.");
        }

        var roster = new DutyRoster
        {
            EmployeeId = request.EmployeeId,
            ShiftId = request.ShiftId,
            DutyDate = request.DutyDate.Date
        };

        await repository.AddAsync(roster);

        var loaded = await context.DutyRosters
            .Include(x => x.Employee)
            .Include(x => x.ShiftType)
            .FirstAsync(x => x.RosterId == roster.RosterId);

        return new DutyRosterDto(loaded.RosterId, loaded.EmployeeId, loaded.Employee?.EmployeeName ?? string.Empty,
            loaded.ShiftId, loaded.ShiftType?.ShiftName ?? string.Empty, loaded.DutyDate);
    }

    public async Task<List<DutyRosterDto>> GetMonthlyAsync(int year, int month)
    {
        var rows = await repository.GetMonthlyAsync(year, month);
        return rows.Select(x => new DutyRosterDto(x.RosterId, x.EmployeeId, x.Employee?.EmployeeName ?? string.Empty,
            x.ShiftId, x.ShiftType?.ShiftName ?? string.Empty, x.DutyDate)).ToList();
    }
}
