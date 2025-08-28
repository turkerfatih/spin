using System;
using Game.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class DropSlot:MonoBehaviour
    {
        private int index;
        public void SetIndex(int sel)
        {
            index = sel;
        }
        
        private void OnMouseUp()
        {
            EventBus.OnDropSlotSelected?.Invoke(index);
        }
    }
}