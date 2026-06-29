using System.Text.Json.Serialization;

namespace DapperOnlineStoreAPI.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnumOrderStatus
    {
        Created = 1,
        Paid = 2,
        Cancelled = 3,
    }
}
