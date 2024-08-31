using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Order.History.Function.Functions;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Microservice.Order.History.Function.Test.Unit;

public class AddOrderHistoryFromOrderCompletedAzureFunctionTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<OrderCompleted>> _mockLogger;
    private readonly OrderCompleted _orderCompleted;

    public AddOrderHistoryFromOrderCompletedAzureFunctionTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<OrderCompleted>>();
        _orderCompleted = new OrderCompleted(_mockLogger.Object, _mockMediator.Object);
    }

    [Test]
    public async Task Azure_function_trigger_service_bus_receive_return_succeed()
    {
        Guid customerId = new("6c84d0a3-0c0c-435f-9ae0-4de09247ee15");
        Guid orderId = new("724cbd34-3dff-4e2a-a413-48825f1ab3b9");
        Guid bookId = new("29a75938-ce2d-473b-b7fe-2903fe97fd6e");

        var addOrderHistoryAddressRequest
                = new AddOrderHistoryAddressRequest("AddressLine1", "AddressLine2", "AddressLine2",
                                                      "TownCity", "County", "Postcode", "Country");

        var addOrderHistoryOrderItemRequest = new AddOrderHistoryOrderItemRequest(bookId, "Infinity Kings", "Book", 9.99m, 1);

        List<AddOrderHistoryOrderItemRequest> orderItemList = [addOrderHistoryOrderItemRequest];

        var addOrderHistoryRequest = new AddOrderHistoryRequest(orderId, customerId, "AddressSurname",
                                        "AddressForename", "OrderNumber", "OrderStatus",
                                             9.99m, DateOnly.FromDateTime(DateTime.Now),
                                                orderItemList, addOrderHistoryAddressRequest);

        var mockMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromString(JsonConvert.SerializeObject(addOrderHistoryRequest)), correlationId: Guid.NewGuid().ToString());

        var mockServiceBusMessageActions = new Mock<ServiceBusMessageActions>();
        mockServiceBusMessageActions.Setup(x => x.CompleteMessageAsync(mockMessage, CancellationToken.None)).Returns(Task.FromResult(true));

        await _orderCompleted.Run(mockMessage, mockServiceBusMessageActions.Object);
    }
}