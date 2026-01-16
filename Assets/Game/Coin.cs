using System;
using Cysharp.Threading.Tasks;
using Game.Core.Animation;
using Game.Pooling;
using UnityEngine;

namespace Game
{
    public class Coin:PoolableMonoBehaviour<Coin>
    {
        [SerializeField]
        private float frequency=20f;
        [SerializeField]
        private float damping=0.5f;
        private Spring3Handle posHandle;
        private Vector3Spring spring;
        private void Awake()
        {
            spring = new Vector3Spring{AngularFrequency = frequency, DampingRatio = damping };
            posHandle = new Spring3Handle(this, spring, UpdateVisualPosition);
        }

        private void UpdateVisualPosition(Vector3 pos)
        {
            transform.localPosition = pos;
        }

        public UniTask Move(Vector3 p)
        {
            return posHandle.Play(p);
        }

        public void ResetPosition(int i=0)
        {
            spring.AngularFrequency = spring.AngularFrequency - i*0.75f;
            spring.Reset(transform.localPosition);
        }
    }
}