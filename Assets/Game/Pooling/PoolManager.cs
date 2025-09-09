using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pooling
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        private void Awake()
        {
            Services.Pool = this;
        }

        private readonly Dictionary<GameObject, object> gameObjectPools = new();
        
        public void CreatePool<T>(T prefab, int initialSize = 10) where T : PoolableMonoBehaviour
        {
            if (gameObjectPools.ContainsKey(prefab.gameObject)) return;

            var pool = new GameObjectPool<T>(prefab, initialSize, transform);
            gameObjectPools.Add(prefab.gameObject, pool);
        }

        public void Return<T>(T obj) where T : PoolableMonoBehaviour
        {
            obj.ReturnToPool();
        }

        public T Get<T>(T prefab) where T : PoolableMonoBehaviour
        {
            if (!gameObjectPools.ContainsKey(prefab.gameObject))
                CreatePool(prefab, 5);

            var pool = gameObjectPools[prefab.gameObject] as GameObjectPool<T>;
            var obj = pool.Get();
            return obj;
        }
    }
}