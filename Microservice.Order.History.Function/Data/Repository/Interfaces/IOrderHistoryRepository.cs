namespace Microservice.Order.History.Function.Data.Repository.Interfaces;

public interface IOrderHistoryRepository
{
    Task<Domain.OrderHistory> AddAsync(History.Function.Domain.OrderHistory orderHistory);
    Task UpdateAsync(Domain.OrderHistory entity);
    Task Delete(Domain.OrderHistory orderHistory);
    Task<Domain.OrderHistory> GetByIdAsync(Guid id, Guid customerId);
    Task<Domain.OrderHistory> OrderSummaryReadOnlyAsync(Guid id);
    Task<List<Domain.OrderHistory>> SearchByDateAsync(DateOnly date);
    Task<bool> ExistsAsync(Guid id);
}