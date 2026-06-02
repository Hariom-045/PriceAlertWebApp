using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace PriceAlertsWebApplication.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HomeController : Controller
{
    
}