using Microsoft.Extensions.DependencyInjection;
using Watering.Core.Services.Interfaces;

namespace Watering.Core.Extensions;

public static class ServiceProviderExtensions
{
    public static void RunCore(this IServiceProvider collection)
    {
        var a = collection.GetRequiredService<ISensorService>();
        
    }
}