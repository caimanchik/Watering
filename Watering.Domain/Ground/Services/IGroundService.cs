using Watering.Domain.Ground.Entites;

namespace Watering.Domain.Ground.Services;

public interface IGroundService
{
    GroundStatus GroundStatus { get; }
    void SetWateringLevels();
    int IncreaseHumidity(int delta = 1);
}