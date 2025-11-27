using System;
using DG.Tweening;
using Game.Actions;
using Game.Event;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class SpinButton:MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
    {
        Vector3 baseScale;
        [SerializeField]
        private TextMeshPro label;
        

        private void Awake()
        {
            baseScale = transform.localScale;
        }

        private void OnEnable()
        {
            EventBus.OnSpinCountChanged+= OnSpinCountChanged;
        }

        private void OnDisable()
        {
            EventBus.OnSpinCountChanged-= OnSpinCountChanged;
        }
        private void OnSpinCountChanged(int count)
        {
            if (count <= 0)
            {
                gameObject.SetActive(false);
                return;
            }

            label.SetText(count.ToString());
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