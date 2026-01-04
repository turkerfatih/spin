using UnityEngine;

namespace Game.UIComponent
{
    public class LeverRaycast:MonoBehaviour
    {
        private PhysicalLever _currentButton;

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSomething = Physics.Raycast(ray, out RaycastHit hit);

            // Detect Mouse Down
            if (Input.GetMouseButtonDown(0) && hitSomething)
            {
                if (hit.collider.TryGetComponent(out _currentButton))
                {
                    _currentButton.PointerDown();
                }
            }

            // Detect Mouse Up
            if (Input.GetMouseButtonUp(0) && _currentButton != null)
            {
                _currentButton.PointerUp();
                _currentButton = null;
            }
        }
    }
}