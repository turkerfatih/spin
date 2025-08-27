namespace Game.Core
{
    public class NumberFormatter
    {
        public static string ToReadableString(double number)
        {
            if (number >= 1_000_000_000)
                return (number / 1_000_000_000D).ToString("0.#") + "B";
            if (number >= 1_000_000)
                return (number / 1_000_000D).ToString("0.#") + "M";
            if (number >= 1_000)
                return (number / 1_000D).ToString("0.#") + "K";
            if (number >= 1_000_000_000_000)
                return (number / 1_000_000_000_000D).ToString("0.#") + "T";

            return number.ToString("0");
        }
    }
}