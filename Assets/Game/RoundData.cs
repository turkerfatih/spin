using System;
using Cysharp.Threading.Tasks;
using Game.Event;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public class RoundData
    {
        public double Collected;
        public double Target;
        public int ReelCount;
        public int BeginCreditCount;
        public int RemainingCredits;
        public int Number;

        public RoundData()
        {
        }

        public RoundData(int reelCount, double target, int beginCreditCount)
        {
            this.Target=target;
            this.BeginCreditCount=beginCreditCount;
            this.ReelCount=reelCount;
            this.RemainingCredits = beginCreditCount;
        }

        public void Spin()
        {
            RemainingCredits--;
            EventBus.OnCreditsChanged?.Invoke(RemainingCredits);
        }

        public void Draw()
        {
            RemainingCredits--;
            EventBus.OnCreditsChanged?.Invoke(RemainingCredits);
            
            //do candle animation
            Services.Hand.RenewHand().Forget();
          
        }

        public bool IsWin()
        {
            return Target <= Collected;
        }

        public bool IsLose()
        {
            return RemainingCredits <=0 &&  Services.Hand.NumberOfCardsInHand==0;
        }

        public void Collect(double amount)
        {
            Collected+=amount;
        }

    }
}