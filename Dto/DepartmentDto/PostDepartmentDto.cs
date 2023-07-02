namespace lets_leave.Dto.DepartmentDto;

using System.ComponentModel.DataAnnotations;

public class PostDepartmentDto
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name should at least contain 2 characters")]
    public string Name { get; set; } = string.Empty;
}