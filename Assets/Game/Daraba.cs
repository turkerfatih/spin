using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Animation;
using UnityEngine;

namespace Game
{
    public class Daraba:MonoBehaviour
    {
        [SerializeField] private Transform cover;
        private SpringHandle posHandle;
        private FloatSpring spring;
        public Transform Cover => cover;
        [SerializeField]
        private float frequency=20f;
        [SerializeField]
        private float damping=0.5f;

        private Vector3 coverPos;
        private void Awake()
        {
            coverPos=cover.localPosition;
            spring = new FloatSpring{AngularFrequency = frequency, DampingRatio = damping };
            posHandle = new SpringHandle(this, spring, UpdateVisualPosition);

        }

        private void UpdateVisualPosition(float val)
        {
            coverPos.y = val;
            cover.localPosition = coverPos;
        }

        public  UniTask Open(float val)
        {
            //spring.Reset(0);
            //return posHandle.Play(val);
            return cover.DOLocalMoveY(val,0.35f).SetEase(Ease.InCirc).ToUniTask();
        }
    }
}