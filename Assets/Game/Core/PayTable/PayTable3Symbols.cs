using System;

namespace Game.Core
{
    public class PayTable3Symbols:IPayTableRow
    {
        private SymbolType[] items;
        private int reward;
        public PayTable3Symbols(SymbolType[] items,int reward)
        {
            this.items = items;
            this.reward = reward;
        }

        public int Check(Span<SymbolType> symbols)
        {
            if(Contains(symbols[0]) && Contains(symbols[1]) && Contains(symbols[2]))
                return reward;
            return 0;
        }

        private bool Contains(SymbolType symbol)
        {
            //if(symbol == SymbolType.Wild)
            //   return true;
            foreach (var t in items)
            {
                if (symbol == t)
                    return true;
            }

            return false;
        }
    }
}