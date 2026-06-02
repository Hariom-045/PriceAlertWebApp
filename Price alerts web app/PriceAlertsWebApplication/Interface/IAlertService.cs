using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;

namespace PriceAlertsWebApplication;

public interface IAlertService
{
    public ServiceResult CreateAlerts(
        List<CreateAlertRequest> requests);
}