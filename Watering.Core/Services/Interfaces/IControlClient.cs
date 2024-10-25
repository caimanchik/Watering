using Watering.Core.Entites;
using Watering.Core.Settings;

namespace Watering.Core.Services.Interfaces;

internal interface IControlClient
{
    void RegisterSettingsChange<T>(Action<T> action) where T: SettingsBase;
}