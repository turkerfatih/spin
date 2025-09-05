using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Effects;
using Game.Event;
using UnityEngine;

namespace Game
{
    public class SlotMachine : MonoBehaviour
    {
        [SerializeField] private List<ReelConfiguration> ReelConfigurations;

        private readonly List<MachineEffect> effects = new();

        public void AddEffect(MachineEffect effect) => effects.Add(effect);
        public void RemoveEffect(MachineEffect effect) => effects.Remove(effect);
        public int Count => ReelConfigurations.Count;

        private List<Symbol>[] symbolsOnTheReels;
        private List<DropSlot> dropSlots;
        public List<Reel> Reels;
        
        private SpinResult spinResult;

        public bool IsSpinning { get;  set; }
        
        private void Awake()
        {
            Services.Machine = this;
            spinResult = new SpinResult();
            
            symbolsOnTheReels = new List<Symbol>[Count];
            dropSlots=new List<DropSlot>(Count);
            Reels=new List<Reel>(Count);
            for (var i = 0; i < ReelConfigurations.Count; i++)
            {
                var configuration = ReelConfigurations[i];
                symbolsOnTheReels[i] = new List<Symbol>();
                dropSlots.Add(null);
                var symbols=new List<Symbol>();
                ReelSymbolPlacement.GenerateReelSymbols(configuration,ref symbols);
                Reels.Add(new Reel(symbols,i));
            }

        }

        public async UniTask ResolveSpin()
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
                Debug.Log("Payout:"+payout.Amount +" for ");
                foreach (var symbol  in payout.Symbols)
                {
                    Debug.Log(" - Symbol:"+symbol.Type);
                }
                EventBus.OnCoinGiven?.Invoke(payout.Amount);
            }
            //todo: animate rewards
        }

        public void RegisterDropSlot(DropSlot dropSlot, int index)
        {
            dropSlots[index] = dropSlot;
        }

        public List<Symbol> GetReel(int index)
        {
            return symbolsOnTheReels[index];
        }

        
        public DropSlot GetDropSlot(int index)
        {
            return dropSlots[index];
        }

        public async UniTask AdvanceSlots()
        {
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


    }

}