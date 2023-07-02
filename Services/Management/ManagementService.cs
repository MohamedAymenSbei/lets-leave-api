using AutoMapper;
using lets_leave.Data;
using lets_leave.Dto.DepartmentDto;
using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.AuthService;
using lets_leave.Services.MailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lets_leave.Services.Management;

public class ManagementService : IManagementService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly IMailService _mailService;

    public ManagementService(ITokenService tokenService, UserManager<User> userManager, IMapper mapper,
        AppDbContext dbContext, IMailService mailService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _mapper = mapper;
        _dbContext = dbContext;
        _mailService = mailService;
    }

    public async Task<ServerResponse<GetUserDto>> AddEmployee(AddEmployeeDto addEmployeeDto, string? depId)
    {
        var response = new ServerResponse<GetUserDto>();
        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var company = await _dbContext.Companies
            .FirstOrDefaultAsync(c => c.Users
                .Any(u => u.Id == userId));
        if (company == null)
        {
            response.Success = false;
            response.Message = "Company not found";
            return response;
        }

        if (addEmployeeDto.Role == Roles.CompanyOwner)
        {
            response.Success = false;
            response.Message = "Role not found";
            return response;
        }

        var user = new User
        {
            FirstName = addEmployeeDto.FirstName,
            LastName = addEmployeeDto.LastName,
            Email = addEmployeeDto.Email,
            Company = company
        };
        var password = GenerateRandomPassword();
        Console.WriteLine("password");
        Console.WriteLine(password);
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            response.Success = false;
            response.Message = string.Join(",", result.Errors.Select(error => error.Description));
            return response;
        }

        await _userManager.AddToRoleAsync(user, addEmployeeDto.Role.ToString());

        var userDto = _mapper.Map<GetUserDto>(user);
        userDto.Department = null;
        userDto.Role = addEmployeeDto.Role.ToString();
        if (depId != null)
        {
            var depResponse = await AddDepartmentToUser(user, depId);
            if (!depResponse.Success)
            {
                await _userManager.DeleteAsync(user);
                response.Success = false;
                response.Message = depResponse.Message;
                return response;
            }

            userDto.Department = _mapper.Map<GetDepartmentDto>(depResponse.Data);
        }

        await _mailService.SendCredentials(
            addEmployeeDto.SendToMail,
            user.Email,
            password
        );

        await _dbContext.SaveChangesAsync();

        response.Data = userDto;

        return response;
    }

    public Task<ServerResponse<GetUserDto>> UpdateRole()
    {
        throw new NotImplementedException();
    }

    public Task<ServerResponse<List<GetLeaveDto>>> GetLeaveRequests()
    {
        throw new NotImplementedException();
    }

    public Task<ServerResponse<GetLeaveDto>> UpdateLeaveRequestStatus()
    {
        throw new NotImplementedException();
    }

    private async Task<ServerResponse<Department?>> AddDepartmentToUser(User user, string depId)
    {
        var response = new ServerResponse<Department?>();

        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(d => d.Id.ToString() == depId);
        if (department == null)
        {
            response.Success = false;
            response.Message = "Department not found";
            return response;
        }

        department.Users.Add(user);
        response.Data = department;

        return response;
    }

    public string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        const int passwordLength = 8;

        var random = new Random();

        var password = new string(Enumerable.Repeat(chars, passwordLength)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return password;
    }
}