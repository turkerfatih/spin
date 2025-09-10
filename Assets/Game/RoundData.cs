namespace Game
{
    public class RoundData
    {
        public double Collected;
        public double Target;
        public int ReelCount;
        public int BeginSpinCount;
        public int RemainingSpins;
        public int Number;

        public RoundData()
        {
        }

        public RoundData(int reelCount, double target, int beginSpinCount)
        {
            this.Target=target;
            this.BeginSpinCount=beginSpinCount;
            this.ReelCount=reelCount;
            this.RemainingSpins = beginSpinCount;
        }

        public void Spin()
        {
            
        }

    }
}