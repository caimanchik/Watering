using Microsoft.Extensions.DependencyInjection;
using Watering.Core.Services;
using Watering.Core.Services.Interfaces;

namespace Watering.Core.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureCore(this IServiceCollection services)
    {
        services
            .AddSingleton<WateringClient>()
            .AddSingleton<WateringService>();

        services
            .AddSingleton<IControlClient>(provider => provider.GetRequiredService<WateringClient>())
            .AddSingleton<IWateringClient>(provider => provider.GetRequiredService<WateringClient>());
        
        services
            .AddSingleton<IPipeInfoService>(provider => provider.GetRequiredService<WateringClient>())
            .AddSingleton<IPipeInfoService>(provider => provider.GetRequiredService<WateringService>());
        
        services
            .AddSingleton<IGroundStateService, GroundStateService>()
            .AddSingleton<ISensorService, SensorService>()
            .AddSingleton<ISprinklerService, SprinklerService>()
            .AddSingleton<IWateringService>(provider => provider.GetRequiredService<WateringService>());
        
        return services;
    }
}