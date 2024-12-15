namespace Watering.Core.Options;

public class MqttOptions
{
    public required string Url { get; set; }
    public int Port { get; set; }
    public required string Topic { get; set; }
}