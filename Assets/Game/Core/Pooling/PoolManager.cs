using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pooling
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        private readonly Dictionary<int, object> pools = new();

        private void Awake()
        {
            Services.Pool = this;
        }

        public void CreatePool<T>(T prefab, int initialSize = 10) where T : PoolableMonoBehaviour<T>
        {
            int key = prefab.GetInstanceID();
            if (pools.ContainsKey(key))
                return;

            var pool = new GameObjectPool<T>(prefab, initialSize, transform);
            pools.Add(key, pool);
        }

        public T Get<T>(T prefab) where T : PoolableMonoBehaviour<T>
        {
            int key = prefab.GetInstanceID();

            if (!pools.TryGetValue(key, out var poolObj))
            {
                CreatePool(prefab, 5);
                poolObj = pools[key];
            }

            var pool = poolObj as GameObjectPool<T>;
            return pool.Get();
        }

        public void Return<T>(T obj) where T : PoolableMonoBehaviour<T>
        {
            obj.ReturnToPool();
        }
    }
}