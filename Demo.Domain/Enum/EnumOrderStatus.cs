using System.Text.Json.Serialization;

namespace Demo.Domain.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumOrderStatus
    {
        Created = 1,
        Paid = 2,
        Cancelled = 3,
    }
}
