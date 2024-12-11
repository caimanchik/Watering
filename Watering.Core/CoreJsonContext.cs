using System.Text.Json.Serialization;
using Watering.Core.Entites.Info;
using Watering.Core.Entites.Settings;

namespace Watering.Core;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(SensorSettings))]
[JsonSerializable(typeof(SprinklerSettings))]
[JsonSerializable(typeof(WateringSettings))]

[JsonSerializable(typeof(SensorInfo))]
[JsonSerializable(typeof(SprinklerInfo))]
[JsonSerializable(typeof(WateringInfo))]
public partial class CoreJsonContext : JsonSerializerContext;