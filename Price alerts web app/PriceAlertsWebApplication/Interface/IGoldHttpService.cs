using System.Threading.Tasks;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication;

public interface IGoldHttpService
{
    public Task<GoldPriceResponseModel.GoldPriceResponse> GetLatestGoldPrice();
}