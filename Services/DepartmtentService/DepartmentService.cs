using AutoMapper;
using lets_leave.Data;
using lets_leave.Dto.DepartmentDto;
using lets_leave.Models;
using lets_leave.Services.CompanyService;
using lets_leave.Services.DeparmtentService;

namespace lets_leave.Services.DepartmentService;

public class DepartmentService : IDepartmentService
{
    private readonly AppDbContext _appDbContext;
    private readonly ICompanyService _companyService;
    private readonly IMapper _mapper;

    public DepartmentService(
        AppDbContext appDbContext,
        ICompanyService companyService,
        IMapper mapper
    )
    {
        _appDbContext = appDbContext;
        _companyService = companyService;
        _mapper = mapper;
    }

    public async  Task<ServerResponse<List<GetDepartmentDto>>> GetAll()
    {
        var response = new ServerResponse<List<GetDepartmentDto>>();

        var companyResponse = await _companyService.GetCompany<Company>();
        var company = companyResponse.Data;

        if (company == null)
        {
            response.Message = "Company not found";
            response.Success = false;
            return response;
        }

        response.Data = company.Departments.Select(d => _mapper.Map<GetDepartmentDto>(d)).ToList();
        return response;
    }

    public async Task<ServerResponse<GetDepartmentDto>> Create(PostDepartmentDto departmentDto)
    {
        var response = new ServerResponse<GetDepartmentDto>();

        var companyResponse = await _companyService.GetCompany<Company>();
        var company = companyResponse.Data;

        if (company == null)
        {
            response.Message = "Company not found";
            response.Success = false;
            return response;
        }

        var departmentExists = company.Departments.FirstOrDefault(d =>
            string.Equals(d.Name, departmentDto.Name, StringComparison.CurrentCultureIgnoreCase)) != null;

        if (departmentExists)
        {
            response.Success = false;
            response.Message = "Department already exists";
            return response;
        }

        var department = _mapper.Map<Department>(departmentDto);
        company.Departments.Add(department);
        await _appDbContext.SaveChangesAsync();
        response.Data = _mapper.Map<GetDepartmentDto>(department);

        return response;
    }

    public async Task<ServerResponse<GetDepartmentDto>> Update(string depId, PostDepartmentDto departmentDto)
    {
        var response = new ServerResponse<GetDepartmentDto>();

        var companyResponse = await _companyService.GetCompany<Company>();
        var company = companyResponse.Data;

        if (company == null)
        {
            response.Message = "Company not found";
            response.Success = false;
            return response;
        }

        var department = company.Departments.FirstOrDefault(d => d.Id.ToString() == depId);
        if (department == null)
        {
            response.Message = "Department not found";
            response.Success = false;
            return response;
        }
        var departmentExists = company.Departments.FirstOrDefault(d =>
            string.Equals(d.Name, departmentDto.Name, StringComparison.CurrentCultureIgnoreCase)) != null;

        if (departmentExists)
        {
            response.Success = false;
            response.Message = "Department with that name already exists";
            return response;
        }

        department.UpdatedAt = DateTime.Now;
        _mapper.Map(departmentDto, department);
        await _appDbContext.SaveChangesAsync();
        response.Data = _mapper.Map<GetDepartmentDto>(department);
        return response;
    }

    public async Task<ServerResponse<string>> Delete(string id)
    {
        var response = new ServerResponse<string>();

        var companyResponse = await _companyService.GetCompany<Company>();
        var company = companyResponse.Data;

        if (company == null)
        {
            response.Message = "Company not found";
            response.Success = false;
            return response;
        }

        var department = company.Departments.FirstOrDefault(d => d.Id.ToString() == id);
        if (department == null)
        {
            response.Message = "Department not found";
            response.Success = false;
            return response;
        }

        _appDbContext.Departments.Remove(department);
        await _appDbContext.SaveChangesAsync();
        response.Data = department.Id.ToString();
        return response;
    }
}