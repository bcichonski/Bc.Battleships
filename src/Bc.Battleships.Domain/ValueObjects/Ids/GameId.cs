namespace Bc.Battleships.Domain.ValueObjects
{
    public record GameId
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
