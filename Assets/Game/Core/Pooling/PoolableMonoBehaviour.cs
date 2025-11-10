using UnityEngine;

namespace Game.Pooling
{
    public class PoolableMonoBehaviour<T> : MonoBehaviour, IPoolable<T> 
        where T : PoolableMonoBehaviour<T>
    {
        private IBasePool<T> pool;

        public void SetPool(IBasePool<T> pool)
        {
            this.pool = pool;
        }

        public void ReturnToPool()
        {
            pool?.Return((T)this);
        }

        public virtual void OnSpawnFromPool() { }

        public virtual void OnReturnToPool() { }
    }
}