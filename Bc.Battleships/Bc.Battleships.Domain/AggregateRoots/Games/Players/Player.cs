using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players
{
    public abstract class Player
    {
        public PlayerId Id { get; set; }

        public Player()
        {
            Id = new PlayerId();
        }

        internal Player(PlayerId id)
        {
            Id = id;
        }
    }
}
