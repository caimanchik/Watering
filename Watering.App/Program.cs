// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Watering.Console.Extensions;
using Watering.Core.Extensions;

var builder = Host.CreateDefaultBuilder();

builder
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    });

builder
    .ConfigureAppConfiguration(b =>
    {
        b.AddJsonFile("appsettings.json", optional: false);
    });

builder
    .ConfigureServices((context, services) =>
    {
        services
            .ConfigureMqtt(c => context.Configuration.Bind("Mqtt", c));
        
        services
            .ConfigureCore()
            .ConfigureConsoleApp();
    });

var app = builder.Build();

await app.RunAsync();
