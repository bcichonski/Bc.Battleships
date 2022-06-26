namespace Bc.Battleships.Domain.ValueObjects
{
    public struct GameId
    {
        public Guid Value { get; set; }

        public GameId()
        {
            Value = Guid.NewGuid();
        }

        public GameId(Guid value)
        {
            Value = value;
        }
    }
}
