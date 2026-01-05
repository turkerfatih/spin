using UnityEngine;

namespace Game.Core.Animation
{
    public abstract class BaseSpring<T,K>:ICachedEvaluate,INudgeable<K>
    {
        public float AngularFrequency = 15.0f;
        public float DampingRatio = 0.6f;

        public T EndValue;
        public T CurrentValue;
        public T CurrentVelocity;

        // Reset to a specific state
        public  virtual void Reset(T value)
        {
            CurrentValue = value;
            CurrentVelocity = default;
            EndValue = value;
        }

        // Standard update (Calculates params internally)
        public abstract T Evaluate(float deltaTime);

        // Batch update (Uses pre-calculated params for performance)
        public abstract void Evaluate(DampedSpringMotionParams cachedParams);
        public abstract bool IsSettled(float precision = Springs.Epsilon);

        public abstract void Nudge(K t);
    }
}