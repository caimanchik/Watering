using Watering.Core.Entites.Settings;

namespace Watering.Core.Client.Interfaces;

public interface IWateringClient
{
    void SendSettingsChange(SettingsBase settings);
}