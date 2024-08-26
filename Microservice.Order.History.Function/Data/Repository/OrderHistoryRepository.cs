using Microservice.Order.History.Function.Data.Context;
using Microservice.Order.History.Function.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.History.Function.Data.Repository;

public class OrderHistoryRepository(IDbContextFactory<OrderHistoryDbContext> dbContextFactory) : IOrderHistoryRepository
{
    public IDbContextFactory<OrderHistoryDbContext> _dbContextFactory { get; set; } = dbContextFactory;

    public async Task<Domain.OrderHistory> AddAsync(Domain.OrderHistory orderHistory)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        await db.AddAsync(orderHistory);
        db.SaveChanges();

        return orderHistory;
    }

    public async Task UpdateAsync(Domain.OrderHistory orderHistory)
    {
        using var db = _dbContextFactory.CreateDbContext();

        db.Entry(orderHistory).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }

    public async Task Delete(Domain.OrderHistory orderHistory)
    {
        using var db = _dbContextFactory.CreateDbContext();

        db.OrdersHistory.Remove(orderHistory);
        await db.SaveChangesAsync();
    }

    public async Task<Domain.OrderHistory> GetByIdAsync(Guid id, Guid customerId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.OrdersHistory
                        .Where(o => o.Id.Equals(id) && o.CustomerId.Equals(customerId))
                        .Include(e => e.OrderItems)
                        .SingleOrDefaultAsync();
    }

    public async Task<List<Domain.OrderHistory>> SearchByDateAsync(DateOnly date)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.OrdersHistory
                        .Where(o => o.Created.Equals(date))
                        .Include(e => e.OrderItems)
                        .OrderBy(e => e.Created)
                        .ToListAsync();
    }

    public async Task<Domain.OrderHistory> OrderSummaryReadOnlyAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.OrdersHistory.AsNoTracking()
                        .Where(o => o.Id.Equals(id))
                        .Include(e => e.OrderItems)
                        .SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.OrdersHistory.AnyAsync(x => x.Id.Equals(id));
    }
}