using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.Animation
{
    public class SpringHandle<T, TSpring,K> where TSpring : BaseSpring<T,K>
    {
        public readonly TSpring Spring;
        private readonly MonoBehaviour _owner;
        private readonly Action<T> _onUpdate;
        
        private bool _isUpdating;
        public bool IsUpdating => _isUpdating;
        public event Action OnSettled;

        public SpringHandle(MonoBehaviour owner, TSpring spring, Action<T> onUpdate)
        {
            _owner = owner;
            Spring = spring;
            _onUpdate = onUpdate;
        }

        public UniTask Play(T targetValue)
        {
            Spring.EndValue = targetValue;
            if (!_isUpdating) return RunLoop();
            
            // If already running, we return the same task logic
            return UniTask.WaitUntil(() => !_isUpdating, 
                cancellationToken: _owner.GetCancellationTokenOnDestroy());
        }
  

        private async UniTask RunLoop()
        {
            if (_owner == null) return;

            _isUpdating = true;
            try
            {
                // Safety check: owner must exist and be active
                while (_owner != null && !Spring.IsSettled())
                {
                    // Only update if the object is active in the hierarchy
                    if (_owner.isActiveAndEnabled)
                    {
                        T newValue = Spring.Evaluate(Time.deltaTime);
                        _onUpdate?.Invoke(newValue);
                    }

                    await UniTask.Yield(PlayerLoopTiming.Update, _owner.GetCancellationTokenOnDestroy());
                }

                // Final snap if the owner still exists
                if (_owner != null)
                {
                    _onUpdate?.Invoke(Spring.EndValue);
                }
            }
            finally
            {
                _isUpdating = false;
                OnSettled?.Invoke();
            }
        }
        public void Nudge(K amount)
        {
            Spring.Nudge(amount);
    
            // If the spring was settled, we need to wake it up!
            if (!_isUpdating)
            {
                RunLoop().Forget();
            }
        }
    }

    public class SpringHandle:SpringHandle<float,FloatSpring,float>
    {
        public SpringHandle(MonoBehaviour owner, FloatSpring spring, Action<float> onUpdate) : base(owner, spring, onUpdate)
        {
        }
    }

    public class Spring2Handle : SpringHandle<Vector2, Vector2Spring,Vector2>
    {
        public Spring2Handle(MonoBehaviour owner, Vector2Spring spring, Action<Vector2> onUpdate) : base(owner, spring, onUpdate)
        {
        }
    }

    public class Spring3Handle : SpringHandle<Vector3, Vector3Spring,Vector3>
    {
        public Spring3Handle(MonoBehaviour owner, Vector3Spring spring, Action<Vector3> onUpdate) : base(owner, spring, onUpdate)
        {
        }
    }
}