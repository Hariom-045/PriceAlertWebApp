using Microsoft.Extensions.Options;
using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication.Services;

public class TelegramNotificationService : ITelegramNotificationService
{
    private readonly TelegramAPISettings _telegramAPISettings;
    public TelegramNotificationService(IOptions<TelegramAPISettings> telegramAPISettings)
    {
        _telegramAPISettings = telegramAPISettings.Value;
    }
    public async Task<string> SendTelegramNotification(string message)
    {
        try
        {
            var client = new HttpClient();
            string url = $"{_telegramAPISettings.baseUrl}{_telegramAPISettings.botToken}/sendMessage";
            var data = new Dictionary<string, string> { { "chat_id", _telegramAPISettings.botChatId }, { "text", message } };
            var content = new FormUrlEncodedContent(data);
            var response = await client.PostAsync( url, content); 
            response.EnsureSuccessStatusCode();
            return "Telegram message sent successfully";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return ex.Message;
        }
        
    }
}