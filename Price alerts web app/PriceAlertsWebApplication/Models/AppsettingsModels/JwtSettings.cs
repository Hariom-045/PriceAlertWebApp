namespace PriceAlertsWebApplication.Models;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryDays { get; set; }
    
    public List<string> AllowedUsers { get; set; } = new List<string>();
    
    public string PublicKey { get; set; } = string.Empty;
    
    public bool CookieSecure { get; set; }
}