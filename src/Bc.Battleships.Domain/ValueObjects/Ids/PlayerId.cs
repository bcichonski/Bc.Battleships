namespace Bc.Battleships.Domain.ValueObjects
{
    public struct PlayerId
    {
        public Guid Value { get; set; }

        public PlayerId()
        {
            Value = Guid.NewGuid();
        }

        public PlayerId(Guid value)
        {
            Value = value;
        }
    }
}
