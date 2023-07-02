namespace lets_leave.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LeaveType
{
    Vacation,
    Sick,
    Maternity,
    Personal,
    Jury,
    Military,
    Family
}