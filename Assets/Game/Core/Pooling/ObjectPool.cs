using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pooling
{
    public class ObjectPool<T> : IBasePool<T> where T : IPoolable<T>
    {
        private readonly Queue<T> pool = new();
        private readonly Func<T> factory;

        public ObjectPool(Func<T> factory, int initialSize = 0)
        {
            this.factory = factory;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = factory();
                obj.SetPool(this);
                pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            if (pool.Count == 0)
            {
                var obj = factory();
                obj.SetPool(this);
                return obj;
            }

            var item = pool.Dequeue();
            item.OnSpawnFromPool();
            return item;
        }

        public void Return(T item)
        {
            item.OnReturnToPool();
            pool.Enqueue(item);
        }
    }
}