using Microsoft.Extensions.Hosting;
using Watering.Core.ServicesBase.Interfaces;

namespace Watering.Core.Ground.Interfaces;

internal interface ISensorService : ISettingsChangeService, IHostedService
{
}