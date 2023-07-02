using System.Security.Claims;
using lets_leave.Dto.AuthDto;
using lets_leave.Dto.CompanyDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.AuthService;
using lets_leave.Services.CompanyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lets_leave.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICompanyService _companyService;

    public AuthController(IAuthService authService, ICompanyService companyService)
    {
        _authService = authService;
        _companyService = companyService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServerResponse<Auth>>> Register(RegisterDto registerDto)
    {
        var response = await _authService.Register(registerDto);
        if (response.Success) return Ok(response);
        if (response.Message.Contains("is already taken") || response.Message.Contains("Company already exists"))
            return Conflict(response);
        return BadRequest(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServerResponse<Auth>>> Login(LoginDto loginDto)
    {
        var response = await _authService.Login(loginDto);
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("User does not exists") => NotFound(response),
            false => BadRequest(response)
        };
    }

    
}