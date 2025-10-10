using UnityEngine;

namespace Game.Pooling
{
    public class PoolableMonoBehaviour:MonoBehaviour,IPoolable
    {
        private IBasePool<PoolableMonoBehaviour> pool;
        public void SetPool<T>(IBasePool<T> pool) where T : IPoolable
        {
            this.pool = pool as IBasePool<PoolableMonoBehaviour>;
        }

        public void ReturnToPool()
        {
            pool?.Return(this);
        }

        public virtual void OnSpawnFromPool() { }
        public virtual void OnReturnToPool() { }
     
    }
}