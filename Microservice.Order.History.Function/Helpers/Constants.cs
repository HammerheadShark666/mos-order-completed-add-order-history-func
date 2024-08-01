namespace Microservice.Order.History.Function.Helpers;
public class Constants
{
    public const string AzureServiceBusConnection = "AZURE_SERVICE_BUS_CONNECTION";
    public const string AzureServiceBusQueueOrderCompleted = "order-completed";
    public const string AzureServiceBusQueueOrderHistoryAdded = "order-history-added";

    public const string FailureReasonValidation = "Validation Errors.";
    public const string FailureReasonInternal = "Internal Error.";

    public const string DatabaseConnectionString = "SQLAZURECONNSTR_ORDER_HISTORY";
}