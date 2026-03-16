using FrontOfficeERP.API.Models;

namespace FrontOfficeERP.API.Auth;

public interface ITokenService
{
    string GenerateToken(User user, string roleName);
}
