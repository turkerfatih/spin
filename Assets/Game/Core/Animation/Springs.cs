using System.Runtime.CompilerServices;
using UnityEngine;
//ref:Ryan Juckett's Code for Damped Springs (https://www.ryanjuckett.com/damped-springs/
namespace Game.Core.Animation
{
    //******************************************************************************
    // Cached set of motion parameters that can be used to efficiently update
    // multiple springs using the same time step, angular frequency and damping
    // ratio.
    //******************************************************************************
    public struct DampedSpringMotionParams
    {
        // newPos = posPosCoef*oldPos + posVelCoef*oldVel
        public float posPosCoef, posVelCoef;

        // newVel = velPosCoef*oldPos + velVelCoef*oldVel
        public float velPosCoef, velVelCoef;
    };

    public static class Springs
    {
        public const float Epsilon = 0.0001f;
        public const float EpsilonExp2 = 0.0001f*0.0001f;
        //******************************************************************************
        // This function will compute the parameters needed to simulate a damped spring
        // over a given period of time.
        // - An angular frequency is given to control how fast the spring oscillates.
        // - A damping ratio is given to control how fast the motion decays.
        //     damping ratio > 1: over damped
        //     damping ratio = 1: critically damped
        //     damping ratio < 1: under damped
        //******************************************************************************
        public static DampedSpringMotionParams CalcDampedSpringMotionParams(
            float deltaTime, // time step to advance
            float angularFrequency, // angular frequency of motion
            float dampingRatio) // damping ratio of motion
        {
           
            DampedSpringMotionParams pOutParams;

            // force values into legal range
            if (dampingRatio < 0.0f) dampingRatio = 0.0f;
            if (angularFrequency < 0.0f) angularFrequency = 0.0f;

            // if there is no angular frequency, the spring will not move and we can
            // return identity
            if (angularFrequency < Epsilon)
            {
                pOutParams.posPosCoef = 1.0f;
                pOutParams.posVelCoef = 0.0f;
                pOutParams.velPosCoef = 0.0f;
                pOutParams.velVelCoef = 1.0f;
                return pOutParams;
            }

            if (dampingRatio > 1.0f + Epsilon)
            {
                // over-damped
                float za = -angularFrequency * dampingRatio;
                float zb = angularFrequency * Mathf.Sqrt(dampingRatio * dampingRatio - 1.0f);
                float z1 = za - zb;
                float z2 = za + zb;
                // Value e (2.7) raised to a specific power
                float e1 = Mathf.Exp(z1 * deltaTime);
                float e2 = Mathf.Exp(z2 * deltaTime);

                float invTwoZb = 1.0f / (2.0f * zb); // = 1 / (z2 - z1)

                float e1_Over_TwoZb = e1 * invTwoZb;
                float e2_Over_TwoZb = e2 * invTwoZb;

                float z1e1_Over_TwoZb = z1 * e1_Over_TwoZb;
                float z2e2_Over_TwoZb = z2 * e2_Over_TwoZb;

                pOutParams.posPosCoef = e1_Over_TwoZb * z2 - z2e2_Over_TwoZb + e2;
                pOutParams.posVelCoef = -e1_Over_TwoZb + e2_Over_TwoZb;

                pOutParams.velPosCoef = (z1e1_Over_TwoZb - z2e2_Over_TwoZb + e2) * z2;
                pOutParams.velVelCoef = -z1e1_Over_TwoZb + z2e2_Over_TwoZb;
            }
            else if (dampingRatio < 1.0f - Epsilon)
            {
                // under-damped
                float omegaZeta = angularFrequency * dampingRatio;
                float alpha = angularFrequency * Mathf.Sqrt(1.0f - dampingRatio * dampingRatio);

                float expTerm = Mathf.Exp(-omegaZeta * deltaTime);
                float cosTerm = Mathf.Cos(alpha * deltaTime);
                float sinTerm = Mathf.Sin(alpha * deltaTime);

                float invAlpha = 1.0f / alpha;

                float expSin = expTerm * sinTerm;
                float expCos = expTerm * cosTerm;
                float expOmegaZetaSin_Over_Alpha = expTerm * omegaZeta * sinTerm * invAlpha;

                pOutParams.posPosCoef = expCos + expOmegaZetaSin_Over_Alpha;
                pOutParams.posVelCoef = expSin * invAlpha;

                pOutParams.velPosCoef = -expSin * alpha - omegaZeta * expOmegaZetaSin_Over_Alpha;
                pOutParams.velVelCoef = expCos - expOmegaZetaSin_Over_Alpha;
            }
            else
            {
                // critically damped
                float expTerm = Mathf.Exp(-angularFrequency * deltaTime);
                float timeExp = deltaTime * expTerm;
                float timeExpFreq = timeExp * angularFrequency;

                pOutParams.posPosCoef = timeExpFreq + expTerm;
                pOutParams.posVelCoef = timeExp;

                pOutParams.velPosCoef = -angularFrequency * timeExpFreq;
                pOutParams.velVelCoef = -timeExpFreq + expTerm;
            }

            return pOutParams;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Approximately(float value,float other)
        {
            return Mathf.Abs(value - other) < Epsilon;
        }

        //******************************************************************************
        // This function will update the supplied position and velocity values over
        // according to the motion parameters.
        //******************************************************************************
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateDampedSpringMotion(
            ref float pPos, // position value to update
            ref float pVel, // velocity value to update
            float equilibriumPos, // position to approach
            DampedSpringMotionParams parameters) // motion parameters to use
        {
            float oldPos = pPos - equilibriumPos; // update in equilibrium relative space
            float oldVel = pVel;

            pPos = oldPos * parameters.posPosCoef + oldVel * parameters.posVelCoef + equilibriumPos;
            pVel = oldPos * parameters.velPosCoef + oldVel * parameters.velVelCoef;
        }

        /// <summary>
        /// Calculate a spring motion development for a given deltaTime
        /// </summary>
        /// <param name="position">"Live" position value</param>
        /// <param name="velocity">"Live" velocity value</param>
        /// <param name="equilibriumPosition">Goal (or rest) position</param>
        /// <param name="deltaTime">Time to update over</param>
        /// <param name="angularFrequency">Angular frequency of motion</param>
        /// <param name="dampingRatio">Damping ratio of motion</param>
        public static void CalcDampedSimpleHarmonicMotion(ref float position, ref float velocity,
            float equilibriumPosition, float deltaTime, float angularFrequency, float dampingRatio)
        {
            var motionParams = CalcDampedSpringMotionParams(deltaTime, angularFrequency, dampingRatio);
            UpdateDampedSpringMotion(ref position, ref velocity, equilibriumPosition, motionParams);
        }

        /// <summary>
        /// Calculate a spring motion development for a given deltaTime
        /// </summary>
        /// <param name="position">"Live" position value</param>
        /// <param name="velocity">"Live" velocity value</param>
        /// <param name="equilibriumPosition">Goal (or rest) position</param>
        /// <param name="deltaTime">Time to update over</param>
        /// <param name="angularFrequency">Angular frequency of motion</param>
        /// <param name="dampingRatio">Damping ratio of motion</param>
        public static void CalcDampedSimpleHarmonicMotion(ref Vector2 position, ref Vector2 velocity,
            Vector2 equilibriumPosition, float deltaTime, float angularFrequency, float dampingRatio)
        {
            var motionParams = CalcDampedSpringMotionParams(deltaTime, angularFrequency, dampingRatio);
            UpdateDampedSpringMotion(ref position.x, ref velocity.x, equilibriumPosition.x, motionParams);
            UpdateDampedSpringMotion(ref position.y, ref velocity.y, equilibriumPosition.y, motionParams);
        }

        /// <summary>
        /// Calculate a spring motion development for a given deltaTime
        /// </summary>
        /// <param name="position">"Live" position value</param>
        /// <param name="velocity">"Live" velocity value</param>
        /// <param name="equilibriumPosition">Goal (or rest) position</param>
        /// <param name="deltaTime">Time to update over</param>
        /// <param name="angularFrequency">Angular frequency of motion</param>
        /// <param name="dampingRatio">Damping ratio of motion</param>
        public static void CalcDampedSimpleHarmonicMotion(ref Vector3 position, ref Vector3 velocity,
            Vector3 equilibriumPosition, float deltaTime, float angularFrequency, float dampingRatio)
        {
            var motionParams = CalcDampedSpringMotionParams(deltaTime, angularFrequency, dampingRatio);
            UpdateDampedSpringMotion(ref position.x, ref velocity.x, equilibriumPosition.x, motionParams);
            UpdateDampedSpringMotion(ref position.y, ref velocity.y, equilibriumPosition.y, motionParams);
            UpdateDampedSpringMotion(ref position.z, ref velocity.z, equilibriumPosition.z, motionParams);
        }
        
        public static void CalcDampedSimpleHarmonicMotion(
            ref Quaternion currentRotation, 
            ref Vector3 angularVelocity, 
            Quaternion targetRotation, 
            float deltaTime, 
            float angularFrequency, 
            float dampingRatio)
        {
            var motionParams = CalcDampedSpringMotionParams(deltaTime, angularFrequency, dampingRatio);

            // 1. Get the relative rotation (error) between current and target
            // This is the "oldPos" in Quaternion space
            Quaternion error = currentRotation * Quaternion.Inverse(targetRotation);

            // 2. Convert Quaternion error to a Vector3 (Axis-Angle)
            error.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180f) angle -= 360f; // Handle shortest path
    
            // This is our 3D position representation of the rotation
            Vector3 rotationVector = axis * (angle * Mathf.Deg2Rad);

            // 3. Apply the spring logic to the 3D rotation vector and angular velocity
            // (Note: equilibriumPos is Vector3.zero because we are in relative space)
            Vector3 oldPos = rotationVector;
            Vector3 oldVel = angularVelocity;

            Vector3 newPos = oldPos * motionParams.posPosCoef + oldVel * motionParams.posVelCoef;
            angularVelocity = oldPos * motionParams.velPosCoef + oldVel * motionParams.velVelCoef;

            // 4. Convert the new 3D rotation vector back into a Quaternion
            float newAngleRad = newPos.magnitude;
            if (newAngleRad > Epsilon)
            {
                Quaternion deltaStep = Quaternion.AngleAxis(newAngleRad * Mathf.Rad2Deg, newPos.normalized);
                currentRotation = deltaStep * targetRotation;
            }
            else
            {
                currentRotation = targetRotation;
            }
        }
        public static void UpdateDampedSpringMotionQuaternion(
            ref Quaternion pPos,
            ref Vector3 pVel,
            Quaternion equilibriumPos,
            DampedSpringMotionParams parameters)
        {
            // 1. Calculate the relative rotation (the "error" or "distance")
            // This is equivalent to: relative = current * inv(target)
            Quaternion relative = pPos * Quaternion.Inverse(equilibriumPos);

            // 2. Convert to Axis-Angle (The "Vector" representation of the rotation)
            relative.ToAngleAxis(out float angle, out Vector3 axis);

            // Ensure we take the shortest path around the sphere
            if (angle > 180f) angle -= 360f;

            // rotationVector represents the "position" in angular space
            Vector3 rotationVector = axis * (angle * Mathf.Deg2Rad);

            // 3. Apply the standard spring coefficients to the 3D vector and velocity
            Vector3 oldPos = rotationVector;
            Vector3 oldVel = pVel;

            Vector3 newPos = oldPos * parameters.posPosCoef + oldVel * parameters.posVelCoef;
            pVel = oldPos * parameters.velPosCoef + oldVel * parameters.velVelCoef;

            // 4. Convert back to Quaternion
            float newAngleRad = newPos.magnitude;
            if (newAngleRad > Epsilon)
            {
                Quaternion deltaStep = Quaternion.AngleAxis(newAngleRad * Mathf.Rad2Deg, newPos.normalized);
                pPos = deltaStep * equilibriumPos;
            }
            else
            {
                pPos = equilibriumPos;
            }
        }
    }
}