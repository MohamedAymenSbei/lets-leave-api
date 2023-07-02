using System.ComponentModel.DataAnnotations;
using lets_leave.Enums;

namespace lets_leave.Dto.LeaveDto;

public class PostLeaveDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Leave type is required")]
    [EnumDataType(typeof(LeaveType), ErrorMessage = "Invalid type")]
    public LeaveType Type { get; set; } = LeaveType.Family;

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; } = DateTime.Now;
}