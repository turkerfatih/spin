using UnityEngine;

namespace Game.Core.Animation
{
    public class FloatSpring : BaseSpring<float,float>
    {
        public override float Evaluate(float deltaTime)
        {
            Springs.CalcDampedSimpleHarmonicMotion(ref CurrentValue, ref CurrentVelocity, EndValue, deltaTime, AngularFrequency, DampingRatio);
            return CurrentValue;
        }

        public override void Evaluate(DampedSpringMotionParams cachedParams)
        {
            // Using the internal 'Update' logic from your library directly
            float oldPos = CurrentValue - EndValue;
            float oldVel = CurrentVelocity;

            CurrentValue = oldPos * cachedParams.posPosCoef + oldVel * cachedParams.posVelCoef + EndValue;
            CurrentVelocity = oldPos * cachedParams.velPosCoef + oldVel * cachedParams.velVelCoef;
        }
        
        public override bool IsSettled(float precision=Springs.Epsilon)
        {
            return Mathf.Abs(CurrentValue - EndValue) < precision && 
                   Mathf.Abs(CurrentVelocity) < precision;
        }

        public override void Nudge(float amount)
        {
            CurrentVelocity += amount;
        }
    }
}