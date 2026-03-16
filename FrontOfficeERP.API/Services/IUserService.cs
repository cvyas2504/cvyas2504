using FrontOfficeERP.API.DTOs;

namespace FrontOfficeERP.API.Services;

public interface IUserService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task<UserDto> CreateUserAsync(CreateUserRequestDto request);
    Task<List<UserDto>> GetUsersAsync();
}
