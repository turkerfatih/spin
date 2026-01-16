using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core.Animation;
using UnityEngine;

namespace Game.Test
{
    public class CoinTester:MonoBehaviour
    {
        public Coin prefab;
        public Transform spawnLoc;
        public Transform targetLoc;

        private float coinHeight = 0.7f;
        private Queue<Coin> coins = new Queue<Coin>(10);

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            while (coins.Count>0)
            {
                var coin=coins.Dequeue();
                Destroy(coin.gameObject);
            }

            var p = targetLoc.position;
            for (int i = 0; i < 7; i++)
            {
                var coin = Instantiate(prefab, spawnLoc.position, Quaternion.Euler(80, 0, 0));
                coins.Enqueue(coin);
                var t=new Vector3(p.x,p.y,p.z);
                t.y+=coinHeight*i;
                coin.ResetPosition(i);
                coin.Move(t).Forget();
            }
        }
    }
}