using lets_leave.Dto.AuthDto;
using lets_leave.Dto.UserDto;
using lets_leave.Models;

namespace lets_leave.Services.AuthService;

public interface IAuthService
{
    Task<ServerResponse<Auth>> Register(RegisterDto registerDto);
    Task<ServerResponse<Auth>> Login(LoginDto loginDto);
    Task<ServerResponse<GetUserDto>> GetUserInfo();
}