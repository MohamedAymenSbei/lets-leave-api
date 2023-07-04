using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lets_leave.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManagementController : ControllerBase
{
    private readonly IManagementService _managementService;

    public ManagementController(IManagementService managementService)
    {
        _managementService = managementService;
    }

    [Authorize(Roles = nameof(Roles.HumanRecourses) + "," + nameof(Roles.CompanyOwner))]
    [HttpGet("LeaveRequests")]
    public async Task<ActionResult<ServerResponse<List<UserLeaveRequestDto>>>> GetLeaveRequests()
    {
        var response = await _managementService.GetLeaveRequests();
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    [Authorize(Roles = nameof(Roles.HumanRecourses) + "," + nameof(Roles.CompanyOwner))]
    [HttpPatch("LeaveRequests")]
    public async Task<ActionResult<ServerResponse<GetLeaveDto>>> UpdateLeaveRequests([FromQuery] string requestId,
        [FromQuery] LeaveStatus status)
    {
        var response = await _managementService.UpdateLeaveRequestStatus(requestId,status);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }

    [Authorize(Roles = nameof(Roles.HumanRecourses) + "," + nameof(Roles.CompanyOwner))]
    [HttpPost("AddEmployee")]
    public async Task<ActionResult<ServerResponse<GetUserDto>>> AddEmployee([FromQuery] string? depId,
        AddEmployeeDto addEmployeeDto)
    {
        var response = await _managementService.AddEmployee(addEmployeeDto, depId);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }

    [Authorize(Roles = nameof(Roles.CompanyOwner))]
    [HttpPatch("UpdateEmployeeRole")]
    public async Task<ActionResult<ServerResponse<GetUserDto>>> UpdateRole([FromQuery] bool isHr, [FromQuery] string id)
    {
        var response = await _managementService.UpdateRole(employeeId: id, isHr);
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
    
    [Authorize(Roles = nameof(Roles.HumanRecourses) + "," + nameof(Roles.CompanyOwner))]
    [HttpGet("Employees")]
    public async Task<ActionResult<ServerResponse<List<GetUserDto>>>> GetEmployees()
    {
        var response = await _managementService.GetEmployees();
        if (response.Success)
            return Ok(response);
        return BadRequest(response);
    }
}