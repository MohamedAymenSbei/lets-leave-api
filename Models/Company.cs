using lets_leave.Enums;
using Microsoft.EntityFrameworkCore;

namespace lets_leave.Models;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Sector Sector { get; set; } = Sector.Education;
    public List<User> Users { get; set; } = new();
    public List<Department> Departments { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Company>()
            .HasIndex(c => c.Name)
            .IsUnique();
        builder.Entity<Company>()
            .HasIndex(c => c.Email)
            .IsUnique();
        
    }
}