namespace FrontOfficeERP.API.DTOs;

public record LoginRequestDto(string Username, string Password);
public record LoginResponseDto(string Token, string Username, string Role);
public record CreateUserRequestDto(string Username, string Password, int RoleId, string Status);
public record UserDto(int UserId, string Username, int RoleId, string RoleName, string Status, DateTime CreatedDate);
