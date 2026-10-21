namespace Bc.Battleships.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static T GetRandomValue<T>(Random random) where T : struct, System.Enum
        {
            var values = Enum.GetValues<T>();

            return values[random.Next(values.Length)];
        }
    }
}
