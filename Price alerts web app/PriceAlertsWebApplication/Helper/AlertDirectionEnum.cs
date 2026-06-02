using System.Text.Json.Serialization;

namespace PriceAlertsWebApplication.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AlertDirection
{
    Above = 1,
    Below = 2
}