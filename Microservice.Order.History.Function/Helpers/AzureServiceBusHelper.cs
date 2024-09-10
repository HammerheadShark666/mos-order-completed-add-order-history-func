using Azure.Messaging.ServiceBus;
using Microservice.Order.History.Function.Helpers.Interfaces;

namespace Microservice.Order.History.Function.Helpers;

public class AzureServiceBusHelper(ServiceBusClient serviceBusClient) : IAzureServiceBusHelper
{
    public async Task SendMessage(string queue, string data)
    {
        var sender = serviceBusClient.CreateSender(queue);
        await sender.SendMessageAsync(new ServiceBusMessage(data));
    }
}