using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Info;
using Watering.Core.Services.Interfaces;

namespace Watering.Core.Services;

internal class InfoService(IWateringClient wateringClient) : IInfoService
{
    private AllInfo AllInfo { get; set; } = new(null, null, null);

    public void RegisterInfoAction()
    {
        wateringClient.RegisterInfoChange<SensorInfo>(HandleInfoChange);
        wateringClient.RegisterInfoChange<SprinklerInfo>(HandleInfoChange);
        wateringClient.RegisterInfoChange<WateringInfo>(HandleInfoChange);
    }

    public AllInfo GetAllInfo() => AllInfo;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterInfoAction();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private Task HandleInfoChange(InfoBase info)
    {
        AllInfo = info switch
        {
            SensorInfo sensorInfo => new AllInfo(sensorInfo, AllInfo.SprinklerInfo, AllInfo.WateringInfo),
            SprinklerInfo sprinklerInfo => new AllInfo(AllInfo.SensorInfo, sprinklerInfo, AllInfo.WateringInfo),
            WateringInfo wateringInfo => new AllInfo(AllInfo.SensorInfo, AllInfo.SprinklerInfo, wateringInfo),
            _ => AllInfo
        };
        
        return Task.CompletedTask;
    }
}