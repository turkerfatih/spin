using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI
{
    public class WindowManager:MonoBehaviour
    {
        public static WindowManager Instance { get; private set; }
        private Stack<IWindow> windows = new Stack<IWindow>();

        private void Awake()
        {
            Instance = this;
        }

        public void Show(IWindow window)
        {
            if (windows.Count > 0)
                windows.Peek().SetActive(false);
            windows.Push(window);
            if (!UIOverlay.Instance.IsOpen)
            {
                UIOverlay.Instance.Show(window.HasCustomClose);
            }

            window.SetActive(true);
        }

        public void Hide()
        {
            if (windows.Count == 0) return;
            var window = windows.Pop();
            window.SetActive(false);
            if (windows.Count > 0)
                windows.Peek().SetActive(true);
            else
            {
                if (UIOverlay.Instance.IsOpen)
                {
                    UIOverlay.Instance.Hide();
                }
            }
        }

        public IWindow Current => windows.Count > 0 ? windows.Peek() : null;
    }
}