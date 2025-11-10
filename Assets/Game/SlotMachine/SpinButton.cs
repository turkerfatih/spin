using System;
using DG.Tweening;
using Game.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class SpinButton:MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {
        Vector3 baseScale;

        private void Awake()
        {
            baseScale = transform.localScale;
        }
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(baseScale * 1.1f,0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(baseScale,0.1f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(!Services.Machine.CanSpin())
                return;
            Services.Actions.Add(new SpinAction());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(baseScale*0.9f,0.1f).SetEase(Ease.OutSine);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(baseScale*1.1f,0.1f).SetEase(Ease.InBounce);
        }
    }
}