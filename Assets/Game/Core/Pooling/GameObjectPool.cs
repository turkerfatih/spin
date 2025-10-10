using System.Collections.Generic;
using UnityEngine;

namespace Game.Pooling
{
    public class GameObjectPool<T> : IBasePool<T> where T : PoolableMonoBehaviour
    {
        private readonly Queue<T> pool = new();
        private readonly T prefab;
        private readonly Transform container;

        public GameObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            container = new GameObject(typeof(T).Name + " Pool").transform;
            if (parent != null) container.SetParent(parent);

            for (int i = 0; i < initialSize; i++)
            {
                var obj = CreateNew();
                Return(obj);
            }
        }

        private T CreateNew()
        {
            var obj = Object.Instantiate(prefab, container);
            obj.gameObject.SetActive(false);
            obj.SetPool(this);
            return obj;
        }

        public T Get()
        {
            if (pool.Count == 0)
                pool.Enqueue(CreateNew());

            var obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            obj.OnSpawnFromPool();
            return obj;
        }

        public void Return(T obj)
        {
            obj.OnReturnToPool();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(container);
            pool.Enqueue(obj);
        }
    }
}