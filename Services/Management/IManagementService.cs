using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Models;

namespace lets_leave.Services.Management;

public interface IManagementService
{
    Task<ServerResponse<GetUserDto>> AddEmployee();
    Task<ServerResponse<GetUserDto>> UpdateRole();
    Task<ServerResponse<List<GetLeaveDto>>> GetLeaveRequests();
    Task<ServerResponse<GetLeaveDto>> UpdateLeaveRequestStatus();
}