using Watering.Core.Entites.Settings;

namespace Watering.Core.Client.Interfaces;

public interface IWateringClient : IClient
{
    Task SendSettingsChange(SettingsBase settings);
}