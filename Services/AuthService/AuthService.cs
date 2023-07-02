using AutoMapper;
using lets_leave.Dto.AuthDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.CompanyService;
using Microsoft.AspNetCore.Identity;

namespace lets_leave.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly ICompanyService _companyService;

    public AuthService(UserManager<User> userManager, IMapper mapper, ITokenService tokenService,
        ICompanyService companyService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _companyService = companyService;
    }

    public async Task<ServerResponse<Auth>> Register(RegisterDto registerDto)
    {
        var response = new ServerResponse<Auth>();

        //Create user entity
        var user = _mapper.Map<User>(registerDto.UserInformation);
        user.Company = _mapper.Map<Company>(registerDto.CompanyInformation);
        // check if company exists in DB
        var isCompanyExist = await _companyService.IsCompanyExists(user.Company.Email, user.Company.Name);
        if (isCompanyExist)
        {
            response.Success = false;
            response.Message = "Company already exists";
            return response;
        }

        // Create user
        var result = await _userManager.CreateAsync(user, registerDto.UserInformation.Password);
        if (result.Succeeded)
        {
            //Add user role
            await _userManager.AddToRoleAsync(user, nameof(Roles.CompanyOwner));

            //Generate token
            var token = await _tokenService.CreateToken(user);
                
            response.Data = new Auth
            {
                Role = Roles.CompanyOwner,
                Token = token
            };
            response.Success = true;
            response.Message = "User has been created";
        }
        else
        {
            response.Success = false;
            response.Message = string.Join(",", result.Errors.Select(error => error.Description));
            return response;
        }

        return response;
    }

    public async Task<ServerResponse<Auth>> Login(LoginDto loginDto)
    {
        var response = new ServerResponse<Auth>();
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            // User does not exist;
            response.Success = false;
            response.Message = "User does not exists";
            return response;
        }

        // Check user password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!isPasswordValid)
        {
            // Invalid Credentials
            response.Success = false;
            response.Message = "Invalid Credentials";
            return response;
        }

        var token = await _tokenService.CreateToken(user);
        var roles = await _userManager.GetRolesAsync(user);
        response.Success = true;
        response.Data = new Auth
        {
            Role = Enum.Parse<Roles>(roles.First()),
            Token = token
        };


        return response;
    }
}