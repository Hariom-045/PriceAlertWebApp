using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication.Persistance.InMemoryStore;

public class AlertStore
{
    public static List<PriceAlertModel> Alerts { get; } = [];
}