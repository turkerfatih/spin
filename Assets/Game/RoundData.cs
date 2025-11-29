using System;
using Game.Event;

namespace Game
{
    [Serializable]
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
            RemainingSpins--;
            EventBus.OnSpinCountChanged?.Invoke(RemainingSpins);
        }

        public bool IsWin()
        {
            return Target <= Collected;
        }

        public bool IsLose()
        {
            return RemainingSpins <=0 &&  Services.Hand.NumberOfCardsInHand==0;
        }

        public void Collect(double amount)
        {
            Collected+=amount;
        }

    }
}