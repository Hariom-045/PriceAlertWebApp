using PriceAlertsWebApplication.Persistance;
using PriceAlertsWebApplication.Persistance.InMemoryStore;

namespace PriceAlertsWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var totalAlerts =
            AlertStore.Alerts.Count;

        var triggeredAlerts =
            AlertStore.Alerts.Count(
                x => x.Triggered);

        var activeAlerts =
            totalAlerts - triggeredAlerts;
        return Ok(new
        {
            Running = true,
            StartedAt = AppState.StartedAt,
            TotalAlerts = totalAlerts,
            ActiveAlerts = activeAlerts,
            TriggeredAlerts = triggeredAlerts,
            LastPriceCheck = AppState.LastPriceCheck
        });
    }
}