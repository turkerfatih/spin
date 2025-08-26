using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class SlotMachineView:MonoBehaviour
    {
        [SerializeField] private ReelView ReelViewPrefab;
        private List<ReelView> reelViews;
        [SerializeField]
        private Transform Frame;

        private float[] delay;
        private List<UniTask> spinTasks;

        public SlotMachineView()
        {
            delay = new[] { 0, 0.5f, 1f };
        }

        private void Awake()
        {
            Services.MachineView = this;
            spinTasks=new List<UniTask>();
        }

        private void Start()
        {
            var reelCount = Services.SlotMachine.Count;
            reelViews = new List<ReelView>(reelCount);
            var left = (-ReelView.Width * reelCount/2f)+(ReelView.Width/2);
            for (int i = 0; i < reelCount; i++)
            {
                var reelView = Instantiate(ReelViewPrefab, transform);
                reelViews.Add(reelView);
                reelView.transform.localPosition = new Vector3(left + i * ReelView.Width, 0, 0);
                reelView.Setup(Services.SlotMachine.GetReel(i),i);
            }

            //var mid = (left + (reelCount-1) * ReelView.Width) / 2;
            //Frame.localPosition = new Vector3(mid, 0, 0);
        }

        public  async UniTask Spin()
        {
            //SpinLastPos();
            //SpinFirstPos();
            //return;
            spinTasks.Clear();
            int delayIndex = Random.Range(0, 3);
            for (var i = 0; i <= reelViews.Count-1; i++)
            {
                var reelView = reelViews[i];
                    
                var reel = Services.SlotMachine.GetReel(i);
                var r = Services.Random.Range(0, reel.Count);
                var item = reel[r];
                //spinResult.Symbol=item.Type;
                var d = delay[(delayIndex + i) % 3];
                spinTasks.Add( reelView.Spin(item.Id,d));
                //spinResult.Delay = d;
            }
            await UniTask.WhenAll(spinTasks);
        }
        public void SpinLastPos()
        {
            int delayIndex = Random.Range(0, 3);
            for (var i = 0; i < reelViews.Count; i++)
            {
                var reelView = reelViews[i];
                    
                var reel = Services.SlotMachine.GetReel(i);
                var item = reel[^1];
                reelView.Spin(item.Id,delay[(delayIndex+i) % 3]);
            }
        }
        public void SpinFirstPos()
        {
            int delayIndex = Random.Range(0, 3);
            for (var i = 0; i < reelViews.Count; i++)
            {
                var reelView = reelViews[i];
                    
                var reel = Services.SlotMachine.GetReel(i);
                var item = reel[0];
                reelView.Spin(item.Id,delay[(delayIndex+i) % 3]);
            }
        }

        public void ReelSpin(int slotIndex)
        {
            var reelView = reelViews[slotIndex];
                    
            var reel = Services.SlotMachine.GetReel(slotIndex);
            var r = Random.Range(0, reel.Count);
            var item = reel[r];
            reelView.Spin(item.Id);
        }

        public void ReelPush(int slotIndex)
        {
            var reelView = reelViews[slotIndex];
            reelView.Push();
        }
        
        public void ReelPull(int slotIndex)
        {
            var reelView = reelViews[slotIndex];
            reelView.Pull();
        }
    }
}