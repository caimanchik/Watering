using Microsoft.Extensions.DependencyInjection;
using Watering.Core.Client;
using Watering.Core.Client.Interfaces;
using Watering.Core.Controllers;
using Watering.Core.Controllers.Interfaces;
using Watering.Core.Ground;
using Watering.Core.Ground.Interfaces;

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
            .AddSingleton<IGroundStateService, GroundStateService>()
            .AddSingleton<ISensorService, SensorService>()
            .AddSingleton<ISprinklerService, SprinklerService>()
            .AddSingleton<IWateringService>(provider => provider.GetRequiredService<WateringService>());
        
        services
            .AddHostedService<IGroundStateService>(s => s.GetRequiredService<IGroundStateService>())
            .AddHostedService<ISensorService>(s => s.GetRequiredService<ISensorService>())
            .AddHostedService<ISprinklerService>(s => s.GetRequiredService<ISprinklerService>())
            .AddHostedService<IWateringService>(s => s.GetRequiredService<IWateringService>());
        
        return services;
    }
}