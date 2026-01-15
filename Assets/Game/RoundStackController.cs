using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class RoundStackController:MonoBehaviour
    {
        [SerializeField] private float fillCoinDuration = 0.35f;
        [SerializeField] private Vector3 from;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float coinHeight;
        [SerializeField] private float startRotation;
        [SerializeField] private float finalRotation;

        private void Awake()
        {
            Services.RoundStack = this;
        }

        public UniTask AnimateSetup(int count)
        {
            var sequence = DOTween.Sequence();
            for (int i = 0; i < count; i++)
            {
                var coin = Services.Pool.Get<Coin>();
                coin.transform.SetParent(transform);
                coin.transform.localPosition=from;
                coin.transform.localRotation = Quaternion.Euler(startRotation,0,0);
                var target=new Vector3(offset.x,offset.y+i*coinHeight,offset.z);
                var moveAnim=coin.transform.DOLocalMove(target, fillCoinDuration);
                var rotateAnim=coin.transform.DOLocalRotate(new Vector3(finalRotation,0,0), fillCoinDuration);
                sequence.Append(moveAnim);
                sequence.Join(rotateAnim);
            }
            

            return sequence.ToUniTask();
        }
    }
}