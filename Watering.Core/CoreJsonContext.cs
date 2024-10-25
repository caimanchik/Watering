using System.Text.Json.Serialization;
using Watering.Core.Entites;
using Watering.Core.Settings;

namespace Watering.Core;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(SensorSettings))]
[JsonSerializable(typeof(SprinklerSettings))]
[JsonSerializable(typeof(SensorInfo))]
[JsonSerializable(typeof(WateringSettings))]
public partial class CoreJsonContext : JsonSerializerContext;