using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class ReelView : MonoBehaviour
    {
        public const float Width = 2.75f;
        private const float Speed = SymbolView.Height*10;
        private const int DuplicatesNeeded = 3;
        
        [SerializeField]
        private Transform VerticalList;
        
        private List<Symbol> symbols;
        private List<SymbolView> views;//going to be used for symbol effects
        private int currentIndex;
        private float bottomLimit;
        private int reelIndex;
        private float spinVel;
        private float spinSpeed;
        
     

        public void Setup(List<Symbol> items,int index)
        {
            reelIndex=index;
            symbols = items;
            views = new List<SymbolView>(items.Count);
            bottomLimit = items.Count * SymbolView.Height;
            Debug.Log("reel view bottom limit:"+bottomLimit);
            var pos = new Vector3(0, SymbolView.Height*DuplicatesNeeded, 0);
            for (int i = 0; i < DuplicatesNeeded; i++)
            {
                AddView(items.Count-DuplicatesNeeded+i,pos);
                pos.y -= SymbolView.Height;
            }
            for (int i = 0; i < symbols.Count; i++)
            {
                AddView(i,pos);
                pos.y -= SymbolView.Height;
            }
            for (int i = 0; i < DuplicatesNeeded; i++)
            {
                AddView(i,pos);
                pos.y -= SymbolView.Height;
            }
        }

        private void AddView(int index,Vector3 pos)
        {
            var sym = symbols[index];
            var symbolView = Services.SymbolBuilder.GetView(VerticalList, sym,index);
            views.Add(symbolView);
            symbolView.transform.localPosition = pos;
        }
        
        
        public async UniTask Spin(Guid symbolId,float delay=0)
        {
            int targetIndex = symbols.FindIndex(s => s.Id == symbolId);
            if (targetIndex == -1)
            {
                Debug.LogWarning("Symbol ID not found on reel.");
                return ;
            }
            Debug.Log($"target index:{targetIndex}");
 
            await Spin(targetIndex, delay);
            
        }
        
        private async UniTask Spin(int target,float delay=0)
        {
            var startDelay=Random.Range(0.1f,0.3f);
            await UniTask.WaitForSeconds(startDelay);
            var duration = 2f+delay;
            var time = 0f;
           
            spinVel = -1f;
            spinSpeed = 0;
            while (time<=duration)
            {
                var currentPos = VerticalList.localPosition;
                spinSpeed= Mathf.SmoothDamp(spinSpeed,Speed,ref spinVel,0.7f);
                var moveAmount = new Vector3(0, - spinSpeed * Time.deltaTime, 0);
                var nextPos = currentPos + moveAmount;
                if (nextPos.y < 0)
                {
                    nextPos.y = bottomLimit-nextPos.y;
                }
                time += Time.deltaTime;
                VerticalList.localPosition = nextPos;
                await UniTask.WaitForEndOfFrame(this);
            }
            
            var targetY = SymbolView.Height*target;
            var jumpY = SymbolView.Height * (target + 2);
            var pos = VerticalList.localPosition;
            pos.y = jumpY;//fake landing with sudden locate
            VerticalList.localPosition = pos;
            var dur = 0.55f;//Random.Range(0.5f, 1f);
            
            Ease ease = Ease.OutBack;
            if (Random.value > 0.5)
            {
                ease = Ease.OutElastic;
            }
            ease=Ease.InFlash;
            await VerticalList.DOLocalMoveY(targetY, dur).SetEase(ease).ToUniTask();
            
            OnSpinComplete();
        }
        

        private Vector3 FixAlign(Vector3 currentPos,float moveAmount)
        {
            var nextPosY = currentPos.y + moveAmount;
            if (nextPosY > bottomLimit)
            {
                nextPosY = 0;
                VerticalList.localPosition=new Vector3(currentPos.x, nextPosY, currentPos.z);
            }

            if (nextPosY < 0)
            {
                nextPosY = bottomLimit+currentPos.y;
                VerticalList.localPosition=new Vector3(currentPos.x, nextPosY, currentPos.z);
            }

            return VerticalList.localPosition;
        }

        private void OnSpinComplete()
        {
            EventBus.OnReelSpinEnd?.Invoke(reelIndex);
        }

        public void Push()
        {
            PushOrPull(1);
        }

        public void Pull()
        {
            PushOrPull(-1);
        }

        private void PushOrPull(int dir)
        {
            var amount = SymbolView.Height * dir;
            var pos =FixAlign(VerticalList.localPosition,amount);
            VerticalList.DOLocalMoveY(pos.y + amount, 0.75f)
                .SetEase( dir>0 ? Ease.InBack : Ease.OutBack).OnComplete(OnSpinComplete);
        }
        
    }
}
