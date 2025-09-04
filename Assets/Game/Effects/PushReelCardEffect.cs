using Game.Actions;
using Game.Core;
using Game.Effect;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Cards/Effects/Push Card Effect", order = 0)]
    public class PushReelCardEffect:CardEffect
    {
        public override void OnPlay(Card card, int targetReelIndex = -1)
        {
            Services.Actions.Add(new PullAction(targetReelIndex,push:true));
        }
    }
}