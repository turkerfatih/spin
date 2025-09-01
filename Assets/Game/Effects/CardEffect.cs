using System;
using Game.Core;
using Game.Effects;
using UnityEngine;

namespace Game.Effect
{
    [Serializable]
    public abstract class CardEffect:ScriptableObject
    {
        public abstract void OnPlay(Card card, int targetReelIndex = -1);

    }
}