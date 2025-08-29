using System;
using Game.Core;

namespace Game.Effect
{
    [Serializable]
    public abstract class CardEffect
    {
        public abstract void OnPlay(Card card, int targetReelIndex = -1);
        public abstract CardEffect Clone();
    }
}