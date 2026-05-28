using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication;

public interface IGoldService
{
    public Task<GoldPriceResponseModel.GoldPriceResponse> CreateGoldPriceAlert(GoldPriceRequestModel goldPriceRequestModel);
}