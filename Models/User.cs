using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace lets_leave.Models;

public class User: IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public override string UserName { get; set; } = GenerateUserName();
    public Company Company { get; set; }
    public Department? Department { get; set; }
    public List<LeaveRequest> LeaveRequests { get; set; } = new();

    private static string GenerateUserName()
    {
        var guid = Guid.NewGuid().ToByteArray();
        var base64Guid = Convert.ToBase64String(guid);
        var shortBase64 = base64Guid[..8]; // use the first 8 characters of the Base64 string
        var userName = Regex.Replace(shortBase64, @"[^a-zA-Z0-9\s]","");
        return userName.ToUpper();
    }

    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<User>()
            .Property(u => u.FirstName)
            .HasMaxLength(30);
        
        builder.Entity<User>()
            .Property(u => u.LastName)
            .HasMaxLength(30);

        builder.Entity<User>()
            .HasOne(u=>u.Company)
            .WithMany(c => c.Users)
            .OnDelete(DeleteBehavior.Cascade);
    }
}