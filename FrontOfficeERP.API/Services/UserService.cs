using FrontOfficeERP.API.Auth;
using FrontOfficeERP.API.Data;
using FrontOfficeERP.API.DTOs;
using FrontOfficeERP.API.Models;
using FrontOfficeERP.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FrontOfficeERP.API.Services;

public class UserService(
    IUserRepository userRepository,
    AppDbContext context,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IUserService
{
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username);
        if (user?.Role is null || user.Status != "Active") return null;
        if (!passwordHasher.Verify(request.Password, user.PasswordHash)) return null;

        var token = tokenService.GenerateToken(user, user.Role.RoleName);
        return new LoginResponseDto(token, user.Username, user.Role.RoleName);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequestDto request)
    {
        var role = await context.Roles.FirstAsync(x => x.RoleId == request.RoleId);
        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHasher.Hash(request.Password),
            RoleId = request.RoleId,
            Status = request.Status
        };

        await userRepository.AddAsync(user);

        return new UserDto(user.UserId, user.Username, user.RoleId, role.RoleName, user.Status, user.CreatedDate);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await userRepository.GetAllAsync();
        return users
            .Select(x => new UserDto(x.UserId, x.Username, x.RoleId, x.Role?.RoleName ?? string.Empty, x.Status, x.CreatedDate))
            .ToList();
    }
}
