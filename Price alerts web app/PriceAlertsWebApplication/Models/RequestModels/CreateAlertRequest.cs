namespace PriceAlertsWebApplication.Models;

public class CreateAlertRequest
{
    public double TargetPrice { get; set; }

    public AlertDirection Direction { get; set; }
}