using Microservice.Order.History.Function.Helpers;
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
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()
                              ?? throw new Exception("Configuration not created.");

        ServiceExtension.ConfigureDependencyInjection(services);
        ServiceExtension.ConfigureApplicationInsights(services);
        ServiceExtension.ConfigureMediatr(services);
        ServiceExtension.ConfigureMemoryCache(services);
        ServiceExtension.ConfigureSqlServer(services, configuration);
        ServiceExtension.ConfigureServiceBusClient(services);

        //services.AddApplicationInsightsTelemetryWorkerService();
        //services.ConfigureFunctionsApplicationInsights();

        //var configuration = services.BuildServiceProvider().GetService<IConfiguration>()
        //                      ?? throw new Exception("Configuration not created.");

        //services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        //services.AddAutoMapper(Assembly.GetAssembly(typeof(AddOrderHistoryMapper)));
        //services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
        //services.AddScoped<IAzureServiceBusHelper, AzureServiceBusHelper>();
        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //services.AddMemoryCache();

        //services.AddAzureClients(builder =>
        //{
        //    builder.AddServiceBusClientWithNamespace(EnvironmentVariables.GetEnvironmentVariable(Constants.AzureServiceBusConnection));
        //    builder.UseCredential(new ManagedIdentityCredential());
        //});

        //services.AddDbContextFactory<OrderHistoryDbContext>(options =>
        //options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
        //    options => options.EnableRetryOnFailure()));
    })
    .Build();

await host.RunAsync();