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
                RemainingCredits = 5,
                BeginCreditCount = 5,
                Collected = 0
            };
            return round;
        }

        public static RoundData GetFirstRound()
        {
            var round = new RoundData
            {
                No = 1,
                Debt = 25,
                RemainingCredits = 5,
                BeginCreditCount = 5,
                Collected = 0
            };
            return round;
        }
    }
}