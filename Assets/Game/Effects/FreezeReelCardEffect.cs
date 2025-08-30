using System;
using Game.Core;
using Game.Effect;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Cards/Effects/FreezeReelCardEffect", fileName = "FreezeReelCardEffect", order = 0)]
    [Serializable]
    public class FreezeReelCardEffect:CardEffect
    {
        public int Durability = 2;

        public override void OnPlay(Card card, int targetReelIndex = -1)
        {
            if (targetReelIndex < 0) return;

            var reel = Services.Machine.Reels[targetReelIndex];
            reel.AddEffect(new FreezeReelEffect(Durability));
        }
        
    }
}
