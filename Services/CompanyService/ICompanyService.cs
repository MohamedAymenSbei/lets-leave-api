using System.Linq.Expressions;
using System.Security.Claims;
using lets_leave.Dto.CompanyDto;
using lets_leave.Models;

namespace lets_leave.Services.CompanyService;

public interface ICompanyService
{
    Task<ServerResponse<Company>> CreateCompany(PostCompanyDto companyDto);
    Task<bool> IsCompanyExists(string email, string name);
    Task<ServerResponse<T>> GetCompany<T>();
    Task<ServerResponse<string>> Delete();
}