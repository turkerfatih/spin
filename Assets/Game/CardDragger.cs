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
        private Card card;
        private bool dragging = false;
        [SerializeField] private float SmoothTime = 1;


        private void OnEnable()
        {
            EventBus.OnDropSlotSelected += DropSlotSelected;
        }

        private void OnDisable()
        {
            EventBus.OnDropSlotSelected -= DropSlotSelected;
        }

        public void StartDragging(Card targetCard)
        {
            card = targetCard;
            card.SetOrder(10000);
            enabled = true;
            Vector3 mousePos = Services.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            offset = card.transform.position - new Vector3(mousePos.x, mousePos.y, card.transform.position.z);
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

            Vector3 mousePos = Services.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var targetPos = new Vector3(mousePos.x, mousePos.y, card.transform.position.z) + offset;
            card.transform.position=Vector3.Lerp(card.transform.position,targetPos,Time.deltaTime*SmoothTime);
            
            /*/Vector3 localMouse = card.transform.InverseTransformPoint(
                Services.MainCamera.ScreenToWorldPoint(Input.mousePosition)
            );
            card.UpdateRotate(localMouse);*/
        }

        private void CancelDragging()
        {
            EventBus.OnDragCancel?.Invoke(card);
            enabled = false;
        }

        private void DropSlotSelected(int index)
        {
            enabled = false;
        }
    }
}