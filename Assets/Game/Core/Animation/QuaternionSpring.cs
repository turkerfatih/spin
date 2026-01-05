using UnityEngine;

namespace Game.Core.Animation
{
    public class QuaternionSpring : BaseSpring<Quaternion,Vector3>
    {
        // Overriding velocity type because Quaternion velocity is a 3D vector (angular velocity)
        public new Vector3 CurrentVelocity;

        public override void Reset(Quaternion value)
        {
            base.Reset(value);
            CurrentVelocity = Vector3.zero;
        }

        public override Quaternion Evaluate(float deltaTime)
        {
            var motionParams = Springs.CalcDampedSpringMotionParams(deltaTime, AngularFrequency, DampingRatio);
            Evaluate(motionParams);
            return CurrentValue;
        }

        public override void Evaluate(DampedSpringMotionParams cachedParams)
        {
            Springs.UpdateDampedSpringMotionQuaternion(
                ref CurrentValue, 
                ref CurrentVelocity, 
                EndValue, 
                cachedParams
            );
        }
        
        public override bool IsSettled(float precision=Springs.Epsilon)
        {
            // Quaternion.Dot returns 1.0 if rotations are identical
            // We check if it is very close to 1.0
            float dot = Quaternion.Dot(CurrentValue, EndValue);
            bool isRotationSettled = Mathf.Abs(dot) > (1.0f - precision);
    
            // Velocity for Quaternion is a Vector3 (angular velocity)
            bool isVelSettled = CurrentVelocity.sqrMagnitude < (precision * precision);
    
            return isRotationSettled && isVelSettled;
        }

        public override void Nudge(Vector3 amount)
        {
            CurrentVelocity += amount;
        }
    }
}