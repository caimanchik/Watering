using Microsoft.Extensions.DependencyInjection;
using Watering.Console.Commands;
using Watering.Console.Commands.Interfaces;
using Watering.Console.Services;
using Watering.Console.Services.Interfaces;

namespace Watering.Console.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureConsoleApp(this IServiceCollection services)
    {
        services.AddScoped<ICommandService, CommandService>();
        
        services.AddScoped<ICommandBase, InfoCommand>();
        services.AddScoped<ICommandBase, ExitCommand>();
        services.AddScoped<ICommandBase, SensorSettingsCommand>();
        services.AddScoped<ICommandBase, SprinklerSettingsCommand>();
        services.AddScoped<ICommandBase, WateringSettingsCommand>();
        services.AddScoped<ICommandBase, HelpCommand>();
        
        services.AddHostedService<ICommandService>(s => s.GetRequiredService<ICommandService>());
        
        return services;
    }
}