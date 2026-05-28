using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IGoldService _goldService;

    public HomeController(IGoldService goldService, ILogger<HomeController> logger)
    {
        _goldService = goldService;
        _logger = logger;
    }
    [HttpPost]
    public IActionResult CreateAlert(
        GoldPriceRequestModel goldPriceRequestModel)
    {
         _goldService.CreateGoldPriceAlert(goldPriceRequestModel);
        return Ok("Alert created successfully");
    }

}