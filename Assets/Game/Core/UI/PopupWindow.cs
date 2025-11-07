using UnityEngine;

namespace Game.Core.UI
{
    public class PopupWindow:MonoBehaviour
    {
        public void Show()
        {
            //todo:we should use window manager
            if (!UIOverlay.Instance.IsOpen)
            {
                UIOverlay.Instance.Show();
            }
        }

        public void Hide()
        {
            
        }
    }
}