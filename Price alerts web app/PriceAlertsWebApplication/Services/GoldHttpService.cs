using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication.Services;

public class GoldHttpService : IGoldHttpService
{
    private readonly TwelveAPISettings _apiSettings;

    public GoldHttpService(IOptions<TwelveAPISettings> apiSettings)
    {
        _apiSettings = apiSettings.Value;
    } 
    public async Task<GoldPriceResponseModel.GoldPriceResponse>  GetLatestGoldPrice()
    {
       // string url =
         //   "https://api.twelvedata.com/time_series?apikey=686e4127117a4c5a89abe7d24178cb3c&interval=1min&format=JSON&symbol=XAU/USD&outputsize=1";
        string url = _apiSettings.baseURL +"?apikey" + _apiSettings.apiKey + "&interval" + _apiSettings.interval
            + "&format=" + _apiSettings.format + "&symbol=" + _apiSettings.symbol + "&outputsize="+ _apiSettings.outPutSize;
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url),
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            var goldResponse = JsonSerializer.Deserialize<GoldPriceResponseModel.GoldPriceResponse>(body);
            return goldResponse;
        }
    }
}