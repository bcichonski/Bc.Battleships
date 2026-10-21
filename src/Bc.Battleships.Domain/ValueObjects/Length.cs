using EnsureThat;
using Bc.Battleships.Domain.ValueObjects.Exceptions;

namespace Bc.Battleships.Domain.ValueObjects
{
    public record Length
    {
        public int Value { get; }

        public Length(int value)
        {
            Ensure.That(value >= 0, optsFn: x => x.WithException(new NegativeLengthValueException()));

            Value = value;
        }
    }
}
