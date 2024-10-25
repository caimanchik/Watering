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
        services.AddScoped<ICommandBase, InfoCommand>();
        services.AddScoped<ICommandService, CommandService>();
        
        return services;
    }
}