using FrontOfficeERP.API.DTOs;
using FrontOfficeERP.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontOfficeERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DutyRosterController(IDutyRosterService dutyRosterService) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<ActionResult<DutyRosterDto>> Create(DutyRosterCreateDto request)
        => Ok(await dutyRosterService.CreateAsync(request));

    [HttpGet("monthly")]
    public async Task<ActionResult<List<DutyRosterDto>>> GetMonthly([FromQuery] int year, [FromQuery] int month)
        => Ok(await dutyRosterService.GetMonthlyAsync(year, month));
}
