using Watering.Core.Settings;

namespace Watering.Core.Services.Interfaces;

public interface IWateringClient
{
    void SendSettingsChange(SettingsBase settings);
}