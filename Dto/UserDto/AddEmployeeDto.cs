using lets_leave.Enums;

namespace lets_leave.Dto.UserDto;

using System.ComponentModel.DataAnnotations;

public class AddEmployeeDto
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string SendToMail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    [EnumDataType(typeof(Roles), ErrorMessage = "Invalid role")]
    public Roles Role { get; set; } = Roles.Employee;
}