using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Watering.Bot.Bot;
using Watering.Bot.Bot.Interfaces;
using Watering.Bot.Commands;
using Watering.Bot.Commands.Commands;
using Watering.Bot.Commands.Commands.Abstracts;
using Watering.Bot.Commands.Interfaces;
using Watering.Bot.Options;

namespace Watering.Bot.Extensions;

public static class ConfigurationsExtensions
{
    public static IServiceCollection ConfigureBot(this IServiceCollection services, Action<BotOptions> configureAction) 
        => services.Configure(configureAction);

    public static IServiceCollection AddBot(this IServiceCollection services)
    {
        services.AddSingleton<IBot>(s =>
        {
            var execetor = s.GetRequiredService<ICommandExecutor>();
            var options = s.GetRequiredService<IOptions<BotOptions>>().Value;
            var logger = s.GetRequiredService<ILogger<WateringBot>>();
            
            return new WateringBot(execetor, options, logger);
        });
        services.AddSingleton<ICommandExecutor, CommandExecutor>();

        services
            .AddSingleton<CommandBase, HelpCommand>()
            .AddSingleton<CommandBase, ErrorCommand>()
            .AddSingleton<CommandBase, InfoCommand>()
            .AddSingleton<CommandBase, SensorSettingsCommand>()
            .AddSingleton<CommandBase, SprinklerSettingsCommand>()
            .AddSingleton<CommandBase, WateringSettingsCommand>();
        
        services.AddHostedService<IBot>(s => s.GetRequiredService<IBot>());

        return services;
    }
}