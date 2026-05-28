using System;
using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication.Services;

public class GoldService : IGoldService
{
    public IGoldHttpService _goldHttpService;
    public GoldService(IGoldHttpService goldHttpService)
    {
        _goldHttpService = goldHttpService;
    }
    public Task<GoldPriceResponseModel.GoldPriceResponse> CreateGoldPriceAlert(GoldPriceRequestModel goldPriceRequestModel)
    {
        var response =  _goldHttpService.GetLatestGoldPrice();
        if (response == null || response.Result == null)
        {
            
        }
        else
        {
            var prices = response.Result.Values;
            foreach (var price in prices)
            {
                if ((goldPriceRequestModel.lower_price >= Convert.ToDouble(price.Low) &&
                    goldPriceRequestModel.lower_price <= Convert.ToDouble(price.High))
                    || (goldPriceRequestModel.upper_price >= Convert.ToDouble(price.Low)) && 
                    goldPriceRequestModel.upper_price <= Convert.ToDouble(price.High))
                {
                    // write telegram implementation here.
                    Console.WriteLine("Telegram implementation incoming...");
                    
                }
            }
        }

        return response;
    }
}