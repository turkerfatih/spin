using UnityEngine;

namespace Game.UIComponent
{
    public class PhysicalRaycaster : MonoBehaviour
    {
        [Header("Settings")]
        public LayerMask interactableLayer;
        public float dragSensitivity = 0.5f;

        private PhysicalButton _currentButton;
        private PhysicalLever _currentLever;
        
        private Vector3 _lastMousePosition;
        private float _currentDragAngle;

        void Update()
        {
            HandleInteraction();
        }

        private void HandleInteraction()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, 100f, interactableLayer);

            // 1. MOUSE DOWN: Identify what we hit
            if (Input.GetMouseButtonDown(0) && hitSomething)
            {
                _lastMousePosition = Input.mousePosition;

                // Handle Button
                if (hit.collider.TryGetComponent(out _currentButton))
                {
                    _currentButton.PointerDown();
                }
                // Handle Lever
                else if (hit.collider.TryGetComponent(out _currentLever))
                {
                    _currentLever.PointerDown();
                    _currentDragAngle = 0; 
                }
            }

            // 2. MOUSE HELD: Handle Dragging for the Lever
            if (Input.GetMouseButton(0))
            {
                if (_currentLever != null)
                {
                    // Calculate vertical mouse movement
                    float mouseDeltaY = _lastMousePosition.y - Input.mousePosition.y;
                    
                    // Update the drag angle based on movement
                    _currentDragAngle += mouseDeltaY * dragSensitivity;
                    _currentDragAngle = Mathf.Clamp(_currentDragAngle, 0, _currentLever.maxPullAngle);
                    
                    // Feed the angle into the Lever's spring
                    _currentLever.UpdateDragAngle(_currentDragAngle);
                    
                    _lastMousePosition = Input.mousePosition;
                }
            }

            // 3. MOUSE UP: Release whatever we were holding
            if (Input.GetMouseButtonUp(0))
            {
                if (_currentButton != null)
                {
                    _currentButton.PointerUp();
                    _currentButton = null;
                }

                if (_currentLever != null)
                {
                    _currentLever.PointerUp();
                    _currentLever = null;
                }
            }
        }
    }
}