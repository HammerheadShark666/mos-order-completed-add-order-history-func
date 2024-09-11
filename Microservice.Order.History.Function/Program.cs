using MediatR;
using Microservice.Order.History.Function.Data.Context;
using Microservice.Order.History.Function.Data.Repository;
using Microservice.Order.History.Function.Data.Repository.Interfaces;
using Microservice.Order.History.Function.Helpers;
using Microservice.Order.History.Function.MediatR.AddOrderHistory;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()
                              ?? throw new Exception("Configuration not created.");

        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddOrderHistoryMapper)));
        services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
        // services.AddScoped<IAzureServiceBusHelper, AzureServiceBusHelper>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddMemoryCache();

        //services.AddAzureClients(builder =>
        //{
        //    builder.AddServiceBusClientWithNamespace(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnection));
        //    builder.UseCredential(new ManagedIdentityCredential());
        //});

        services.AddDbContextFactory<OrderHistoryDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()));
    })
    .Build();

await host.RunAsync();