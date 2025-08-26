using System;
using System.Collections.Generic;

namespace Game.Core
{
    public class PayTable:IPayTable
    {
        private static List<IPayTableRow> table = new List<IPayTableRow>()
        {
            new PayTable3Symbols(new[] { SymbolType.Cherry },5),
            new PayTable3Symbols(new[] { SymbolType.Orange },8),
            new PayTable3Symbols(new[] { SymbolType.Lemon },12),
            new PayTable3Symbols(new[] { SymbolType.Red7 },20),
            new PayTable3Symbols(new[] { SymbolType.Orange7 },40),
            new PayTable3Symbols(new[] { SymbolType.Yellow7 },100),
            new PayTable3Symbols(new[] { SymbolType.Jackpot },700),
        };


        public int Check(Span<SymbolType> symbols)
        {
            foreach (var row in table)
            {
                var result = row.Check(symbols);
                if(result>0)
                    return result;
            }
            return 0;
        }
    }
}