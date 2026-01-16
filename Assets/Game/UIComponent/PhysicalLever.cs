using Cysharp.Threading.Tasks;
using Game.Actions;
using UnityEngine;
using Game.Core.Animation;

namespace Game.UIComponent
{
    public class PhysicalLever : MonoBehaviour
    {
        public enum LeverState { Idle, Pulling, Actuated, Releasing }
        private LeverState currentState = LeverState.Idle;

        [Header("Pivot Reference")]
        [SerializeField] private Transform leverPivot; // The part that actually rotates

        [Header("Angle Settings")]
        public float maxPullAngle = 70f;
        public float actuationAngle = 50f;

        [Header("Spring Profiles")]
        public float pullFreq = 25f;
        public float pullDamping = 5f;
        public float releaseFreq = 25f;
        public float releaseDamping = 0.3f; // Bouncy release!

        private SpringHandle<float, FloatSpring,float> rotationHandle;

        private void Awake()
        {
            // Initialize spring at 0 angle
            rotationHandle = new SpringHandle<float, FloatSpring,float>(this, new FloatSpring(), UpdateLeverRotation);
            
            rotationHandle.OnSettled += () => {
                if (currentState == LeverState.Releasing) currentState = LeverState.Idle;
            };
        }

        private void UpdateLeverRotation(float currentAngle)
        {
            // Apply rotation to the pivot
            leverPivot.localRotation = Quaternion.Euler(currentAngle, 0, 0);

            // Logic: Trigger actuation
            if (currentState == LeverState.Pulling && currentAngle >= actuationAngle)
            {
                currentState = LeverState.Actuated;
            }
        }

        public void PointerDown()
        {
            if(currentState==LeverState.Releasing)
                return;
            if(!Services.Machine.CanSpin())
                return;
            currentState = LeverState.Pulling;
            rotationHandle.Spring.AngularFrequency = pullFreq;
            rotationHandle.Spring.DampingRatio = pullDamping;
            
            // Move toward the max angle
            rotationHandle.Play(maxPullAngle).Forget();
        }

        public void PointerUp()
        {
            if (currentState == LeverState.Actuated)
            {
                OnLeverPulled();
            }

            currentState = LeverState.Releasing;
            
            // Snap back to 0
            rotationHandle.Spring.AngularFrequency = releaseFreq;
            rotationHandle.Spring.DampingRatio = releaseDamping;
            rotationHandle.Play(0f).Forget();
        }

        private void OnLeverPulled()
        {
            if(!Services.Machine.CanSpin())
                return; 
            Services.RoundStack.Spin().Forget();
        }
        public void UpdateDragAngle(float targetAngle)
        {
            // We don't change the state here (it's already Pulling)
            // We just tell the spring to follow the new target
            rotationHandle.Play(targetAngle).Forget();
        }
    }
}