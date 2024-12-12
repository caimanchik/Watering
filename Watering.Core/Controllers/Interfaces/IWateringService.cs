using Microsoft.Extensions.Hosting;
using Watering.Core.ServicesBase.Interfaces;

namespace Watering.Core.Controllers.Interfaces;

internal interface IWateringService : ISettingsChangeService, IInfoChangeService, IHostedService;