using AutoMapper;
using MediatR;
using Microservice.Order.History.Function.Data.Repository.Interfaces;
using Microservice.Order.History.Function.Domain;
using Microservice.Order.History.Function.Helpers;
using Microservice.Order.History.Function.Helpers.Interfaces;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Microservice.Order.History.Function.MediatR.AddOrder;

public class AddOrderCommandHandler(IOrderHistoryRepository orderHistoryRepository,
                                    IAzureServiceBusHelper azureServiceBusHelper,
                                    IMapper mapper,
                                    ILogger<AddOrderCommandHandler> logger) : IRequestHandler<AddOrderHistoryRequest, AddOrderHistoryResponse>
{ 
    private IOrderHistoryRepository _orderHistoryRepository { get; set; } = orderHistoryRepository;
    private IAzureServiceBusHelper _azureServiceBusHelper { get; set; } = azureServiceBusHelper;
    private IMapper _mapper { get; set; } = mapper;
    private ILogger<AddOrderCommandHandler> _logger { get; set; } = logger;

    private record Order(Guid OrderId);

    public async Task<AddOrderHistoryResponse> Handle(AddOrderHistoryRequest addOrderHistoryRequest, CancellationToken cancellationToken)
    { 
        var orderHistory = _mapper.Map<OrderHistory>(addOrderHistoryRequest);
 
        if(!await _orderHistoryRepository.ExistsAsync(orderHistory.Id))
        { 
            UpdateOrderHistoryItems(orderHistory);

            await SaveOrderHistoryAsync(orderHistory);  
            await SendOrderHistoryAddedToServiceBusQueueAsync(orderHistory.Id);
        } 
        else
        {
            _logger.LogWarning(String.Format("OrderHistory record  already exists: {0}", orderHistory.Id.ToString()));
        }

        return new AddOrderHistoryResponse(); 
    } 

    private async Task SaveOrderHistoryAsync(Domain.OrderHistory orderHistory)
    {
        await _orderHistoryRepository.AddAsync(orderHistory);
    }

    private void UpdateOrderHistoryItems(Domain.OrderHistory orderHistory)
    {
        foreach (var orderItem in orderHistory.OrderItems)
        {
            orderItem.OrderId = orderHistory.Id;
        }
    }

    private async Task SendOrderHistoryAddedToServiceBusQueueAsync(Guid id)
    { 
        await _azureServiceBusHelper.SendMessage(EnvironmentVariables.AzureServiceBusQueueOrderHistoryAdded, GetSerializedOrder(id));
    }

    private string GetSerializedOrder(Guid orderId)
    {
        return JsonSerializer.Serialize(new Order(orderId));
    }
} 