using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core.UI
{
    public class InputManager:MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public enum InputDeviceType { MouseKeyboard, Gamepad, Touch }

        public InputDeviceType ActiveDevice { get; private set; }

        private PlayerInput playerInput;
        public event Action<InputDeviceType> OnDeviceChanged;
        
        void Awake()
        {
            Instance=this;
            playerInput = GetComponent<PlayerInput>();
            playerInput.onControlsChanged += HandleControlsChanged;
        }
        
        private void HandleControlsChanged(PlayerInput input)
        {
            var scheme = input.currentControlScheme;
            var newDevice = scheme.Contains("Gamepad") ? InputDeviceType.Gamepad :
                scheme.Contains("Keyboard") ? InputDeviceType.MouseKeyboard :
                InputDeviceType.Touch;

            if (newDevice != ActiveDevice)
            {
                ActiveDevice = newDevice;
                OnDeviceChanged?.Invoke(ActiveDevice);
            }
        }

        public Vector2 GetNavigate()
        {
            throw new NotImplementedException();
        }
    }
}