using System;
using UnityEngine;

namespace Game.Core.UI
{
    public class HoverHandler : MonoBehaviour
    {
        [SerializeField] private Camera uiCamera;
        [SerializeField] private LayerMask uiLayerMask = ~0; // default: all layers

        private ISelectable hoveredSelectable;
        private InputDeviceType activeDevice=InputDeviceType.MouseKeyboard;

        private void Awake()
        {
            if (uiCamera == null)
                uiCamera = Camera.main;
        }

        private void OnEnable()
        {
            InputManager.Instance.OnDeviceChanged += OnDeviceChanged;
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.OnDeviceChanged -= OnDeviceChanged;
        }

        private void OnDeviceChanged(InputDeviceType device)
        {
            Debug.Log($"OnDeviceChanged for hover handler: {device}");
            activeDevice = device;
        }

        private void Update()
        {
            if(activeDevice== InputDeviceType.None)
                return;
            // Only handle hover when using mouse
            if (activeDevice != InputDeviceType.MouseKeyboard)
                return;

            var input = InputManager.Instance;
            Vector2 mousePos = input.GetPointerPosition();
            Ray ray = uiCamera.ScreenPointToRay(mousePos);
            //Debug.DrawRay(uiCamera.transform.position, Vector3.forward * 100, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, uiLayerMask))
            {
                //Debug.Log("Hit:"+hit.transform.gameObject.name);
                if (hit.collider.TryGetComponent<ISelectable>(out var selectable))
                {
                    if (selectable != hoveredSelectable)
                    {
                        hoveredSelectable = selectable;
                        FocusManager.Instance.SetFocus(hoveredSelectable, fromMouse: true);
                    }
                    if (input.ClickPressed())
                        FocusManager.Instance.Submit();
                }
            }
            else
            {
                hoveredSelectable = null;
            }
        }
    }
}