using lets_leave.Dto.DepartmentDto;
using lets_leave.Models;

namespace lets_leave.Services.DeparmtentService;

public interface IDepartmentService
{
    Task<ServerResponse<List<GetDepartmentDto>>> GetAll();
    Task<ServerResponse<GetDepartmentDto>> Create(PostDepartmentDto departmentDto);
    Task<ServerResponse<GetDepartmentDto>> Update(string id, PostDepartmentDto departmentDto);
    Task<ServerResponse<string>> Delete(string id);
    Task<ServerResponse<Department>> AddDepartmentToUser(User user, string depId);
}