namespace Microservice.Order.History.Function.Helpers.Interfaces;

public interface IAzureServiceBusHelper
{
    Task SendMessage(string queue, string data);
}
