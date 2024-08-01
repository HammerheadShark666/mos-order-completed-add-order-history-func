namespace Microservice.Order.History.Function.Helpers;

public class EnvironmentVariables
{ 
    public static string AzureServiceBusConnection => Environment.GetEnvironmentVariable(Constants.AzureServiceBusConnection);
}