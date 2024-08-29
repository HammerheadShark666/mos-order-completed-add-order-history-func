using Microservice.Order.History.Function.Data.Context;
using Microservice.Order.History.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.History.Function.Data.Repository;

public class OrderHistoryRepository(IDbContextFactory<OrderHistoryDbContext> dbContextFactory) : IOrderHistoryRepository
{
    public async Task<Domain.OrderHistory> AddAsync(Domain.OrderHistory orderHistory)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.AddAsync(orderHistory);
        db.SaveChanges();

        return orderHistory;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.OrdersHistory.AnyAsync(x => x.Id.Equals(id));
    }
}