using Microsoft.Extensions.Hosting;
using Watering.Core.Entites.Info;

namespace Watering.Core.Client.Interfaces;

public interface IClient : IHostedService
{
    void RegisterInfoChange<T>(Func<T, Task> action) where T: InfoBase;
}