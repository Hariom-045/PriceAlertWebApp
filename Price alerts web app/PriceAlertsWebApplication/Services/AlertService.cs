using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;
using PriceAlertsWebApplication.Persistance.InMemoryStore;

namespace PriceAlertsWebApplication.Services;

public class AlertService : IAlertService
{
    public ServiceResult CreateAlerts(
        List<CreateAlertRequest> requests)
    {
        if (requests == null || !requests.Any())
        {
            return new ServiceResult
            {
                Success = false,
                Message = "At least one alert is required."
            };
        }

        foreach (var request in requests)
        {
            if (request.TargetPrice <= 0)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Invalid target price: {request.TargetPrice}"
                };
            }

            AlertStore.Alerts.Add(
                new PriceAlertModel
                {
                    TargetPrice = request.TargetPrice,
                    Direction = request.Direction
                });
        }

        return new ServiceResult
        {
            Success = true,
            Message = $"{requests.Count} alerts created."
        };
    }
}