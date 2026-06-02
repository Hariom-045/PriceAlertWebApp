using System.Text.Json;
using Microsoft.Extensions.Options;
using PriceAlertsWebApplication.Helper;
using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication.Services;

public class TwelveDataService : ItwelveDataService
{
    private readonly TwelveAPISettings _apiSettings;
    private readonly ITelegramNotificationService _telegramNotificationService;
    public TwelveDataService(IOptions<TwelveAPISettings> apiSettings,
        ITelegramNotificationService telegramNotificationService)
    {
        _apiSettings = apiSettings.Value;
        _telegramNotificationService = telegramNotificationService;
    } 
    public async Task<GoldPriceResponse>  GetLatestGoldPrice()
    {
        string url =
            _apiSettings.baseURL +
            "?apikey=" + _apiSettings.apiKey +
            "&interval=" + _apiSettings.interval +
            "&format=" + _apiSettings.format +
            "&symbol=" + _apiSettings.symbol +
            "&outputsize=" + _apiSettings.outPutSize;

        using var client = new HttpClient();

        using var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();

        var goldResponse =
            JsonSerializer.Deserialize<GoldPriceResponse>(body);

        if (goldResponse == null)
            throw new Exception("Failed to deserialize TwelveData response");

        return goldResponse;
    }
}