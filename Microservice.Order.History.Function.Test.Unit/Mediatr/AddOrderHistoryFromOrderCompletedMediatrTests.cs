using MediatR;
using Microservice.Order.History.Function.Data.Repository.Interfaces;
using Microservice.Order.History.Function.Domain;
using Microservice.Order.History.Function.Helpers;
using Microservice.Order.History.Function.Helpers.Interfaces;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Order.History.Function.Test.Unit.Mediatr;

[TestFixture]
public class AddOrderHistoryFromOrderCompletedMediatrTests
{
    private readonly Mock<IOrderHistoryRepository> orderHistoryRepositoryMock = new();
    private readonly Mock<IAzureServiceBusHelper> azureServiceBusHelperMock = new();
    private readonly Mock<ILogger<AddOrderHistoryCommandHandler>> loggerMock = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddOrderHistoryCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<IOrderHistoryRepository>(sp => orderHistoryRepositoryMock.Object);
        services.AddScoped<IAzureServiceBusHelper>(sp => azureServiceBusHelperMock.Object);
        services.AddScoped<ILogger<AddOrderHistoryCommandHandler>>(sp => loggerMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddOrderHistoryMapper)));

        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [Test]
    public async Task Order_history_added_return_success()
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

        azureServiceBusHelperMock.Setup(x => x.SendMessage(EnvironmentVariables.AzureServiceBusQueueOrderHistoryAdded, It.IsNotNull<string>()));

        orderHistoryRepositoryMock
                .Setup(x => x.AddAsync(Moq.It.IsAny<OrderHistory>()));

        orderHistoryRepositoryMock
                .Setup(x => x.ExistsAsync(orderId))
                .Returns(Task.FromResult(false));

        var actualResult = await mediator.Send(addOrderHistoryRequest);
    }

    [Test]
    public async Task Order_history_already_exists_return_true()
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
                                             9.99m, DateOnly.FromDateTime(DateTime.Now), orderItemList,
                                                    addOrderHistoryAddressRequest);

        orderHistoryRepositoryMock
                .Setup(x => x.ExistsAsync(orderId))
                .Returns(Task.FromResult(true));

        var actualResult = await mediator.Send(addOrderHistoryRequest);
    }
}