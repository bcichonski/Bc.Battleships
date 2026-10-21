namespace Bc.Battleships.Domain.Services
{
    public static class NumberToLettersConverter
    {
        const string Letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ";

        public static string Convert(int number)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(number, Letters.Length);

            return Letters[number - 1].ToString();
        }

        public static int Convert(string letter)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(letter);
            if (letter.Length != 1)
            {
                throw new ArgumentException("Letter must be exactly one character long.", nameof(letter));
            }

            return Letters.IndexOf(letter) + 1;
        }
    }
}
