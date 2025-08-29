using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class InfiniteRandomBag<T>
    {
        private readonly T[] items;
        private int currentIndex;

        public InfiniteRandomBag(IEnumerable<T> values)
        {
            if (values == null)
                throw new System.ArgumentNullException(nameof(values));

            items = new List<T>(values).ToArray();
            Shuffle(items);
            currentIndex = 0;
        }
        
        public T GetRandom()
        {
            if (currentIndex >= items.Length)
            {
                Shuffle(items);
                currentIndex = 0;
            }

            return items[currentIndex++];
        }
        
        public void Reset()
        {
            Shuffle(items);
            currentIndex = 0;
        }
        
        private static void Shuffle(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]); // swap
            }
        }
    }
}