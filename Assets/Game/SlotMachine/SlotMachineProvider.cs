using System;
using System.Collections.Generic;
using Game.Event;
using UnityEngine;

namespace Game
{
    public class SlotMachineProvider : MonoBehaviour
    {
        [SerializeField] private List<ReelConfiguration> ReelConfigurations;

        public int Count => ReelConfigurations.Count;

        private List<Symbol>[] reels;
        private List<int> paylineValues;
        
        private bool isSpinning = false;
        
        
        private Camera gameCamera;
        private void Awake()
        {
            gameCamera = Camera.main;
            Services.SlotMachine = this;
            reels = new List<Symbol>[Count];
            paylineValues = new List<int>(Count);
            for (var i = 0; i < ReelConfigurations.Count; i++)
            {
                var configuration = ReelConfigurations[i];
                reels[i] = new List<Symbol>();
                paylineValues.Add( 0);
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
            isSpinning=false;
            AfterSpin();
        }

        public void SetReel(int reelIndex, int symbolsIndex)
        {
            paylineValues[reelIndex] = symbolsIndex;
        }



        private void AfterSpin()
        {
            //DrawCard();
            Span<SymbolType> symbolTypes = stackalloc SymbolType[reels.Length];
            for (int i = 0; i < reels.Length; i++)
            {
                var reel = reels[i];
                symbolTypes[i] = reel[paylineValues[i]].Type;
            }

            var result= Services.PayTable.Evaluate(symbolTypes);
            Debug.Log("Payout Count:"+result.Count);
            foreach (var payout in result)
            {
                Debug.Log("Payout:"+payout.Amount +" for "+payout.Count +"X"+payout.Symbol);
                EventBus.OnCoinGiven?.Invoke(payout.Amount);
            }
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