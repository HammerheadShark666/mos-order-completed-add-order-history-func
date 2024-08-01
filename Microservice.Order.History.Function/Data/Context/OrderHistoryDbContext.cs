using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.History.Function.Data.Contexts;

public class OrderHistoryDbContext : DbContext
{ 
    public OrderHistoryDbContext(DbContextOptions<OrderHistoryDbContext> options) : base(options) { }
 
    public DbSet<Domain.OrderHistory> OrdersHistory { get; set; }
    public DbSet<Domain.OrderItem> OrderItems { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        base.OnModelCreating(modelBuilder);  

        modelBuilder.Entity<Domain.OrderHistory>().HasMany(e => e.OrderItems);
        modelBuilder.Entity<Domain.OrderItem>().HasKey(e => new { e.OrderId, e.ProductId }); 
    }
}

//add-migration
//update-database

//azurite --silent --location c:\azurite --debug c:\azurite\debug.log