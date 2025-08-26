using System;

namespace Game
{
    public interface IPayTable
    {
        public int Check(Span<SymbolType> symbols);
    }
}