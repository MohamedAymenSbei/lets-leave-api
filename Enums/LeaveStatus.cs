namespace lets_leave.Enums;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LeaveStatus
{
    Approved,
    Pending,
    Denied,
    UnderReview
}