using FrontOfficeERP.API.DTOs;
using FrontOfficeERP.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontOfficeERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExcelCompareController(IExcelCompareService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ExcelCompareResultDto>> Compare([FromBody] ExcelCompareRequestDto request)
        => Ok(await service.CompareAsync(request));
}
