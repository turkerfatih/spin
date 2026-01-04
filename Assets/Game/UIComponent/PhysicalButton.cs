using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Game.Core.Animation;
using Unity.VisualScripting;

namespace Game.UIComponent
{
    public class PhysicalButton : MonoBehaviour
    {
        public enum ButtonState { Idle, Pressing, Pressed, Releasing }
        private ButtonState _currentState = ButtonState.Idle;
        
        [Header("Press Profile (Heavy/Resistant)")]
        public float pressFreq = 15f; 
        public float pressDamping = 0.8f; 

        [Header("Release Profile (Snappy/Quick)")]
        public float releaseFreq = 30f;
        public float releaseDamping = 0.4f;
        
        [Header("Movement Settings")]
        public float pressDepth = 0.7f;      // How far the cube moves down
        public float clickThreshold = 0.8f;  // 0 to 1 percentage of depth to trigger click
        

        private SpringHandle<float, FloatSpring> _springHandle;
        private Vector3 _startLocalPos;


        private void Awake()
        {
            _startLocalPos = transform.localPosition;

            // Initialize handle with the named method
            var spring = new FloatSpring();
            _springHandle = new SpringHandle<float, FloatSpring>(this, spring, UpdateVisualPosition);
        }
        
        private void UpdateVisualPosition(float t)
        {
            // 1. Visual Update (Linear movement along local Y)
            transform.localPosition = _startLocalPos + (Vector3.down * (t * pressDepth));

            // 2. State Management (Logic)
            if (_currentState== ButtonState.Pressing && t >= clickThreshold)
            {
                _currentState=ButtonState.Pressed;
                Debug.Log("Button Primed (Threshold Reached)");
            }
        }

        // Call these from your Raycast script
        public void PointerDown()
        {
            _currentState= ButtonState.Pressing;
            _springHandle.Spring.AngularFrequency = pressFreq;
            _springHandle.Spring.DampingRatio = pressDamping;
            _springHandle.Play(1f).Forget(); // Go to compressed state
        }

        public void PointerUp()
        {
            if (_currentState == ButtonState.Pressed) 
                OnSuccessfulClick();
            _currentState = ButtonState.Releasing;
            _springHandle.Spring.AngularFrequency = releaseFreq;
            _springHandle.Spring.DampingRatio = releaseDamping;
            _springHandle.Play(0f).Forget(); // Return to rest
        
        }
        private void OnSuccessfulClick()
        {
            Debug.Log("Success! Button pressed and released.");
            // Play sound or trigger logic here
        }
    }
}