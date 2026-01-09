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
        public const float Width=3.906f;
        private int index;
        private CardView current;
        [CanBeNull] public Card GetCard => current?.Model;
        public int Index => index;

        private SpriteRenderer renderer;
        private void Awake()
        {
            renderer=transform.GetChild(0).GetComponent<SpriteRenderer>();
            renderer.enabled = false;
            //renderer.color=new Color(0,0,0,0);
        }

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
          
            current.DiscardAnimation();
            current=null;
        }


    }
}