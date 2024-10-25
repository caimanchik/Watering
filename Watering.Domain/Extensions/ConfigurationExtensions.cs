using Microsoft.Extensions.DependencyInjection;
using Watering.Domain.Ground.Services;

namespace Watering.Domain.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureDomain(this IServiceCollection services)
    {
        services.AddScoped<IGroundService, GroundService>();
        
        return services;
    }
}