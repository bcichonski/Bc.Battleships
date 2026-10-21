namespace Bc.Battleships.Domain.ValueObjects
{
    public struct ShipId
    {
        public Guid Value { get; set; }

        public ShipId()
        {
            Value = Guid.NewGuid();
        }

        public ShipId(Guid value)
        {
            Value = value;
        }
    }
}
