using lets_leave.Enums;
using Microsoft.EntityFrameworkCore;

namespace lets_leave.Models;

public class LeaveRequest
{
    public Guid Id { get; set; } = new();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LeaveType Type { get; set; } = LeaveType.Family;
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    public User User { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<LeaveRequest>()
            .HasOne(lr => lr.User)
            .WithMany(u => u.LeaveRequests)
            .OnDelete(DeleteBehavior.Cascade);
    }
}