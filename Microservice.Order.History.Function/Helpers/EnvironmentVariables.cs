using Microservice.Order.History.Function.Helpers.Exceptions;

namespace Microservice.Order.History.Function.Helpers;

public class EnvironmentVariables
{
    public static string AzureServiceBusQueueOrderHistoryAdded => GetEnvironmentVariable(Constants.AzureServiceBusQueueOrderHistoryAdded);
    public static string AzureServiceBusConnectionString => GetEnvironmentVariable(Constants.AzureServiceBusConnectionString);

    public static string GetEnvironmentVariable(string name)
    {
        var variable = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrEmpty(variable))
            throw new EnvironmentVariableNotFoundException($"Environment Variable Not Found: {name}.");

        return variable;
    }
}