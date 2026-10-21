namespace Bc.Battleships.Messages.CommandsResults
{
    public class ShootResult
    {
        public bool Hit { get;  }
        public int CurrentHitPoints { get; }

        public ShootResult(bool hit, int currentHitPoints)
        {
            Hit = hit;
            CurrentHitPoints = currentHitPoints;
        }
    }
}
