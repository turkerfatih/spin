using Game.Core;
using UnityEngine;

namespace Game.Effects
{
    public class FreezeReelEffect : ReelEffect
    {
        public FreezeReelEffect(int durability) : base(durability) { }

        public override void OnApply(Reel reel)
        {
            //Debug.Log("Applying Freeze Reel Effect");
            reel.IsFrozen = true; // apply immediately
        }
        

        public override void OnAfterSpin(Reel reel)
        {
            //Debug.Log("OnAfterSpin freeze Consume");
            Consume(reel); // durability goes down after each spin attempt
        }

        public override void OnExpire(Reel reel)
        {
            //Debug.Log("OnExpire freeze");
            reel.IsFrozen = false; // cleanup
        }
    }
}