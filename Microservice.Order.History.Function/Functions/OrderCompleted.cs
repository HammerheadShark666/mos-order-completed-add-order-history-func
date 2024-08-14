using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Order.History.Function.Helpers;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microservice.Order.History.Function.Functions;

public class OrderCompleted(ILogger<OrderCompleted> logger, IMediator mediator)
{
    private ILogger<OrderCompleted> _logger { get; set; } = logger;
    private IMediator _mediator { get; set; } = mediator;
 
    [Function(nameof(OrderCompleted))]
    public async Task Run([ServiceBusTrigger("%" + Constants.AzureServiceBusQueueOrderHistoryAdded + "%",
                                             Connection = Constants.AzureServiceBusConnection)]
                                             ServiceBusReceivedMessage message,
                                             ServiceBusMessageActions messageActions)
    { 
        var addOrderHistoryRequest = JsonHelper.GetRequest<AddOrderHistoryRequest>(message.Body.ToArray());
          
        try
        {
            _logger.LogInformation(string.Format("Order Completed - Add Order History - {0}", addOrderHistoryRequest.Id.ToString()));
             
            await _mediator.Send(addOrderHistoryRequest);
            await messageActions.CompleteMessageAsync(message);

            return;
        }
        catch (FluentValidation.ValidationException validationException)
        {
            _logger.LogError(String.Format("Validation Failures: Id: {0}", addOrderHistoryRequest.Id.ToString()));
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonValidation, ErrorHelper.GetErrorMessagesAsString(validationException.Errors));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, String.Format("Internal Error: Id: {0}", addOrderHistoryRequest.Id.ToString()));
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonInternal, ex.StackTrace);
        } 
    }
}
