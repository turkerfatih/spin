namespace CardFramework.RandomProvider
{
    using System.Security.Cryptography;
    using System.Text;

    public static class SeedGenerator
    {
        private static readonly char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        private static readonly StringBuilder Builder = new StringBuilder();

        // Method to generate a random alphanumeric string of a given length
        public static string Readable(int length = 8)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            char[] seedChars = new char[length];

            for (int i = 0; i < length; i++)
            {
                byte[] randomByte = new byte[1];
                rng.GetBytes(randomByte);
                seedChars[i] = Characters[randomByte[0] % Characters.Length];
            }

            return new string(seedChars);
        }

        public static ulong ToNumeric(string seed, int minLength = 8)
        {

            Builder.Clear();
            Builder.Append(seed);

            // Pad the seed if it's shorter than the minimum required length
            if (seed.Length < minLength && seed.Length > 0)
            {
                int originalLength = seed.Length;
                for (int j = originalLength; j < minLength; j++)
                {
                    int originalIndex = j % originalLength;
                    int offset = j / originalLength;
                    char newChar = (char)(seed[originalIndex] + offset);
                    Builder.Append(newChar);
                }
            }

            // Convert the padded seed to a numeric value
            ulong numericSeed = 0;
            var str = Builder.ToString();
            foreach (char c in str)
            {
                numericSeed = numericSeed * 31 + c;
            }

            return numericSeed;
        }
    }
}