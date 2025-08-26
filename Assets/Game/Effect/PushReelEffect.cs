using UnityEngine;

namespace Game.Effect
{
    [CreateAssetMenu(menuName = "Create PushReelEffect", fileName = "PushReelEffect", order = 0)]
    public class PushReelEffect:CardEffect
    {
        public override void ApplyEffect(int slotIndex)
        {
            Services.MachineView.ReelPush(slotIndex);
        }
    }
}