using AutoMapper;
using lets_leave.Dto.CompanyDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.CompanyService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lets_leave.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;

    public CompanyController(ICompanyService companyService, IMapper mapper)
    {
        _companyService = companyService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ServerResponse<GetCompanyDto>>> Get()
    {
        var response = await _companyService.GetCompany<GetCompanyDto>();
        if (!response.Success)
            return NotFound(response);
        return Ok(response);
    }

    [Authorize(Roles = nameof(Roles.CompanyOwner))]
    [HttpDelete]
    public async Task<ActionResult<ServerResponse<string>>> Delete()
    {
        var response = await _companyService.Delete();
        if (!response.Success)
            return NotFound(response);
        return Ok(response);
    }
}