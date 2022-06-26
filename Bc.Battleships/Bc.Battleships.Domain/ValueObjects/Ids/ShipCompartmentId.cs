namespace Bc.Battleships.Domain.ValueObjects
{
    public struct ShipCompartmentId
    {
        public Guid Value { get; set; }

        public ShipCompartmentId()
        {
            Value = Guid.NewGuid();
        }

        public ShipCompartmentId(Guid value)
        {
            Value = value;
        }
    }
}
