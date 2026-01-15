using System;
using Cysharp.Threading.Tasks;
using Game.Event;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public class RoundData
    {
        public int Collected;
        public int Debt;
        public int ReelCount;
        public int BeginCreditCount;
        public int RemainingCredits;
        public int No;

        public RoundData()
        {
        }

        public RoundData(int reelCount, int debt, int beginCreditCount)
        {
            this.Debt=debt;
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
            return Debt <= Collected;
        }

        public bool IsLose()
        {
            return RemainingCredits <=0 &&  Services.Hand.NumberOfCardsInHand==0;
        }

        public void Collect(int amount)
        {
            Collected+=amount;
        }

    }
}