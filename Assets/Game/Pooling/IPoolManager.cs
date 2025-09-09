using UnityEngine;

namespace Game.Pooling
{
    public interface IPoolManager
    {
        void CreatePool<T>(T prefab, int initialSize = 10) where T : PoolableMonoBehaviour;
        void Return<T>(T obj) where T : PoolableMonoBehaviour;
        public T Get<T>(T prefab) where T : PoolableMonoBehaviour;
    }
}