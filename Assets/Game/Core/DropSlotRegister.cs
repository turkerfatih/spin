using System;
using UnityEngine;

namespace Game.Core
{
    public class DropSlotRegister:MonoBehaviour, IDropSlotProvider
    {
        [SerializeField]
        private DropSlot[] Slots;

        private void Awake()
        {
            Services.DropSlot = this;
        }

        public DropSlot GetDropSlot(int slot)
        {
            //just +1 for now since other slots are not open yet
            return Slots[slot+1];
        }

        public DropSlot[] GetDropSlots()
        {
            return Slots;
        }
    }
}