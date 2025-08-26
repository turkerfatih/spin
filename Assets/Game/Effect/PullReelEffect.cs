using UnityEngine;

namespace Game.Effect
{
    [CreateAssetMenu(menuName = "Create PullReelEffect", fileName = "PullReelEffect", order = 0)]
    public class PullReelEffect:CardEffect
    {
        public override void ApplyEffect(int slotIndex)
        {
            Services.MachineView.ReelPull(slotIndex);
        }
    }
}