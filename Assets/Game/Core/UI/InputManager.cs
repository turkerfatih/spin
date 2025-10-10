using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core.UI
{
    public partial class InputManager:MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public InputDeviceType ActiveDevice { get; private set; }

        private PlayerInput playerInput;
        private InputSystemActions controls;
        public event Action<InputDeviceType> OnDeviceChanged;
        
        void Awake()
        {
            Instance=this;
            playerInput = GetComponent<PlayerInput>();
            controls=new InputSystemActions();
            controls.Enable();
            DetectDevice(playerInput.currentControlScheme);
        }

        private void OnEnable()
        {
            playerInput.onControlsChanged += HandleControlsChanged;
        }

        private void OnDisable()
        {
            playerInput.onControlsChanged -= HandleControlsChanged;
        }


        private void HandleControlsChanged(PlayerInput input)
        {
            Debug.Log("HandleControlsChanged:"+input.currentControlScheme);
            DetectDevice(input.currentControlScheme);
        }

        private void DetectDevice(string scheme)
        {
            var newDevice =
                scheme.Contains("Gamepad") ? InputDeviceType.Gamepad :
                scheme.Contains("Keyboard") ? InputDeviceType.MouseKeyboard :
                InputDeviceType.Touch;

            if (newDevice != ActiveDevice)
            {
                Debug.Log("Device change "+ActiveDevice + " to "+newDevice);
                ActiveDevice = newDevice;
                OnDeviceChanged?.Invoke(ActiveDevice);
            }
        }
        


        public Vector2 GetNavigate() => controls.UI.Navigate.ReadValue<Vector2>();

        public bool SubmitPressed() => controls.UI.Submit.WasPerformedThisFrame();

        public bool CancelPressed() => controls.UI.Cancel.WasPerformedThisFrame();

        public Vector2 GetPointerPosition() => controls.UI.Point.ReadValue<Vector2>();

        public bool ClickPressed() => controls.UI.Click.WasReleasedThisFrame();
    }
}