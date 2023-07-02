using lets_leave.Dto.UserDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lets_leave.Controllers;

[Authorize(Roles = nameof(Roles.HumanRecourses) + "," + nameof(Roles.CompanyOwner))]
[ApiController]
[Route("api/[controller]")]
public class ManagementController : ControllerBase
{
    private readonly IManagementService _managementService;

    public ManagementController(IManagementService managementService)
    {
        _managementService = managementService;
    }

    [HttpPost]
    public async Task<ActionResult<ServerResponse<GetUserDto>>> AddEmployee([FromQuery] string? depId, AddEmployeeDto addEmployeeDto)
    {
        var response = await _managementService.AddEmployee(addEmployeeDto, depId);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
}