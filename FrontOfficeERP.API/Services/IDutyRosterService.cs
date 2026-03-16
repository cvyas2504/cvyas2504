using FrontOfficeERP.API.DTOs;

namespace FrontOfficeERP.API.Services;

public interface IDutyRosterService
{
    Task<DutyRosterDto> CreateAsync(DutyRosterCreateDto request);
    Task<List<DutyRosterDto>> GetMonthlyAsync(int year, int month);
}
