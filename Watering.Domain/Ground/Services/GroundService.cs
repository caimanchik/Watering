using Watering.Domain.Ground.Entites;

namespace Watering.Domain.Ground.Services;

public class GroundService : IGroundService
{
    public GroundStatus GroundStatus { get; }
    public void SetWateringLevels()
    {
        throw new NotImplementedException();
    }

    public int IncreaseHumidity(int delta = 1)
    {
        throw new NotImplementedException();
    }
}