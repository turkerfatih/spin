using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Core.Animation
{
    public class SpringHandle<T, TSpring> where TSpring : BaseSpring<T>
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
    }
}