using System.Collections.Generic;

namespace Game.Core
{
    public class SpinResult
    {
        public List<Symbol> Inputs;
        public List<PayoutResult> Payouts;

        public SpinResult()
        {
            Inputs=new List<Symbol>(6);
            Payouts = new List<PayoutResult>(6);
        }
    }
}