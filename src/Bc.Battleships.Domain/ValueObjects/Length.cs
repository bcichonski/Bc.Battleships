using Bc.Battleships.Domain.ValueObjects.Exceptions;

namespace Bc.Battleships.Domain.ValueObjects
{
    public record Length
    {
        public int Value { get; }

        public Length(int value)
        {
            if (value < 0)
            {
                throw new NegativeLengthValueException();
            }

            Value = value;
        }
    }
}
