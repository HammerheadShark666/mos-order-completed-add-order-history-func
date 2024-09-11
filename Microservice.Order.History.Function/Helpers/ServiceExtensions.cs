using MediatR;
using Microservice.Order.History.Function.Data.Context;
using Microservice.Order.History.Function.Data.Repository;
using Microservice.Order.History.Function.Data.Repository.Interfaces;
using Microservice.Order.History.Function.Helpers.Interfaces;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Microservice.Order.History.Function.Helpers;

public static class ServiceExtensions
{
    public static void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddOrderHistoryMapper)));
        services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
        services.AddScoped<IAzureServiceBusHelper, AzureServiceBusHelper>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static void ConfigureApplicationInsights(IServiceCollection services)
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    }

    public static void ConfigureMediatr(IServiceCollection services)
    {
        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
    }

    public static void ConfigureSqlServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<OrderHistoryDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()));
    }

    public static void ConfigureMemoryCache(IServiceCollection services)
    {
        services.AddMemoryCache();
    }

    public static void ConfigureServiceBusClient(IServiceCollection services, IWebHostEnvironment environment)
    {
        //services.AddAzureClients(builder =>
        //{
        //    builder.AddServiceBusClientWithNamespace(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnection));
        //    builder.UseCredential(new ManagedIdentityCredential());
        //});

        //if (environment.IsDevelopment())
        //{ 
        //    services.AddAzureClients(builder =>
        //    {
        //        builder.AddServiceBusClient(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnectionString));
        //    });
        //}
        //else
        //{
        //    services.AddAzureClients(builder =>
        //    {
        //        builder.AddServiceBusClientWithNamespace(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnectionManagedIdentity));
        //        builder.UseCredential(new ManagedIdentityCredential());
        //    });
        //}
    }

    public static void ConfigureLogging(IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            logging.AddConsole();
        });
    }
}
