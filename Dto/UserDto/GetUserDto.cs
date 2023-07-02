using lets_leave.Dto.DepartmentDto;

namespace lets_leave.Dto.UserDto;

public class GetUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public GetDepartmentDto Department { get; set; }
}