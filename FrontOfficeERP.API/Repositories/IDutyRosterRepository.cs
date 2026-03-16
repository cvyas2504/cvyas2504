using FrontOfficeERP.API.Models;

namespace FrontOfficeERP.API.Repositories;

public interface IDutyRosterRepository
{
    Task<bool> ExistsAsync(int employeeId, DateTime dutyDate);
    Task<DutyRoster> AddAsync(DutyRoster roster);
    Task<List<DutyRoster>> GetMonthlyAsync(int year, int month);
}
