using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication.Controllers;

[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IJwtService _jwtService;

    public AuthController(
        IOptions<JwtSettings> jwtSettings,
        IJwtService jwtService)
    {
        _jwtSettings = jwtSettings.Value;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("Email is required");
        }

        var allowedUsers = _jwtSettings.AllowedUsers;

        if (allowedUsers == null || !allowedUsers.Any())
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "No allowed users configured");
        }
        bool isAllowed = allowedUsers.Any(x =>
            x.Equals(
                request.Email,
                StringComparison.OrdinalIgnoreCase));

        if (!isAllowed)
        {
            return Unauthorized("User not allowed");
        }
        var configuredPublicKey = _jwtSettings.PublicKey;

        if (request.AccessKey != configuredPublicKey)
        {
            return Unauthorized("Invalid access key");
        }
        var token = _jwtService.GenerateToken(request.Email);
        
        var cookieSecure = _jwtSettings.CookieSecure;
        Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = cookieSecure,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = cookieSecure,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        
        return Ok(new
        {
            Message = "Login successful",
            Token = token,
            Email = request.Email
        });
    }
}