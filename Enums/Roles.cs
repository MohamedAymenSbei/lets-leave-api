using System.Text.Json.Serialization;

namespace lets_leave.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Roles
{
    CompanyOwner,
    HumanRecourses,
    Employee
}