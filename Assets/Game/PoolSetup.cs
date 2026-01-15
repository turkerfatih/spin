using System;
using UnityEngine;

namespace Game
{
    public class PoolSetup:MonoBehaviour
    {
        [SerializeField] private Coin coinPrefab;

        private void Awake()
        {
            Services.Pool.CreatePool(coinPrefab,30);
        }
    }
}