using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(menuName = "Cards/Effects/Freeze Reel", fileName = "Freeze Reel Effect")]
    public class FreezeReelEffectDefinition:CardEffectDefinition
    {
        public int Duration = 2;
        public override IGameEffect CreateRuntimeEffect()
        {
            return new FreezeReelEffect(Duration);
        }
    }
}