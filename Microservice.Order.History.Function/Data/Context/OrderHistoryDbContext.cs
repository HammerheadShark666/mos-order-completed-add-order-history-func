using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.History.Function.Data.Context;

public class OrderHistoryDbContext(DbContextOptions<OrderHistoryDbContext> options) : DbContext(options)
{
    public DbSet<Domain.OrderHistory> OrdersHistory { get; set; }
    public DbSet<Domain.OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Domain.OrderItem>().HasKey(e => new { e.OrderId, e.ProductId });
    }
}