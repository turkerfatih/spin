using System.Collections.Generic;

namespace Game.Core
{
    public interface IDropSlotProvider
    {
        public DropSlot GetDropSlot(int index);
        public DropSlot[] GetDropSlots();
    }
}