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

        public void Evaluate(List<Symbol> symbols,List<PayoutResult> results)
        {
            results.Clear();

            if (symbols.Count < 3)
                return ;

            Dictionary<SymbolType, List<Symbol>> groups = new();
            List<Symbol> wilds = new();

            // Group symbols
            foreach (var sym in symbols)
            {
                if (sym.Type == SymbolType.None)
                    continue;

                if (sym.Type == SymbolType.Wild)
                {
                    wilds.Add(sym);
                    continue;
                }

                if (!groups.TryGetValue(sym.Type, out var list))
                {
                    list = new List<Symbol>();
                    groups[sym.Type] = list;
                }
                list.Add(sym);
            }

            // Evaluate each group
            foreach (var kv in groups)
            {
                int total = kv.Value.Count + wilds.Count;
                if (total >= 3 && table.TryGetValue((kv.Key, total), out int payout))
                {
                    List<Symbol> winningSymbols = new List<Symbol>(kv.Value);
                    winningSymbols.AddRange(wilds); // clone wilds for this win

                    results.Add(new PayoutResult(winningSymbols, payout));
                }
            }

            // Special case: all wilds = jackpot
            if (wilds.Count >= 3 && wilds.Count == symbols.Count)
            {
                if (table.TryGetValue((SymbolType.Jackpot, wilds.Count), out int payout))
                {
                    results.Add(new PayoutResult(new List<Symbol>(wilds), payout));
                }
            }

           
        }
    }
    
}
