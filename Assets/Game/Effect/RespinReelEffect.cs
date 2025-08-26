using CardFramework;
using UnityEngine;

namespace Game.Effect
{
    [CreateAssetMenu(menuName = "Create RespinReelEffect", fileName = "RespinReelEffect", order = 0)]
    public class RespinReelEffect:CardEffect
    {
        public override void ApplyEffect(int slotIndex)
        {
            Services.MachineView.ReelSpin(slotIndex);
        }
    }
}