using Microsoft.Extensions.Hosting;
using Watering.Core.Entites.Info;
using Watering.Core.ServicesBase.Interfaces;

namespace Watering.Core.Services.Interfaces;

public interface IInfoService : IInfoChangeService, IHostedService
{
    AllInfo GetAllInfo();
}