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
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}