using PriceAlertsWebApplication.Helper;

namespace PriceAlertsWebApplication.Models;

public class PriceAlertModel
{
    
    public Guid Id { get; set; } = Guid.NewGuid();

    public double TargetPrice { get; set; }

    public AlertDirection Direction { get; set; }

    public bool Triggered { get; set; }

    public DateTime CreatedAt { get; set; } = TimeHelper.IstNow;
}