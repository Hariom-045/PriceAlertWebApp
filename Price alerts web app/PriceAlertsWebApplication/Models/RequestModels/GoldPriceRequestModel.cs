namespace PriceAlertsWebApplication.Models;

public class GoldPriceRequestModel
{
    public double lower_price { get; set; }
    public double upper_price { get; set; }
    public string message { get; set; }
}