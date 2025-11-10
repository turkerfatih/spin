using UnityEngine;

namespace Game.Pooling
{
    public interface IPoolManager
    {
        void CreatePool<T>(T prefab, int initialSize = 10) where T : PoolableMonoBehaviour<T>;
        T Get<T>(T prefab) where T : PoolableMonoBehaviour<T>;
        void Return<T>(T obj) where T : PoolableMonoBehaviour<T>;   }
}