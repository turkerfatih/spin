using System;
using System.Collections.Generic;
using System.Linq;

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

    public void Evaluate(List<Symbol> symbols, List<PayoutResult> results)
    {
        results.Clear();

        if (symbols.Count < 3)
            return;

        int n = symbols.Count;

        // Special case: all wilds = jackpot
        if (symbols.All(s => s.Type == SymbolType.Wild))
        {
            if (table.TryGetValue((SymbolType.Jackpot, n), out int jackpotPayout))
            {
                results.Add(new PayoutResult(new List<Symbol>(symbols), jackpotPayout));
            }
            return;
        }

        // Track the best (longest) run we find per base type
        var bestRuns = new Dictionary<SymbolType, (int length, List<Symbol> chain)>();

        for (int start = 0; start < n; start++)
        {
            if (symbols[start].Type == SymbolType.None)
                continue;

            // Figure out candidate base types
            HashSet<SymbolType> candidates = new();
            if (symbols[start].Type != SymbolType.Wild)
            {
                candidates.Add(symbols[start].Type);
            }
            else
            {
                // Look ahead to seed candidate
                for (int k = start + 1; k < n; k++)
                {
                    if (symbols[k].Type != SymbolType.Wild && symbols[k].Type != SymbolType.None)
                    {
                        candidates.Add(symbols[k].Type);
                        break;
                    }
                }
            }

            foreach (var baseType in candidates)
            {
                int end = start;
                while (end < n &&
                    (symbols[end].Type == baseType ||
                        symbols[end].Type == SymbolType.Wild))
                {
                    end++;
                }

                int length = end - start;
                if (length >= 3 && table.TryGetValue((baseType, length), out int payout))
                {
                    var segment = symbols.GetRange(start, length);

                    // Only keep longest run per baseType
                    if (!bestRuns.TryGetValue(baseType, out var current) || length > current.length)
                    {
                        bestRuns[baseType] = (length, segment);
                    }
                }
            }
        }

        // Add results for each baseType’s longest run
        foreach (var kvp in bestRuns)
        {
            var (len, chain) = kvp.Value;
            int payout = table[(kvp.Key, len)];
            results.Add(new PayoutResult(chain, payout));
        }
    }

    }
    
}
