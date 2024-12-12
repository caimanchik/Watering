// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Watering.Console.Extensions;
using Watering.Core.Extensions;

var hostBuilder = Host.CreateDefaultBuilder()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) =>
    {
        services
            .ConfigureCore()
            .ConfigureConsoleApp();
        // services.Configure<Options>(c => context.Configuration.Bind("Options", c)); // пригодится в следующей лабораторной работе
    });

var app = hostBuilder.Build();

await app.RunAsync();
