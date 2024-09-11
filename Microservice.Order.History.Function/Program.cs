using Microservice.Order.History.Function.Data.Context;
using Microservice.Order.History.Function.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(c =>
    {
        c.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        var builder = WebApplication.CreateBuilder(args);
        var environment = builder.Environment;

        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()
                              ?? throw new Exception("Configuration not created.");

        ServiceExtensions.ConfigureApplicationInsights(services);
        ServiceExtensions.ConfigureMediatr(services);
        ServiceExtensions.ConfigureDependencyInjection(services);
        ServiceExtensions.ConfigureMemoryCache(services);
        //ServiceExtensions.ConfigureSqlServer(services, configuration);
        //ServiceExtensions.ConfigureServiceBusClient(services, environment);

        //if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"))
        //        || Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development")
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

        services.AddDbContextFactory<OrderHistoryDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()));

        ServiceExtensions.ConfigureLogging(services);
    })
    .Build();

await host.RunAsync();