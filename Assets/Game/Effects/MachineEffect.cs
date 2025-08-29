using Game.Core;

namespace Game.Effects
{
    public abstract class MachineEffect
    {
        public int Durability { get; protected set; }

        protected MachineEffect(int durability)
        {
            Durability = durability;
        }

        public virtual void OnBeforeSpin(SlotMachine machine) { }
        public virtual void OnAfterSpin(SlotMachine machine, SpinResult result) { }

        protected bool Tick()
        {
            Durability--;
            return Durability <= 0;
        }

        public bool Consume(SlotMachine machine, SpinResult result = null)
        {
            if (Tick())
            {
                OnExpire(machine);
                return true;
            }
            return false;
        }

        protected virtual void OnExpire(SlotMachine machine) { }
    }
}