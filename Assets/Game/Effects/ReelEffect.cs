using Game.Core;

namespace Game.Effects
{
    public abstract class ReelEffect:IGameEffect
    {
        public int Durability { get; protected set; }

        protected ReelEffect(int durability)
        {
            Durability = durability;
        }

        /// <summary>
        /// Called once when the effect is applied to a reel.
        /// </summary>
        public virtual void OnApply(Reel reel) { }

        /// <summary>
        /// Called before a spin attempt. 
        /// Return false to block the reel from spinning.
        /// </summary>
        public virtual void OnBeforeSpin(Reel reel)
        {
        }

        /// <summary>
        /// Called after a spin attempt finishes (or is skipped).
        /// </summary>
        public virtual void OnAfterSpin(Reel reel) { }

        /// <summary>
        /// Called once when the effect expires or is removed.
        /// </summary>
        public virtual void OnExpire(Reel reel) { }

        /// <summary>
        /// Reduce durability and return true if expired.
        /// </summary>
        protected bool Tick()
        {
            Durability--;
            return Durability <= 0;
        }

        public bool Consume(Reel reel)
        {
            if (Tick())
            {
                OnExpire(reel);
                reel.RemoveEffect(this);
                return true;
            }
            return false;
        }

        public virtual void Apply()
        {
            
        }

        public virtual void OnBeforeSpin()
        {
            
        }

        public virtual void OnAfterSpin()
        {
            
        }
        
    }
}