using PriceAlertsWebApplication.Helper;
using PriceAlertsWebApplication.Models;
using PriceAlertsWebApplication.Models.ResponseModels;
using PriceAlertsWebApplication.Persistance;
using PriceAlertsWebApplication.Persistance.InMemoryStore;

namespace PriceAlertsWebApplication.workers;

using Microsoft.Extensions.Hosting;

public class GoldPriceWorker : BackgroundService
{
    private readonly ItwelveDataService _twelveDataService;
    private readonly ITelegramNotificationService _telegramService;
    private readonly IConfiguration _configuration;
    private DateTime? _lastErrorNotification;
    
    public GoldPriceWorker(ItwelveDataService twelveDataService,
        IConfiguration configuration,
        ITelegramNotificationService telegramService
        )
    {
        _twelveDataService = twelveDataService;
        _telegramService = telegramService;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DoWork(stoppingToken);
            }
            catch (Exception ex)
            {
                await NotifyError(ex, "Worker Error");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken);
        }
    }

    private async Task DoWork(
        CancellationToken cancellationToken)
    {
        // Skip outside trading hours
        var now = TimeHelper.IstNow;
        var startHour =
            _configuration.GetValue<int>("TradingHours:StartHour");

        var endHour =
            _configuration.GetValue<int>("TradingHours:EndHour");

        var isOutsideTradingHours =
            now.Hour < startHour ||
            now.Hour >= endHour;

        if (isOutsideTradingHours)
            return;
        
        var activeAlerts = AlertStore.Alerts
            .Where(x => !x.Triggered)
            .ToList();
        
        //skip if there is no alert
        if (!activeAlerts.Any())
            return;
        GoldPriceResponse? currentPriceList;
        try
        {
            currentPriceList =
                await _twelveDataService.GetLatestGoldPrice();
        }
        catch (Exception ex)
        {
            await NotifyError(ex, "TwelveData API Failed");
            return;
        }

        AppState.LastPriceCheck = TimeHelper.IstNow;

        var candle = currentPriceList.goldPriceValues.First();

        var high = Convert.ToDouble(candle.High);
        var low  = Convert.ToDouble(candle.Low);

        foreach (var alert in activeAlerts)
        {
            bool triggered =
                alert.Direction == AlertDirection.Above
                    ? high >= alert.TargetPrice
                    : low <= alert.TargetPrice;
            if (!triggered)
                continue;

            alert.Triggered = true;

            await _telegramService.SendTelegramNotification(
                $"""
                 🚨 GOLD ALERT

                 Target Price:
                 {alert.TargetPrice}

                 Direction:
                 {alert.Direction}

                 Current High:
                 {high}

                 Current Low:
                 {low}

                 Triggered At:
                 {TimeHelper.IstNow:dd-MMM-yyyy HH:mm:ss}
                 """
            );
        }
    }
    private async Task NotifyError(Exception ex, string source)
    {
        if (_lastErrorNotification.HasValue &&
            TimeHelper.IstNow - _lastErrorNotification.Value < TimeSpan.FromMinutes(15))
            return;

        _lastErrorNotification = TimeHelper.IstNow;

        await _telegramService.SendTelegramNotification(
            $"""
             ❌ GOLD ALERT BOT ERROR

             Source: {source}

             Time: {TimeHelper.IstNow:dd-MMM-yyyy HH:mm:ss}

             Message: {ex.Message}
             """
        );
    }
}