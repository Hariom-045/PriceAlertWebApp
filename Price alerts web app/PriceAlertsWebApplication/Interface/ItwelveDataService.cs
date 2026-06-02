using System.Threading.Tasks;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication;

public interface ItwelveDataService
{
    public Task<GoldPriceResponse> GetLatestGoldPrice();
}