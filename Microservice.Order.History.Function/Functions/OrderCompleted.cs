using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Order.History.Function.Helpers;
using Microservice.Order.History.Function.Helpers.Exceptions;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microservice.Order.History.Function.Functions;

public class OrderCompleted(ILogger<OrderCompleted> logger, IMediator mediator)
{
    [Function(nameof(OrderCompleted))]
    public async Task Run([ServiceBusTrigger("%" + Constants.AzureServiceBusQueueOrderCompleted + "%",
                                             Connection = Constants.AzureServiceBusConnectionManagedIdentity)]
                                             ServiceBusReceivedMessage message,
                                             ServiceBusMessageActions messageActions)
    {
        AddOrderHistoryRequest? addOrderHistoryRequest = JsonHelper.GetRequest<AddOrderHistoryRequest>(message.Body.ToArray()) ?? throw new JsonDeserializeException("Error deserializing AddCustomerAddressRequest.");

        try
        {
            logger.LogInformation("Order Completed - Add Order History - {addOrderHistoryRequest.Id}", addOrderHistoryRequest.Id);

            await mediator.Send(addOrderHistoryRequest);
            await messageActions.CompleteMessageAsync(message);

            return;
        }
        catch (FluentValidation.ValidationException validationException)
        {
            logger.LogError("Validation Failures: Id: {addOrderHistoryRequest.Id}", addOrderHistoryRequest.Id);
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonValidation, ErrorHelper.GetErrorMessagesAsString(validationException.Errors));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Internal Error: Id: {addOrderHistoryRequest.Id} - {ex.Message}", addOrderHistoryRequest.Id, ex.Message);
            await messageActions.DeadLetterMessageAsync(message, null, ex.Message, ex.StackTrace);
        }
    }
}