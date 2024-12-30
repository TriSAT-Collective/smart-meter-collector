using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using trisatenergy_SMCollector.SmartMeterCollector;

namespace trisatenergy_SMCollector;

internal class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config
                    .SetBasePath(AppDomain.CurrentDomain
                        .BaseDirectory) // Set the base path to the current directory
                    .AddJsonFile("config.json", false, true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Register AppSettings as a configuration instance
                services.Configure<AppSettings>(context.Configuration.GetSection(nameof(AppSettings)));
                // Add logging
                services.AddLogging(builder =>
                {
                    builder.AddConfiguration(context.Configuration.GetSection("AppSettings:Logging"));
                    builder.AddConsole();
                });
                // Register the SmartMeter service
                services.AddTransient<MessageCollector>();
            })
            .Build();

        using IServiceScope scope = host.Services.CreateScope();
        var messageCollector = scope.ServiceProvider.GetRequiredService<MessageCollector>();
        await messageCollector.StartCollectingAsync();

        await host.WaitForShutdownAsync();
    }
}