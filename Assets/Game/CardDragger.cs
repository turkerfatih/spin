using System;
using DG.Tweening;
using Game.Core;
using Game.Event;
using UnityEngine;

namespace Game
{
    public class CardDragger:MonoBehaviour
    {
        private Vector3 offset;
        private CardView card;
        private bool dragging;
        [SerializeField] private float SmoothTime = 1;
        private Vector3 velocity;

        private void OnEnable()
        {
            EventBus.OnDropSlotSelected += DropSlotSelected;
        }

        private void OnDisable()
        {
            EventBus.OnDropSlotSelected -= DropSlotSelected;
        }

        public void StartDragging(CardView targetCard, Vector3 dragOffset)
        {
            velocity=Vector3.zero;
            card = targetCard;
            card.SetOrder(10000);
            enabled = true;

            offset = dragOffset;
            dragging = true;
            DOVirtual.Int(0, 1, 0.15f, (t) =>
            {
                offset=Vector3.Lerp(offset, Vector3.zero, t);
            });
        }

        private void Update()
        {
            if(!dragging)return;
            if (Input.GetMouseButtonUp(1))
            {
                CancelDragging();
                return;
            }
            
            Vector3 mousePos = Input.mousePosition;
            var cam = Services.MainCamera;
            mousePos.z = Mathf.Abs(cam.transform.position.z - card.transform.position.z);
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);
            mousePos.z = card.transform.position.z;
            var targetPos = mouseWorldPos + offset;
            //card.transform.position=Vector3.Lerp(card.transform.position,targetPos,Time.deltaTime*SmoothTime);
            card.transform.position = Vector3.SmoothDamp(card.transform.position, targetPos, ref velocity, 1f / SmoothTime);
        }

        private void CancelDragging()
        {
            EventBus.OnDragCancel?.Invoke(card);
            enabled = false;
        }

        private void DropSlotSelected(int index)
        {
            enabled = false;
            EventBus.OnCardDroppedToSlot?.Invoke(card, index);
        }
    }
}