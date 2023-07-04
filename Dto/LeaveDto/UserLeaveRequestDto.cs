using lets_leave.Dto.UserDto;
using lets_leave.Enums;

namespace lets_leave.Dto.LeaveDto;

public class UserLeaveRequestDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public LeaveType Type { get; set; }
    public LeaveStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public GetUserDto User { get; set; }
}