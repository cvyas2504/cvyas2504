using FrontOfficeERP.API.DTOs;
using FrontOfficeERP.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FrontOfficeERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        var response = await userService.LoginAsync(request);
        if (response is null) return Unauthorized("Invalid credentials or inactive user.");
        return Ok(response);
    }
}
