using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication.Services;

public class GoldService : IGoldService
{
    private IGoldHttpService _goldHttpService;
    private ITelegramNotificationService _telegramNotificationService;
    public GoldService(IGoldHttpService goldHttpService, ITelegramNotificationService telegramNotificationService)
    {
        _goldHttpService = goldHttpService;
        _telegramNotificationService = telegramNotificationService;
    }
    public async Task<GoldPriceResponseModel.GoldPriceResponse> CreateGoldPriceAlert(GoldPriceRequestModel goldPriceRequestModel)
    {
        var response =  _goldHttpService.GetLatestGoldPrice();
        string msg = "";
        if (response == null || response.Result == null)
        {
            msg = $"Twelve Price API Failed. Please check Lower_alert_price - {goldPriceRequestModel.lower_price}, " +
                  $"Upper_alert_Price - {goldPriceRequestModel.upper_price} manually";
            _telegramNotificationService.SendTelegramNotification(msg);
        }
        else
        {
            var prices =  response.Result.Values;
            foreach (var price in prices)
            {
                if (goldPriceRequestModel.lower_price >= Convert.ToDouble(price.Low) &&
                    goldPriceRequestModel.lower_price <= Convert.ToDouble(price.High))
                {
                    msg = $"🚨ALERT🚨!!!! Gold Price crossed Lower_price - {goldPriceRequestModel.lower_price} ";
                    await SendTelegramMessageToUser(msg);
                }
                if (goldPriceRequestModel.upper_price >= Convert.ToDouble(price.Low) &&
                    goldPriceRequestModel.upper_price <= Convert.ToDouble(price.High))
                {
                    msg = $"🚨ALERT🚨!!!! Gold Price crossed upper_price - {goldPriceRequestModel.upper_price} ";
                    await SendTelegramMessageToUser(msg);
                }
            }
        }

        return await response;
    }

    private async Task SendTelegramMessageToUser(string msg)
    {
        await _telegramNotificationService.SendTelegramNotification(msg);
    }
}