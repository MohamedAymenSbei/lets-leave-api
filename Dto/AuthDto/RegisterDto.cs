using System.ComponentModel.DataAnnotations;
using lets_leave.Dto.CompanyDto;
using lets_leave.Dto.UserDto;

namespace lets_leave.Dto.AuthDto;

public class RegisterDto
{
    [Required(ErrorMessage = "User information are required")]
    public PostUserDto UserInformation { get; set; }
    [Required(ErrorMessage = "Company information are required")]
    public PostCompanyDto CompanyInformation { get; set; }
}