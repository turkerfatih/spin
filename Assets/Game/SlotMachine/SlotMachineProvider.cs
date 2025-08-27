using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SlotMachineProvider : MonoBehaviour
    {
        [SerializeField] private List<ReelConfiguration> ReelConfigurations;

        public int Count => ReelConfigurations.Count;

        private List<Symbol>[] reels;
        
        private bool isSpinning = false;
        
        
        private Camera gameCamera;
        private void Awake()
        {
            gameCamera = Camera.main;
            Services.SlotMachine = this;
            reels = new List<Symbol>[Count];
            for (var i = 0; i < ReelConfigurations.Count; i++)
            {
                var configuration = ReelConfigurations[i];
                reels[i] = new List<Symbol>();
                ReelSymbolPlacement.GenerateReelSymbols(configuration,ref reels[i]);
                Debug.Log("reel"+i+" has "+reels[i].Count+" item");
            }

        }

        public List<Symbol> GetReel(int index)
        {
            return reels[index];
        }

        public async void Spin()
        {
            if(isSpinning)
                return;
            isSpinning=true;
            Services.Sound.StartSpinning();
            await Services.MachineView.Spin();
            Services.Sound.StopSpinning();
            //DrawCard();
            isSpinning=false;
        }

        public void DrawCard()
        {
            var deck = Services.Deck;
            if(deck.Library.Size==0 && deck.Discarded.Size==0)
                return;
            Services.CurrentHand.AddCard();
        }


    }

}