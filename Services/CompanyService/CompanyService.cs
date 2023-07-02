using System.Security.Claims;
using AutoMapper;
using lets_leave.Data;
using lets_leave.Dto.CompanyDto;
using lets_leave.Models;
using lets_leave.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace lets_leave.Services.CompanyService;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public CompanyService(AppDbContext dbContext, IMapper mapper, ITokenService tokenService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    // Instead of creating a company with db instance , we can just map it with our user
    public async Task<ServerResponse<Company>> CreateCompany(PostCompanyDto companyDto)
    {
        var response = new ServerResponse<Company>();
        if (await IsCompanyExists(companyDto.Email, companyDto.Name))
        {
            response.Success = false;
            response.Message = "Company already exists";
        }
        else
        {
            var companyDb = await _dbContext.Companies
                .AddAsync(_mapper.Map<Company>(companyDto));
            response.Success = true;
            response.Data = companyDb.Entity;
        }

        return response;
    }

    public async Task<bool> IsCompanyExists(string email, string name)
    {
        var existingCompany = await _dbContext.Companies.FirstOrDefaultAsync(c =>
            string.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase) ||
            string.Equals(c.Email, email, StringComparison.CurrentCultureIgnoreCase)
        );

        return existingCompany != null;
    }

    public async Task<ServerResponse<T>> GetCompany<T>()
    {
        var response = new ServerResponse<T>();

        var userId = _tokenService.GetUserId();

        if (userId == null)
        {
            response.Success = false;
            response.Message = "Invalid user";
            ;
            return response;
        }

        var company = await _dbContext.Companies
            .Include(c => c.Departments)
            .FirstOrDefaultAsync(c => c.Users.Any(u => u.Id == userId));
        if (company == null)
        {
            response.Success = false;
            response.Message = "Company not found";
            return response;
        }

        if (typeof(T) == typeof(Company))
        {
            response.Data = (T)(object)company;
        }

        else if (typeof(T) == typeof(GetCompanyDto))
        {
            response.Data = (T)(object)_mapper.Map<GetCompanyDto>(company);
        }
        else
        {
            response.Success = false;
            response.Message = "Invalid response type";
            return response;
        }

        return response;
    }

    public async Task<ServerResponse<string>> Delete()
    {
        var response = new ServerResponse<string>();

        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var company = await _dbContext.Companies
            .FirstOrDefaultAsync(c => c.Users.Any(u => u.Id == userId));
        if (company == null)
        {
            response.Success = false;
            response.Message = "Company not found";
            return response;
        }

        _dbContext.Companies.Remove(company);
        await _dbContext.SaveChangesAsync();
        response.Data = company.Id.ToString();
        response.Message = "Company has been removed";
        return response;
    }
}