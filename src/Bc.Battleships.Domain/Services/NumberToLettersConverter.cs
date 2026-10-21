using EnsureThat;

namespace Bc.Battleships.Domain.Services
{
    public static class NumberToLettersConverter
    {
        const string Letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ";

        public static string Convert(int number)
        {
            Ensure.That(number).IsGt(0);
            Ensure.That(number).IsLte(Letters.Length);

            return Letters[number - 1].ToString();
        }

        public static int Convert(string letter)
        {
            Ensure.That(letter).IsNotNullOrWhiteSpace();
            Ensure.That(letter).HasLength(1);

            return Letters.IndexOf(letter) + 1;
        }
    }
}
