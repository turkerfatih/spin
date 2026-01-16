using System.Collections.Generic;

namespace Game
{
    public static class RoundBuilder
    {
        public static RoundData GetNextRound(RoundData current)
        {
            var round = new RoundData
            {
                No = current.No+1,
                Debt = current.Debt*2,
                RemainingCoins = 5,
                BeginCoinCount = 5,
                Collected = 0
            };
            return round;
        }

        public static RoundData GetFirstRound()
        {
            var round = new RoundData
            {
                No = 1,
                Debt = 10,
                RemainingCoins = 7,
                BeginCoinCount = 7,
                Collected = 0
            };
            return round;
        }
    }
}