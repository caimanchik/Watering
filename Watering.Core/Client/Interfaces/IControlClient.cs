using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;

namespace Watering.Core.Client.Interfaces;

internal interface IControlClient : IClient
{
    void RegisterSettingsChange<T>(Action<T> action) where T: SettingsBase;
    void SendInfo(InfoBase info);
}