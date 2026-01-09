using Cysharp.Threading.Tasks;
using Game.Actions;
using UnityEngine;
using Game.Core.Animation;


namespace Game.UIComponent
{
    public class PhysicalButton : MonoBehaviour
    {
        public enum ButtonState { Idle, Pressing, Pressed, Releasing }
        private ButtonState currentState = ButtonState.Idle;
        
        [Header("Press Profile (Heavy/Resistant)")]
        public float pressFreq = 15f; 
        public float pressDamping = 0.8f; 

        [Header("Release Profile (Snappy/Quick)")]
        public float releaseFreq = 30f;
        public float releaseDamping = 0.4f;
        
        [Header("Down Scale Profile")]
        public float downScale = 0.8f;
        public float downScaleFreq = 5f;
        public float downScaleDamping = 1f;
        [Header("Up Scale Profile")]
        public float upScale = 1f;
        public float upScaleFreq = 10f;
        public float upScaleDamping = 0.5f;
        
        
        [Header("Movement Settings")]
        public float pressDepth = 0.7f;      // How far the cube moves down
        public float clickThreshold = 0.8f;  // 0 to 1 percentage of depth to trigger click
        

        private SpringHandle posHandle;
        private SpringHandle scaleHandle;
        private Vector3 startLocalPos;

        public Transform frame;

        private void Awake()
        {
            startLocalPos = transform.localPosition;
            posHandle = new SpringHandle(this, new FloatSpring(), UpdateVisualPosition);
            var scaleSpring = new FloatSpring(){CurrentValue = upScale,CurrentVelocity = 0};
            scaleHandle = new SpringHandle(this,scaleSpring , UpdateScale);
            posHandle.OnSettled += () => {
                if (currentState == ButtonState.Releasing) 
                    currentState = ButtonState.Idle;
            };
        }
        
        private void UpdateVisualPosition(float t)
        {
            // 1. Visual Update (Linear movement along local Y)
            transform.localPosition = startLocalPos + (Vector3.down * (t * pressDepth));

            // 2. State Management (Logic)
            if (currentState== ButtonState.Pressing && t >= clickThreshold)
            {
                currentState=ButtonState.Pressed;
                Debug.Log("Button Primed (Threshold Reached)");
                //scaleHandle.Spring.Nudge(5f);
            }
        }

        private void UpdateScale(float t)
        {
            float squash = t;
            float stretch = 1f / Mathf.Max(t, 0.1f); // Volume preservation: if it gets shorter, it gets wider
            
            var val= new Vector3(
                 stretch, 
                  squash, 
                 stretch
            );
            transform.localScale = val;
            frame.localScale = val;

        }

        // Call these from your Raycast script
        public void PointerDown()
        {
            if(currentState== ButtonState.Releasing)
                return;
  
            currentState= ButtonState.Pressing;
            posHandle.Spring.AngularFrequency = pressFreq;
            posHandle.Spring.DampingRatio = pressDamping;
            posHandle.Play(1f).Forget(); // Go to compressed state
            scaleHandle.Spring.AngularFrequency = downScaleFreq;
            scaleHandle.Spring.DampingRatio = downScaleDamping;
            scaleHandle.Play(downScale).Forget();
        }

        public void PointerUp()
        {
            if (currentState == ButtonState.Pressed) 
                OnSuccessfulClick();
            currentState = ButtonState.Releasing;
            posHandle.Spring.AngularFrequency = releaseFreq;
            posHandle.Spring.DampingRatio = releaseDamping;
            posHandle.Play(0f).Forget(); // Return to rest
            scaleHandle.Spring.AngularFrequency = upScaleFreq;
            scaleHandle.Spring.DampingRatio = upScaleDamping;
            scaleHandle.Play(upScale).Forget();
        
        }
        private void OnSuccessfulClick()
        {
            Services.Round.Draw();
        }

    }
}