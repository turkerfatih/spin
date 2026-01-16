using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Actions;
using UnityEngine;

namespace Game
{
    public class RoundStackController:MonoBehaviour
    {
        [SerializeField] private float fillCoinDuration = 0.15f;
        [SerializeField] private Vector3 from;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float coinHeight;
        [SerializeField] private float startRotation;
        [SerializeField] private float finalRotation;
        
        private Queue<Coin> coins=new Queue<Coin>(10);
        private TweenCallback onCompleteCache;

        private void Awake()
        {
            Services.RoundStack = this;
            onCompleteCache = ShakeCoins;
        }

        public UniTask AnimateSetup(int count)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < count; i++)
            {
                var coin = Services.Pool.Get<Coin>();
                coins.Enqueue(coin);
                coin.transform.SetParent(transform);
                coin.transform.localPosition=from;
                coin.transform.localRotation = Quaternion.Euler(startRotation,0,0);
                var target=new Vector3(offset.x,offset.y+i*coinHeight,offset.z);
                var moveAnim = coin.transform.DOLocalMove(target, fillCoinDuration).SetEase(Ease.InExpo);
                var rotateAnim=coin.transform.DOLocalRotate(new Vector3(finalRotation,0,0), fillCoinDuration*0.75f).SetEase(Ease.InExpo);
                sequence.Join(moveAnim);
                sequence.Join(rotateAnim);
            }
            sequence.OnComplete(onCompleteCache);
            return sequence.ToUniTask();
        }

        private void ShakeCoins()
        {
            var sequence = DOTween.Sequence();
            int i = 0;
            foreach (var item in coins)
            {
                var anim = item.transform.DOPunchPosition(Vector3.up*coinHeight, fillCoinDuration*0.25f).SetDelay(i*0.05f);
                sequence.Join(anim );
                i++;
            }
        }

        public async UniTask Spin()
        {
            if(coins.Count==0)
                return;
            await AnimateCoinRemoval();
            Services.Actions.Add(new SpinAction());
        }

        public async UniTask AnimateCoinRemoval()
        {
            var coin = coins.Dequeue();
            var pos = coin.transform.localPosition;
            pos.z = 0.8f;
            pos.y -= coinHeight;
            var sequence = DOTween.Sequence();
            var coinMove= coin.transform.DOLocalMove(pos, fillCoinDuration).SetEase(Ease.InExpo);
            sequence.Join(coinMove);
            
            foreach (var item in coins)
            {
                var p=item.transform.localPosition;
                p.y -= coinHeight;
                sequence.Join( item.transform.DOLocalMove(p, fillCoinDuration).SetEase(Ease.InExpo));
            }
            await sequence.ToUniTask();
            Services.Pool.Return(coin);
        }
    }
}