using lets_leave.Enums;

namespace lets_leave.Dto.CompanyDto;

public class GetCompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Email { get; set; } 
    public Sector Sector { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}