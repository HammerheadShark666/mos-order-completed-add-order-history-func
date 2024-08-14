namespace Microservice.Order.History.Function.Helpers;

public class EnvironmentVariables
{ 
    public static string AzureServiceBusConnection => Environment.GetEnvironmentVariable(Constants.AzureServiceBusConnection);
     
    public static string AzureServiceBusQueueOrderHistoryAdded => Environment.GetEnvironmentVariable(Constants.AzureServiceBusQueueOrderHistoryAdded);
}