using System;

namespace Game.Core
{
    public interface IPayTableRow
    {
        public int Check(Span<SymbolType> symbols);
    }
}