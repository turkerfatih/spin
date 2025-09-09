using System.Collections.Generic;

namespace Game.Core
{
    public class PayoutResult
    {
        public List<Symbol> Symbols { get; }
        public List<int> Indexes;
        public int Amount { get; }

        public PayoutResult(List<Symbol> symbols, int amount)
        {
            Symbols = symbols;
            Amount = amount;
        }
    }
}