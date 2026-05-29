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
        try
         {
             string url = _apiSettings.baseURL +"?apikey=" + _apiSettings.apiKey + "&interval=" + _apiSettings.interval
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
                 var goldResponse = JsonSerializer.Deserialize<GoldPriceResponseModel.GoldPriceResponse>(body);
                 return goldResponse;
             }
         }
         catch (Exception ex)
         {
             Console.WriteLine(ex);
             throw;
         }
    }
}