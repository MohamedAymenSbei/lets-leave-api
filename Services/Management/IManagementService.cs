using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Enums;
using lets_leave.Models;

namespace lets_leave.Services.Management;

public interface IManagementService
{
    Task<ServerResponse<GetUserDto>> AddEmployee(AddEmployeeDto addEmployeeDto, string? depId);
    Task<ServerResponse<GetUserDto>> UpdateRole(string employeeId, bool isHr);
    Task<ServerResponse<List<UserLeaveRequestDto>>> GetLeaveRequests();
    Task<ServerResponse<GetLeaveDto>> UpdateLeaveRequestStatus(string requestId, LeaveStatus status);

    Task<ServerResponse<List<GetUserDto>>> GetEmployees();
}