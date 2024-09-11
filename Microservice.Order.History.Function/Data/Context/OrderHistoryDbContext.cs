using Microsoft.EntityFrameworkCore;

namespace Microservice.Order.History.Function.Data.Context;

public class OrderHistoryDbContext(DbContextOptions<OrderHistoryDbContext> options) : DbContext(options)
{
    public DbSet<Domain.OrderHistory> OrdersHistory { get; set; }
    public DbSet<Domain.OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<OrderHistory>()
        //               .HasMany(e => e.OrderItems)
        //               .WithOne(e => e.OrderHistory)
        //               .HasForeignKey(e => e.OrderId)
        //               .HasPrincipalKey(e => e.Id);

        //modelBuilder.Entity<OrderHistory>()
        //.HasOne(p => p.User)
        //.WithMany(u => u.Posts)
        //.HasForeignKey(p => p.UserId);

        //modelBuilder.Entity<OrderItem>()
        //    .HasOne(p => p.OrderId)
        //.WithMany(u => u.)
        //.HasForeignKey(p => p.UserId);

        // modelBuilder.Entity<Domain.OrderHistory>()
        //.HasMany(e => e.OrderItems)
        //.WithOne(e => e.Order)
        //.HasForeignKey(e => new { e.ContainingBlogId1, e.ContainingBlogId2 });


        //modelBuilder.Entity<Domain.OrderHistory>()
        //    .HasMany(e => e.OrderItems)
        //    .WithOne()
        //    .HasForeginKey<OrderItem>(c => c.);


        //modelBuilder.Entity<Domain.OrderHistory>().HasMany(e => e.OrderItems);
        modelBuilder.Entity<Domain.OrderItem>().HasKey(e => new { e.OrderId, e.ProductId });

    }
}