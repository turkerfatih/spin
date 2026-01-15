using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Effects;
using Game.Event;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class SlotMachine : MonoBehaviour
    {
         [SerializeField] private ReelConfiguration ReelConfiguration;
         [SerializeField] private int ReelCount;

        private readonly List<MachineEffect> effects = new();

        public void AddEffect(MachineEffect effect) => effects.Add(effect);
        public void RemoveEffect(MachineEffect effect) => effects.Remove(effect);
        public int Count => ReelCount;
        
        public List<Reel> Reels;
        private List<Symbol> symbols;
        
        private SpinResult spinResult;

        public bool IsSpinning { get;  set; }
        
        public void Setup()
        {
            Services.Machine = this;
            spinResult = new SpinResult();
            
            Reels=new List<Reel>(Count);
            symbols=new List<Symbol>();
            var configuration = ReelConfiguration;
            ReelSymbolPlacement.GenerateReelSymbols(configuration,ref symbols);
            for (var i = 0; i < ReelCount; i++)
            {
                Reels.Add(new Reel(symbols,i));
            }
            
        }



        public void PrintCurrentSymbols()
        {
            foreach (var reel in Reels)
            {
                Debug.Log(reel.CurrentSymbol.Id);
            }
        }

        public List<Symbol> GetSymbolsOnTheReel(int index )
        {
            return Reels[index].Symbols;
        }

        public async UniTask ResolvePayout()
        {
            spinResult.Inputs.Clear();
            foreach (var reel in Reels)
            {
                spinResult.Inputs.Add(reel.CurrentSymbol);
            }
            Services.PayTable.Evaluate(spinResult.Inputs,spinResult.Payouts);
            if(spinResult.Payouts.Count==0)
                return;
            Debug.Log("Payout Count:"+spinResult.Payouts.Count);
            foreach (var payout in spinResult.Payouts)
            {
                await AnimateSymbols(payout);
                Debug.Log("Payout:"+payout.Amount +" for ");
                foreach (var symbol  in payout.Symbols)
                {
                    Debug.Log(" - Symbol:"+symbol.Type);
                }
                //EventBus.OnScoreGiven?.Invoke(payout.Amount);
                await Services.DebtMeter.Reduce(payout.Amount);
                Services.Round.Collect(payout.Amount);
            }
            
            //todo: animate rewards
        }

        private async UniTask AnimateSymbols(PayoutResult payout)
        {
            List<UniTask> animations=new List<UniTask>();
            for (var i = 0; i < payout.Symbols.Count; i++)
            {
                var symbol = payout.Symbols[i];
                var reelIndex = payout.Indexes[i];
                animations.Add( Reels[reelIndex].AnimateSymbol(symbol.Id,0));
            }
            await UniTask.WhenAll(animations);
        }

        public async UniTask AdvanceSlots()
        {
            var dropSlots = Services.DropSlot.GetDropSlots();
            foreach (var slot in dropSlots)
            {
                var card = slot.GetCard;
                if (card == null)
                {
                    //Debug.Log($"Slot[{slot.Index}] no card skipped ");
                    continue;
                }

                if (card.OnAfterSpin())
                {
                    //Debug.Log($"Slot[{slot.Index}] clear call ");
                    slot.ClearCard();
                    Services.Hand.OnCardReturnFromSlot(card);
                }
                
            }
        }

        public bool CanSpin()
        {
            return !IsSpinning;
        }


    }

}