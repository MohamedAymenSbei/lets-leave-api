using System.Text.Json.Serialization;

namespace lets_leave.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Sector
{
    Insurance,
    Technology,
    Finance,
    Retail,
    Education,
    Energy,
    Hospitality,
    Manufacturing,
    Transportation,
    Communications
}