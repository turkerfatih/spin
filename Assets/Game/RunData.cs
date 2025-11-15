using System;
using System.Collections.Generic;
using CardFramework.RandomProvider;
using Game.Core;

namespace Game
{
    [Serializable]
    public class RunData
    {
        public List<CardSaveData> Cards;
        public int HandCount;
        public string Seed;
        public RandomStateData RandomState;
        public int Round;
        public List<Symbol> Symbols;
        public RunData CurrentRoundData;
    }
}