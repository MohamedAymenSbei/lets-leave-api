using lets_leave.Dto.LeaveDto;
using lets_leave.Dto.UserDto;
using lets_leave.Models;

namespace lets_leave.Services.Management;

public class ManagementService : IManagementService
{
    public Task<ServerResponse<GetUserDto>> AddEmployee()
    {
        throw new NotImplementedException();
    }

    public Task<ServerResponse<GetUserDto>> UpdateRole()
    {
        throw new NotImplementedException();
    }

    public Task<ServerResponse<List<GetLeaveDto>>> GetLeaveRequests()
    {
        throw new NotImplementedException();
    }

    public Task<ServerResponse<GetLeaveDto>> UpdateLeaveRequestStatus()
    {
        throw new NotImplementedException();
    }
}