using System;
using Game.Core;

namespace Game
{
    [Serializable]
    public class StartingDeckItem
    {
        public CardDefinition Data;
        public int Count;
    }
}