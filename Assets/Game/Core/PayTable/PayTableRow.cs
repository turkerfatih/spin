using System;

namespace Game.Core
{
    public  class PayTableRow:IPayTableRow
    {
        public int Check(Span<SymbolType> symbols)
        {
            return 0;
        }
    }
}