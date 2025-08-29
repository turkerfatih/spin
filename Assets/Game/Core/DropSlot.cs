using System;
using DG.Tweening;
using Game.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class DropSlot:MonoBehaviour
    {
        private int index;
        private CardView current;
        public void SetIndex(int sel)
        {
            index = sel;
        }
        
        private void OnMouseUp()
        {
            EventBus.OnDropSlotSelected?.Invoke(index);
        }

        public void SetCard(CardView card)
        {
            current=card;
            card.transform.DOMove(transform.position, 0.15f);
        }

    }
}