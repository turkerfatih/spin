using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pooling
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        private readonly Dictionary<Type, object> pools = new();

        private void Awake()
        {
            Services.Pool = this;
        }

        public void CreatePool<T>(T prefab, int initialSize = 10) where T : PoolableMonoBehaviour<T>
        {
            var key = typeof(T);
            if (pools.ContainsKey(key))
                return;

            var pool = new GameObjectPool<T>(prefab, initialSize, transform);
            pools.Add(key, pool);
        }

        public T Get<T>() where T : PoolableMonoBehaviour<T>
        {
            var key = typeof(T);
            var pool = pools[key] as GameObjectPool<T>;
            return pool.Get();
        }
        

        public void Return<T>(T obj) where T : PoolableMonoBehaviour<T>
        {
            obj.ReturnToPool();
        }
    }
}