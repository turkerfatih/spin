using System.Collections.Generic;
using UnityEngine;

namespace Game.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        private readonly Dictionary<GameObject, object> gameObjectPools = new();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void CreatePool<T>(T prefab, int initialSize = 10) where T : PoolableMonoBehaviour
        {
            if (gameObjectPools.ContainsKey(prefab.gameObject)) return;

            var pool = new GameObjectPool<T>(prefab, initialSize, transform);
            gameObjectPools.Add(prefab.gameObject, pool);
        }

        public T Get<T>(T prefab, Vector3 position, Quaternion rotation) where T : PoolableMonoBehaviour
        {
            if (!gameObjectPools.ContainsKey(prefab.gameObject))
                CreatePool(prefab, 5);

            var pool = gameObjectPools[prefab.gameObject] as GameObjectPool<T>;
            var obj = pool.Get();
            obj.transform.SetPositionAndRotation(position, rotation);
            return obj;
        }

        public void Return<T>(T obj) where T : PoolableMonoBehaviour
        {
            obj.ReturnToPool();
        }
    }
}