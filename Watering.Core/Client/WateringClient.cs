using System.Text;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Watering.Core.Client.Interfaces;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;
using Watering.Core.Options;

namespace Watering.Core.Client;

internal class WateringClient(IMqttClient client, MqttOptions options) : IWateringClient, IControlClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        TypeInfoResolver = CoreJsonContext.Default
    };

    public void RegisterSettingsChange<T>(Func<T, Task> action) where T : SettingsBase 
        => client.ApplicationMessageReceivedAsync += GetOnReceiveAction(action);

    public void RegisterInfoChange<T>(Func<T, Task> action) where T : InfoBase
        => client.ApplicationMessageReceivedAsync += GetOnReceiveAction(action);

    public async Task SendInfo(InfoBase info)
    {
        var jsonString = JsonSerializer.Serialize(info, info.GetType(), JsonOptions);
        await SendMessageAsync(jsonString);
    }

    public async Task SendSettingsChange(SettingsBase settings)
    {
        var jsonString = JsonSerializer.Serialize(settings, settings.GetType(), JsonOptions);
        await SendMessageAsync(jsonString);
    }

    private Func<MqttApplicationMessageReceivedEventArgs, Task> GetOnReceiveAction<T>(Func<T, Task> action)
    {
        return async received =>
        {
            try
            {
                if (!received.ApplicationMessage.Topic.Equals(options.Topic, StringComparison.OrdinalIgnoreCase))
                    return;
                
                var message = Encoding.UTF8.GetString(received.ApplicationMessage.PayloadSegment);
                var receivedSettings = JsonSerializer.Deserialize<T>(message, JsonOptions);
                if (receivedSettings is null)
                    return;

                await action(receivedSettings);
            }
            catch (Exception)
            {
                // ignored
            }
        };
    }

    private async Task SendMessageAsync(string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(options.Topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithRetainFlag()
            .Build();

        await client.PublishAsync(message);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var mqttOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(options.Url, options.Port)
            .Build();
        var connectResult = await client.ConnectAsync(mqttOptions, cancellationToken);

        if (connectResult.ResultCode is MqttClientConnectResultCode.Success)
            await client.SubscribeAsync(options.Topic, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}