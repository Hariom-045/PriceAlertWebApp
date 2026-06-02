namespace PriceAlertsWebApplication.Helper;

public static class TimeHelper
{
    private static readonly TimeZoneInfo IstTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows()
                ? "India Standard Time"
                : "Asia/Kolkata");

    public static DateTime IstNow =>
        TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            IstTimeZone);
}