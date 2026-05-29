using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication;

public interface ITelegramNotificationService
{
    public Task<string> SendTelegramNotification(string message);
}