using System.ComponentModel.DataAnnotations;

namespace lets_leave.Dto.AuthDto;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8,ErrorMessage = "Password must contain at least 8 characters")]
    public string Password { get; set; } = string.Empty;
}