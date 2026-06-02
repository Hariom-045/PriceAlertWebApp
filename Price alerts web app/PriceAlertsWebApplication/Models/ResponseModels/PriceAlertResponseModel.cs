using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PriceAlertsWebApplication.Models.ResponseModels;

public class PriceAlertResponseModel
{
    public GoldPriceResponse gold_price_response { get; set; }
}

public class GoldPriceResponse
{
    [JsonPropertyName("meta")]
    public Meta Meta { get; set; }

    [JsonPropertyName("values")]
    public List<GoldPriceValues> goldPriceValues { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public class Meta
{
    [JsonPropertyName("symbol")]
    public string Symbol { get; set; }

    [JsonPropertyName("interval")]
    public string Interval { get; set; }

    [JsonPropertyName("currency_base")]
    public string CurrencyBase { get; set; }

    [JsonPropertyName("currency_quote")]
    public string CurrencyQuote { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class GoldPriceValues
{
    [JsonPropertyName("datetime")]
    public string Datetime { get; set; }

    [JsonPropertyName("open")]
    public string Open { get; set; }

    [JsonPropertyName("high")]
    public string High { get; set; }

    [JsonPropertyName("low")]
    public string Low { get; set; }

    [JsonPropertyName("close")]
    public string Close { get; set; }
}