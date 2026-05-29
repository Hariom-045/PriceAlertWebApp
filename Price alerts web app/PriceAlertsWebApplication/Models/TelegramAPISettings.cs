namespace PriceAlertsWebApplication.Models;

public class TelegramAPISettings
{
    public string baseUrl { get; set; }
    public string botToken { get; set; }
    
    public string botChatId { get; set; }
    
    public string botName  { get; set; }
}