using Microsoft.EntityFrameworkCore;

namespace lets_leave.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Company Company { get; set; }
    public List<User> Users { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<Department>()
            .HasOne(d => d.Company)
            .WithMany(c => c.Departments)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Department>()
            .HasMany<User>(d => d.Users)
            .WithOne(u => u.Department);
    }
}