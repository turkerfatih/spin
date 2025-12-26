using System;
using UnityEngine;

namespace Game.Core.UI
{
    public class PopupWindow:MonoBehaviour,IWindow
    {

        public void Show()
        {
            WindowManager.Instance.Show(this);
        }

        public void Hide()
        {
            WindowManager.Instance.Hide();
        }

        public virtual bool HasCustomClose => false;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}