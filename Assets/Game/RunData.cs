using System;
using System.Collections.Generic;
using CardFramework.RandomProvider;

namespace Game
{
    [Serializable]
    public class RunData
    {
        public List<string> Cards;
        public int HandCount;
        public string Seed;
        public RandomStateData RandomState;
        public int Round;
        public List<Symbol> Symbols;
    }
}