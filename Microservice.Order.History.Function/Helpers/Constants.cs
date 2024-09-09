namespace Microservice.Order.History.Function.Helpers;
public class Constants
{
    public const string AzureServiceBusConnectionManagedIdentity = "ServiceBusConnection";
    public const string AzureServiceBusConnection = "AZURE_SERVICE_BUS_CONNECTION";
    public const string AzureServiceBusQueueOrderCompleted = "AZURE_SERVICE_BUS_QUEUE_ORDER_COMPLETED";
    public const string AzureServiceBusQueueOrderHistoryAdded = "AZURE_SERVICE_BUS_QUEUE_ORDER_HISTORY_ADDED";

    public const string FailureReasonValidation = "Validation Errors.";
    public const string FailureReasonInternal = "Internal Error.";

    public const string DatabaseConnectionString = "SQLAZURECONNSTR_ORDER_HISTORY";
}