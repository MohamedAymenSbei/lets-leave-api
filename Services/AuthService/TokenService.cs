using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using lets_leave.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace lets_leave.Services.AuthService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private const int ExpirationDays = 1;

    public TokenService(IConfiguration configuration, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
    {
        _configuration = configuration;
        _userManager = userManager;
        _contextAccessor = contextAccessor;
    }

    public async Task<string> CreateToken(User user)
    {
        var expiration = DateTime.Now.AddDays(ExpirationDays);
        var token = CreateJwtToken(
            await CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string? GetUserClaimsId(ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }

    public string? GetUserId()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return userId?.Value;
    }

    private JwtSecurityToken CreateJwtToken(
        IEnumerable<Claim> claims,
        SigningCredentials credentials,
        DateTime expiration
    )
    {
        return new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private async Task<Claim[]> CreateClaims(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new Claim[]
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, roles.First())
        };
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256
        );
    }
}