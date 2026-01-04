namespace Game.Core.Animation
{
    public class Vector3Spring : BaseSpring<UnityEngine.Vector3>
    {
        public override UnityEngine.Vector3 Evaluate(float deltaTime)
        {
            Springs.CalcDampedSimpleHarmonicMotion(ref CurrentValue, ref CurrentVelocity, EndValue, deltaTime, AngularFrequency, DampingRatio);
            return CurrentValue;
        }

        public override void Evaluate(DampedSpringMotionParams cachedParams)
        {
            // Manual per-axis update using the shared params
            UpdateAxis(ref CurrentValue.x, ref CurrentVelocity.x, EndValue.x, cachedParams);
            UpdateAxis(ref CurrentValue.y, ref CurrentVelocity.y, EndValue.y, cachedParams);
            UpdateAxis(ref CurrentValue.z, ref CurrentVelocity.z, EndValue.z, cachedParams);
        }

        private void UpdateAxis(ref float p, ref float v, float target, DampedSpringMotionParams prm)
        {
            float oldPos = p - target;
            p = oldPos * prm.posPosCoef + v * prm.posVelCoef + target;
            v = oldPos * prm.velPosCoef + v * prm.velVelCoef;
        }
        
        // ReSharper disable once OptionalParameterHierarchyMismatch
        public override bool IsSettled(float precision=Springs.EpsilonExp2)
        {
            float sqrPrecision = precision;
            bool isPosSettled = (CurrentValue - EndValue).sqrMagnitude < sqrPrecision;
            bool isVelSettled = CurrentVelocity.sqrMagnitude < sqrPrecision;
            return isPosSettled && isVelSettled;
        }
    }
}