namespace lets_leave.Models;

using lets_leave.Enums;

public class Auth
{
    public string Token { get; init; } = string.Empty;
    public Roles Role { get; init; } = Roles.Employee;
}