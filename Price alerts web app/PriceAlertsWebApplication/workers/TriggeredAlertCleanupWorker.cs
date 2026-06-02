using PriceAlertsWebApplication.Helper;
using PriceAlertsWebApplication.Persistance.InMemoryStore;

namespace PriceAlertsWebApplication.workers;

public class TriggeredAlertCleanupWorker : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(
                TimeSpan.FromHours(1),
                stoppingToken);

            var cutoff = TimeHelper.IstNow.AddDays(-2);

            // 🧹 Remove: Triggered alerts and Untriggered alerts older than 2 days
            AlertStore.Alerts.RemoveAll(x =>
                x.Triggered ||
                x.CreatedAt < cutoff);
        }
    }
}