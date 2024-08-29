namespace Microservice.Order.History.Function.Data.Repository.Interfaces;

public interface IOrderHistoryRepository
{
    Task<Domain.OrderHistory> AddAsync(History.Function.Domain.OrderHistory orderHistory);
    Task<bool> ExistsAsync(Guid id);
}