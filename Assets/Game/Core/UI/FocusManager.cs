using System;
using UnityEngine;

namespace Game.Core.UI
{
    public class FocusManager:MonoBehaviour
    {
        public static FocusManager Instance { get; private set; }
        public ISelectable Current { get; private set; }
        private InputDeviceType activeDevice;
        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            activeDevice = InputManager.Instance.ActiveDevice;
        }

        private void OnEnable()
        {
            InputManager.Instance.OnDeviceChanged += OnDeviceChanged;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnDeviceChanged -= OnDeviceChanged;
        }

        void OnDestroy()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDeviceChanged -= OnDeviceChanged;
        }

        private void OnDeviceChanged(InputDeviceType device)
        {
            activeDevice = device;
            Cursor.visible = device == InputDeviceType.MouseKeyboard;
        }
        public void SetFocus(ISelectable target, bool fromMouse = false)
        {
            // Only allow mouse focus if mouse is active
            if (fromMouse && activeDevice != InputDeviceType.MouseKeyboard)
            {
                //Debug.Log("SetFocus cancelled");
                return;
            }

            if (Current == target)
            {
                //Debug.Log("SetFocus cancelled for same");
                return;
            }

            Current?.OnDeselected();
            Current = target;
            Current?.OnSelected();
        }
        public void Navigate(Vector2 dir)
        {
            if (Current == null) return;
            var nav = Current.Navigation;

            ISelectable next = null;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                next = dir.x > 0 ? nav.Right : nav.Left;
            else
                next = dir.y > 0 ? nav.Up : nav.Down;

            if (next != null)
                SetFocus(next);
        }
        void Update()
        {
            var input = InputManager.Instance;

            if (input.ActiveDevice == InputDeviceType.Gamepad)
            {
                Vector2 nav = input.GetNavigate();
                if (nav != Vector2.zero)
                    Instance.Navigate(nav);

                if (input.SubmitPressed())
                    Instance.Submit();
            }
        }
        public void Submit()
        {
            if (Current == null)
                return;

            Current.OnSubmit();
        }

        // Optional, for Esc/B button behavior
        public void Cancel()
        {
            // Could close popup, go back, etc.
        }
    }
}