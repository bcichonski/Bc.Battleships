using Bc.Battleships.Domain.ValueObjects;

namespace Bc.Battleships.Domain.AggregateRoots.Games.Players
{
    public class Player
    {
        public PlayerId Id { get; set; }

        public Player()
        {
            Id = new PlayerId();
        }

        private Player(PlayerId id)
        {
            Id = id;
        }
    }
}
