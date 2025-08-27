namespace Game.Core
{
    public class PayoutResult
    {
        public SymbolType Symbol { get; }
        public int Count { get; }
        public int Amount { get; }

        public PayoutResult(SymbolType symbol, int count, int amount)
        {
            Symbol = symbol;
            Count = count;
            Amount = amount;
        }
    }
}