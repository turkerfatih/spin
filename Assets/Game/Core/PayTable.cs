using System;
using System.Collections.Generic;

namespace Game.Core
{
    public class PayTable
    {
        private static readonly Dictionary<(SymbolType, int), int> table = new()
        {
            // Fruits
            { (SymbolType.Lemon, 3), 5 }, { (SymbolType.Lemon, 4), 10 },
            { (SymbolType.Lemon, 5), 20 }, { (SymbolType.Lemon, 6), 40 },

            { (SymbolType.Orange, 3), 6 }, { (SymbolType.Orange, 4), 12 },
            { (SymbolType.Orange, 5), 25 }, { (SymbolType.Orange, 6), 50 },

            { (SymbolType.Cherry, 3), 8 }, { (SymbolType.Cherry, 4), 16 },
            { (SymbolType.Cherry, 5), 35 }, { (SymbolType.Cherry, 6), 70 },

            // Sevens
            { (SymbolType.Yellow7, 3), 15 }, { (SymbolType.Yellow7, 4), 30 },
            { (SymbolType.Yellow7, 5), 60 }, { (SymbolType.Yellow7, 6), 120 },

            { (SymbolType.Orange7, 3), 20 }, { (SymbolType.Orange7, 4), 40 },
            { (SymbolType.Orange7, 5), 80 }, { (SymbolType.Orange7, 6), 160 },

            { (SymbolType.Red7, 3), 25 }, { (SymbolType.Red7, 4), 50 },
            { (SymbolType.Red7, 5), 100 }, { (SymbolType.Red7, 6), 200 },

            // Jackpot
            { (SymbolType.Jackpot, 3), 100 }, { (SymbolType.Jackpot, 4), 500 },
            { (SymbolType.Jackpot, 5), 2000 }, { (SymbolType.Jackpot, 6), 10000 },
        };

        public  List<PayoutResult> Evaluate(Span<SymbolType> symbols)
        {
            List<PayoutResult> results = new();

            if (symbols.Length < 3)
                return results;

            Dictionary<SymbolType, int> counts = new();
            int wilds = 0;

            foreach (var sym in symbols)
            {
                if (sym == SymbolType.None)
                    continue;

                if (sym == SymbolType.Wild)
                {
                    wilds++;
                    continue;
                }

                if (!counts.ContainsKey(sym))
                    counts[sym] = 0;

                counts[sym]++;
            }

            // Check for each symbol type
            foreach (var kv in counts)
            {
                int total = kv.Value + wilds;
                if (total >= 3 && table.TryGetValue((kv.Key, total), out int payout))
                {
                    results.Add(new PayoutResult(kv.Key, total, payout));
                }
            }

            // Edge case: All Wilds = Jackpot
            if (wilds >= 3)
            {
                if (table.TryGetValue((SymbolType.Jackpot, wilds), out int payout))
                {
                    results.Add(new PayoutResult(SymbolType.Jackpot, wilds, payout));
                }
            }

            return results;
        }
    }
}