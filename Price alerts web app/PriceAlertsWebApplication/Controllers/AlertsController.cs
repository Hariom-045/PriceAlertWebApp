using Microsoft.AspNetCore.Authorization;
using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Persistance.InMemoryStore;

namespace PriceAlertsWebApplication.Controllers;

using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private IAlertService _alertService;

    public AlertsController(IAlertService alertService)
    {
        _alertService = alertService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(AlertStore.Alerts);
    }

    [HttpPost]
    public IActionResult Create(
        List<CreateAlertRequest> request)
    {
        _alertService.CreateAlerts(request);

        return Ok($"{request.Count} alerts created.");
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var removed =
            AlertStore.Alerts.RemoveAll(
                x => x.Id == id);

        if (removed == 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("triggered")]
    public IActionResult DeleteTriggered()
    {
        AlertStore.Alerts.RemoveAll(
            x => x.Triggered);
        return Ok("All Triggered alerts are deleted.");
    }
}