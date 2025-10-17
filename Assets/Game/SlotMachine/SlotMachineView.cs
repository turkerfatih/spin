using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class SlotMachineView:MonoBehaviour
    {
        [SerializeField] private ReelView ReelViewPrefab;
        private List<ReelView> reelViews;
        
        public void Setup()
        {
            Services.MachineView = this;
            var reelCount = Services.Machine.Count;
            reelViews = new List<ReelView>(reelCount);
            var left = (-ReelView.Width * reelCount/2f)+(ReelView.Width/2);
            for (int i = 0; i < reelCount; i++)
            {
                var reel = Services.Machine.Reels[i];
                var reelView = Instantiate(ReelViewPrefab, transform);
                reelViews.Add(reelView);
                reelView.transform.localPosition = new Vector3(left + i * ReelView.Width, 0, 0);
                reelView.Setup(reel.Symbols,i);
                reel.View = reelView;

            }
            
        }
        
    }
}