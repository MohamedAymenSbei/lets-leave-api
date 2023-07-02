using System.Security.Claims;
using lets_leave.Models;

namespace lets_leave.Services.AuthService;

public interface ITokenService
{
    Task<string> CreateToken(User user);
    string? GetUserClaimsId(ClaimsPrincipal user);
    string? GetUserId();
}