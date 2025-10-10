using UnityEngine;

namespace Game.Core.UI
{
    public class Routing:MonoBehaviour
    {
        void OnEnable()
        {
            InputManager.Instance.OnDeviceChanged += OnDeviceChange;
        }

        void Update()
        {
            var input = InputManager.Instance;
            if (input.ActiveDevice == InputManager.InputDeviceType.Gamepad)
            {
                Vector2 nav = input.GetNavigate();
                if (nav != Vector2.zero)
                    FocusManager.Instance.Navigate(nav);
            }
        }

        void OnDeviceChange(InputManager.InputDeviceType type)
        {
            Cursor.visible = type == InputManager.InputDeviceType.MouseKeyboard;
        }
    }
}