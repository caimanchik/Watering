using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;

namespace Watering.Core.Client.Interfaces;

internal interface IControlClient : IClient
{
    void RegisterSettingsChange<T>(Func<T, Task> action) where T: SettingsBase;
    Task SendInfo(InfoBase info);
}