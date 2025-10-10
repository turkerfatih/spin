using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI
{
    public class WindowManager:MonoBehaviour
    {
        private Stack<IWindow> windows = new Stack<IWindow>();
        public void Push(IWindow window)
        {
            if (windows.Count > 0)
                windows.Peek().SetActive(false);
            windows.Push(window);
            window.SetActive(true);
        }

        public void Pop()
        {
            if (windows.Count == 0) return;
            var window = windows.Pop();
            window.SetActive(false);
            if (windows.Count > 0)
                windows.Peek().SetActive(true);
        }

        public IWindow Current => windows.Count > 0 ? windows.Peek() : null;
    }
}