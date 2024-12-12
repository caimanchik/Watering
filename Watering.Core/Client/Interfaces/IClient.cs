using Watering.Core.Entites.Info;

namespace Watering.Core.Client.Interfaces;

public interface IClient
{
    void RegisterInfoChange<T>(Action<T> action) where T: InfoBase;
}