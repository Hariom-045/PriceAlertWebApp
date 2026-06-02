using PriceAlertsWebApplication.Helper;

namespace PriceAlertsWebApplication.Persistance;

public static class AppState
{
    public static DateTime StartedAt
        = TimeHelper.IstNow;

    public static DateTime? LastPriceCheck
        = null;
}