namespace PriceAlertsWebApplication.Models.ResponseModels;

public class ServiceResult
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}