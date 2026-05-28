using System;
using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication.Services;

public class GoldService : IGoldService
{
    public IGoldHttpService _goldHttpService;
    public GoldService(IGoldHttpService goldHttpService)
    {
        _goldHttpService = goldHttpService;
    }
    public void CreateGoldPriceAlert(GoldPriceRequestModel goldPriceRequestModel)
    {
        var response =  _goldHttpService.GetLatestGoldPrice();
        if (response == null || response.Result == null)
        {
            
        }
        else
        {
            foreach (var prices in response.Result.Values)
            {
                if (goldPriceRequestModel.lower_price >= Convert.ToDouble(prices.Low) &&
                    goldPriceRequestModel.upper_price <= Convert.ToDouble(prices.High))
                {
                    // write telegram implementation here.
                }
            }
        }
    }
}