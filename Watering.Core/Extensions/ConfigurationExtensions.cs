using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MQTTnet;
using Watering.Core.Client;
using Watering.Core.Client.Interfaces;
using Watering.Core.Controllers;
using Watering.Core.Controllers.Interfaces;
using Watering.Core.Ground;
using Watering.Core.Ground.Interfaces;
using Watering.Core.Options;
using Watering.Core.Services;
using Watering.Core.Services.Interfaces;

namespace Watering.Core.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureMqtt(this IServiceCollection services, Action<MqttOptions> action) 
        => services.Configure(action);

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services
            .AddSingleton<WateringClient>(p =>
            {
                var options = p.GetRequiredService<IOptions<MqttOptions>>().Value;
                var factory = new MqttFactory();
                var client = factory.CreateMqttClient();

                return new WateringClient(client, options);
            })
            .AddSingleton<WateringService>();

        services
            .AddSingleton<IControlClient>(provider => provider.GetRequiredService<WateringClient>())
            .AddSingleton<IWateringClient>(provider => provider.GetRequiredService<WateringClient>());
        
        services
            .AddSingleton<IGroundStateService, GroundStateService>()
            .AddSingleton<ISensorService, SensorService>()
            .AddSingleton<ISprinklerService, SprinklerService>()
            .AddSingleton<IWateringService>(provider => provider.GetRequiredService<WateringService>())
            .AddSingleton<IInfoService, InfoService>();
        
        services
            .AddHostedService<IClient>(s => s.GetRequiredService<WateringClient>())
            .AddHostedService<IInfoService>(s => s.GetRequiredService<IInfoService>())
            .AddHostedService<IGroundStateService>(s => s.GetRequiredService<IGroundStateService>())
            .AddHostedService<ISensorService>(s => s.GetRequiredService<ISensorService>())
            .AddHostedService<ISprinklerService>(s => s.GetRequiredService<ISprinklerService>())
            .AddHostedService<IWateringService>(s => s.GetRequiredService<IWateringService>());
        
        return services;
    }
}