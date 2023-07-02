using System.Diagnostics;
using lets_leave.Dto.LeaveDto;
using lets_leave.Models;
using lets_leave.Services.LeaveService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lets_leave.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LeaveRequestController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeaveRequestController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [HttpGet]
    public async Task<ActionResult<ServerResponse<List<GetLeaveDto>>>> GetAll()
    {
        var response = await _leaveService.GetAll();
        if (!response.Success)
            return NotFound(response);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServerResponse<GetLeaveDto>>> Create(PostLeaveDto leaveDto)
    {
        var response = await _leaveService.Create(leaveDto);
        if (!response.Success)
            return BadRequest(response);
        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<ServerResponse<GetLeaveDto>>> Update([FromQuery] string id, PostLeaveDto leaveDto)
    {
        var response = await _leaveService.Update(id, leaveDto);
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("not found") => NotFound(response),
            _ => BadRequest(response)
        };
    }
    [HttpDelete]
    public async Task<ActionResult<ServerResponse<GetLeaveDto>>> Delete([FromQuery] string id)
    {
        var response = await _leaveService.Delete(id);
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("not found") => NotFound(response),
            _ => BadRequest(response)
        };
    }
}