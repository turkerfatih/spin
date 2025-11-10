using System;
using UnityEngine;

namespace Game.Core.UI
{
    public class PopupWindow:MonoBehaviour,IWindow
    {
        protected int SortingLayerId;

        protected virtual void Awake()
        {
            SortingLayerId= SortingLayer.NameToID("Popup");
        }

        public void Show()
        {
            WindowManager.Instance.Show(this);
        }

        public void Hide()
        {
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}