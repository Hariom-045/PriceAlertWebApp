namespace PriceAlertsWebApplication;

public interface IJwtService
{
    public string GenerateToken(string email);
}