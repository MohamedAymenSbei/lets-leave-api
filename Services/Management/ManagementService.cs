using AutoMapper;
using lets_leave.Data;
using lets_leave.Dto.DepartmentDto;
using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.AuthService;
using lets_leave.Services.CompanyService;
using lets_leave.Services.DeparmtentService;
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
    private readonly IDepartmentService _departmentService;
    private readonly ICompanyService _companyService;

    public ManagementService(ITokenService tokenService, UserManager<User> userManager, IMapper mapper,
        AppDbContext dbContext, IMailService mailService, IDepartmentService departmentService,
        ICompanyService companyService)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _mapper = mapper;
        _dbContext = dbContext;
        _mailService = mailService;
        _departmentService = departmentService;
        _companyService = companyService;
    }

    public async Task<ServerResponse<GetUserDto>> AddEmployee(AddEmployeeDto addEmployeeDto, string? depId)
    {
        var response = new ServerResponse<GetUserDto>();

        var companyResponse = await _companyService.GetCompany<Company>();
        if (companyResponse.Data == null)
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
            Company = companyResponse.Data
        };
        var password = GenerateRandomPassword();

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
            var depResponse = await _departmentService.AddDepartmentToUser(user, depId);
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

    public async Task<ServerResponse<GetUserDto>> UpdateRole(string employeeId, bool isHr)
    {
        var response = new ServerResponse<GetUserDto>();

        var companyResponse = await _companyService.GetCompany<Company>(includeUsers: true, includeDepartments: true);
        if (companyResponse.Data == null)
        {
            response.Success = false;
            response.Message = "Company not found";
            return response;
        }

        var employee = companyResponse.Data.Users.FirstOrDefault(u => u.Id == employeeId);
        if (employee == null)
        {
            response.Success = false;
            response.Message = "Employee not found";
            return response;
        }

        string role;
        if (isHr)
        {
            // Add HR role and remove Employee role
            await _userManager.AddToRoleAsync(employee, nameof(Roles.HumanRecourses));
            await _userManager.RemoveFromRoleAsync(employee, nameof(Roles.Employee));
            role = nameof(Roles.HumanRecourses);
        }
        else
        {
            // Add Employee role and remove HR role
            await _userManager.AddToRoleAsync(employee, nameof(Roles.Employee));
            await _userManager.RemoveFromRoleAsync(employee, nameof(Roles.HumanRecourses));
            role = nameof(Roles.Employee);
        }

        // Update the user in the database
        await _userManager.UpdateAsync(employee);
        var userDto = _mapper.Map<GetUserDto>(employee);
        userDto.Role = role;
        response.Data = userDto;
        return response;
    }

    public async Task<ServerResponse<List<UserLeaveRequestDto>>> GetLeaveRequests()
    {
        var response = new ServerResponse<List<UserLeaveRequestDto>>();
        var companyResponse = await _companyService.GetCompany<Company>();
        if (companyResponse.Data == null)
        {
            response.Success = false;
            response.Message = companyResponse.Message;
            return response;
        }

        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var leaveRequests = await _dbContext.LeaveRequests
            .Include(lr => lr.User)
            .ThenInclude(u => u.Company)
            .ThenInclude(u => u.Departments)
            .Where(lr => lr.User.Company.Id == companyResponse.Data.Id)
            .Where(lr => lr.User.Id != userId)
            .ToListAsync();

        var requestsDto = new List<UserLeaveRequestDto>();

        foreach (var request in leaveRequests)
        {
            var userDto = _mapper.Map<GetUserDto>(request.User);
            userDto.Department = _mapper.Map<GetDepartmentDto>(request.User.Department);
            var roles = await _userManager.GetRolesAsync(request.User);
            userDto.Role = roles.First();

            var requestDto = _mapper.Map<UserLeaveRequestDto>(request);
            requestDto.User = userDto;
            requestsDto.Add(requestDto);
        }

        response.Data = requestsDto;
        return response;
    }

    public async Task<ServerResponse<GetLeaveDto>> UpdateLeaveRequestStatus(string requestId, LeaveStatus status)
    {
        var response = new ServerResponse<GetLeaveDto>();

        var companyResponse = await _companyService.GetCompany<Company>();
        if (companyResponse.Data == null)
        {
            response.Success = false;
            response.Message = companyResponse.Message;
            return response;
        }

        var request = await _dbContext.LeaveRequests
            .Include(lr => lr.User)
            .ThenInclude(u => u.Company)
            .Where(lr => lr.User.Company.Id == companyResponse.Data.Id)
            .FirstOrDefaultAsync(lr=>lr.Id.ToString() == requestId);
        if (request == null)
        {
            response.Success = false;
            response.Message = "Leave request not found";
            return response;
        }

        request.Status = status;
        request.UpdatedAt = DateTime.Now;
        _dbContext.Update(request);
        await _dbContext.SaveChangesAsync();
        response.Data = _mapper.Map<GetLeaveDto>(request);
        return response;
    }

    public async Task<ServerResponse<List<GetUserDto>>> GetEmployees()
    {
        var response = new ServerResponse<List<GetUserDto>>();

        var companyResponse = await _companyService.GetCompany<Company>();
        if (companyResponse.Data == null)
        {
            response.Success = false;
            response.Message = companyResponse.Message;
            return response;
        }

        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }
        var employees = await _dbContext.Users
            .Include(u => u.Company)
            .Include(u => u.Department)
            .Where(u=>u.Id != userId)
            .Where(u => u.Company.Id == companyResponse.Data.Id)
            .ToListAsync();

        var employeesDto = new List<GetUserDto>();

        foreach (var employee in employees)
        {
            var userDto = _mapper.Map<GetUserDto>(employee);
            userDto.Department = _mapper.Map<GetDepartmentDto>(employee.Department);
            var roles = await _userManager.GetRolesAsync(employee);
            userDto.Role = roles.First();
            employeesDto.Add(userDto);
        }

        response.Data = employeesDto.ToList();
        return response;
    }


    private static string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        const int passwordLength = 8;

        var random = new Random();

        var password = new string(Enumerable.Repeat(chars, passwordLength)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return password;
    }
}