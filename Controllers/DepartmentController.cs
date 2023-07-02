using lets_leave.Dto.DepartmentDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.DeparmtentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lets_leave.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(Roles.HumanRecourses) + "," + nameof(Roles.CompanyOwner))]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ServerResponse<List<GetDepartmentDto>>>> Get()
    {
        var response = await _departmentService.GetAll();
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("not found") => NotFound(response),
            false => BadRequest(response)
        };
    }

    [HttpPost]
    public async Task<ActionResult<ServerResponse<GetDepartmentDto>>> Create(PostDepartmentDto postDepartmentDto)
    {
        var response = await _departmentService.Create(postDepartmentDto);
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("not found") => NotFound(response),
            false when response.Message.Contains("exists") => Conflict(response),
            false => BadRequest(response)
        };
    }

    [HttpPatch]
    public async Task<ActionResult<ServerResponse<GetDepartmentDto>>> Update(
        [FromQuery] string id,
        [FromBody] PostDepartmentDto postDepartmentDto)
    {
        var response = await _departmentService.Update(id, postDepartmentDto);
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("not found") => NotFound(response),
            false when response.Message.Contains("exists") => Conflict(response),
            false => BadRequest(response)
        };
    }

    [HttpDelete]
    public async Task<ActionResult<ServerResponse<GetDepartmentDto>>> Delete([FromQuery] string id)
    {
        var response = await _departmentService.Delete(id);
        return response.Success switch
        {
            true => Ok(response),
            false when response.Message.Contains("not found") => NotFound(response),
            false => BadRequest(response)
        };
    }
}