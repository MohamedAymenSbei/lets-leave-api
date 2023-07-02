using System.ComponentModel.DataAnnotations;
using lets_leave.Enums;

namespace lets_leave.Dto.CompanyDto;

public class PostCompanyDto
{
    [Required(ErrorMessage = "Company name is required")]
    [StringLength(30,MinimumLength = 5)]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Company email is required")]
    [EmailAddress(ErrorMessage = "Email address is not valid")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Company sector is required")]
    [EnumDataType(typeof(Sector),ErrorMessage = "Invalid sector")]
    public Sector Sector { get; set; } = Sector.Education;
    
}