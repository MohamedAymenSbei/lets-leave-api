using lets_leave.Dto.AuthDto;
using lets_leave.Dto.CompanyDto;
using lets_leave.Dto.DepartmentDto;
using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Models;

namespace lets_leave.Data;

using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<PostUserDto, User>();
        CreateMap<User, GetUserDto>();

        CreateMap<PostCompanyDto, Company>();
        CreateMap<Company, GetCompanyDto>();

        CreateMap<PostDepartmentDto, Department>();
        CreateMap<Department, GetDepartmentDto>();

        CreateMap<PostLeaveDto, LeaveRequest>();
        CreateMap<LeaveRequest, GetLeaveDto>();
    }
}