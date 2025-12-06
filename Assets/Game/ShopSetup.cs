using System;
using UnityEngine;

namespace Game
{
    public class ShopSetup:MonoBehaviour
    {
        [SerializeField] private Shop ShopPrefab;
        private void Start()
        {
            var shop = Instantiate(ShopPrefab);
            shop.Setup();
        }
    }
}