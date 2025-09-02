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
            int i = 0;

            while (i < n)
            {
                if (symbols[i].Type == SymbolType.None)
                {
                    i++;
                    continue;
                }

                // Find base type for this run
                SymbolType baseType = symbols[i].Type;

                if (baseType == SymbolType.Wild)
                {
                    for (int k = i + 1; k < n; k++)
                    {
                        if (symbols[k].Type != SymbolType.Wild && symbols[k].Type != SymbolType.None)
                        {
                            baseType = symbols[k].Type;
                            break;
                        }
                    }
                    // Still wild means it's an all-wild block, jackpot handled later
                    if (baseType == SymbolType.Wild)
                    {
                        break;
                    }
                }

                // Collect contiguous run
                List<Symbol> chain = new();
                int j = i;
                while (j < n)
                {
                    var sym = symbols[j];
                    if (sym.Type == SymbolType.None)
                        break;

                    if (sym.Type == baseType || sym.Type == SymbolType.Wild)
                    {
                        chain.Add(sym);
                        j++;
                    }
                    else
                        break;
                }

                // Only score the *longest run* of this block
                if (chain.Count >= 3 && table.TryGetValue((baseType, chain.Count), out int payout))
                {
                    results.Add(new PayoutResult(new List<Symbol>(chain), payout));
                }

                i = j; // move to next block
            }

            // Special case: all wilds = jackpot
            if (symbols.All(s => s.Type == SymbolType.Wild))
            {
                if (table.TryGetValue((SymbolType.Jackpot, symbols.Count), out int payout))
                {
                    results.Add(new PayoutResult(new List<Symbol>(symbols), payout));
                }
            }
        }

    }
    
}
