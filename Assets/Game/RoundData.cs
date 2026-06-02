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
        public int BeginCoinCount;
        public int RemainingCoins;
        public int No;

        public RoundData()
        {
        }

        public RoundData(int reelCount, int debt, int beginCoinCount)
        {
            this.Debt=debt;
            this.BeginCoinCount=beginCoinCount;
            this.ReelCount=reelCount;
            this.RemainingCoins = beginCoinCount;
        }

        public void OnSpin()
        {
            RemainingCoins--;
            EventBus.OnCreditsChanged?.Invoke(RemainingCoins);
        }

        public void OnDraw()
        {
            RemainingCoins--;
            EventBus.OnCreditsChanged?.Invoke(RemainingCoins);
        }

        public bool IsWin()
        {
            return Debt <= Collected;
        }

        public bool IsLose()
        {
            return RemainingCoins <=0 &&  Services.Hand.NumberOfCardsInHand==0;
        }

        public void Collect(int amount)
        {
            Collected+=amount;
        }

    }
}