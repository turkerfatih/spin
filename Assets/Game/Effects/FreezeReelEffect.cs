using Game.Core;

namespace Game.Effects
{
    public class FreezeReelEffect : ReelEffect
    {
        public FreezeReelEffect(int durability) : base(durability) { }

        public override void OnApply(Reel reel)
        {
            reel.IsFrozen = true; // apply immediately
        }
        

        public override void OnAfterSpin(Reel reel)
        {
            Consume(reel); // durability goes down after each spin attempt
        }

        public override void OnExpire(Reel reel)
        {
            reel.IsFrozen = false; // cleanup
        }
    }
}