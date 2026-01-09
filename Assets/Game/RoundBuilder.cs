using System.Collections.Generic;

namespace Game
{
    public static class RoundBuilder
    {
        public static RoundData GetNextRound(RoundData current)
        {
            var round = new RoundData
            {
                Number = current.Number+1,
                Target = current.Target*2,
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
                Number = 1,
                Target = 25,
                RemainingCredits = 5,
                BeginCreditCount = 5,
                Collected = 0
            };
            return round;
        }
    }
}