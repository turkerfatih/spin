using Game.Actions;
using Game.Core;
using Game.Effect;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Cards/Effects/ReSpin Card Effect", order = 0)]
    public class ReSpinCardEffect:CardEffect
    {
        public override void OnPlay(Card card, int targetReelIndex = -1)
        {
            Services.Actions.Add(new SpinAction(targetReelIndex));
        }
    }
}