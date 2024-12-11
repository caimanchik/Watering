using Watering.Core.Entites;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;

namespace Watering.Core.Client.Interfaces;

internal interface IControlClient
{
    void RegisterSettingsChange<T>(Action<T> action) where T: SettingsBase;
    
    void RegisterInfoChange<T>(Action<T> action) where T: InfoBase;

    void SendInfo(InfoBase info);
}