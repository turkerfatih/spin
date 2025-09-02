using System;
using DG.Tweening;
using Game.Event;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core
{
    public class DropSlot:MonoBehaviour
    {
        private int index;
        private CardView current;
        [CanBeNull] public Card GetCard => current?.Model;
        public void SetIndex(int sel)
        {
            index = sel;
        }
        
        private void OnMouseUp()
        {
            EventBus.OnDropSlotSelected?.Invoke(index);
        }

        public void SetCard(CardView cardView)
        {
            current=cardView;
            cardView.transform.DOMove(transform.position, 0.15f);
            foreach (var cardEffect in cardView.Model.Definition.Effects)
            {
                cardEffect.OnPlay(cardView.Model,index);
            }
        }
        public void ClearCard()
        {
            current=null;
        }


    }
}