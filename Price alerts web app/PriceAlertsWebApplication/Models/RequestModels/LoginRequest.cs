namespace PriceAlertsWebApplication.Models;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string AccessKey { get; set; } =  string.Empty;
}