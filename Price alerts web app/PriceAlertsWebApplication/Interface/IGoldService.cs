using PriceAlertsWebApplication.Models;

namespace PriceAlertsWebApplication;

public interface IGoldService
{
    public void CreateGoldPriceAlert(GoldPriceRequestModel goldPriceRequestModel);
}