namespace lets_leave.Dto.UserDto;
using System.ComponentModel.DataAnnotations;

public class PostUserDto
{
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8,ErrorMessage = "Password must contain at least 8 characters")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = string.Empty;
}