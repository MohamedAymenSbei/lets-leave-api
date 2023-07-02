using lets_leave.Dto.LeaveDto;
using lets_leave.Models;

namespace lets_leave.Services.LeaveService;

public interface ILeaveService
{
    Task<ServerResponse<List<GetLeaveDto>>> GetAll();
    Task<ServerResponse<GetLeaveDto>> Create(PostLeaveDto postLeaveDto);
    Task<ServerResponse<GetLeaveDto>> Update(string id, PostLeaveDto postLeaveDto);
    Task<ServerResponse<string>> Delete(string id);
}