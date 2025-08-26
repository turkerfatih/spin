namespace CardFramework.RandomProvider
{
    public interface IRandomProvider
    {
        public int Next();
        public int Range(int min, int max);
        public int Next(int max);
        public float NextFloat();
        public bool NextBool();
    }
}