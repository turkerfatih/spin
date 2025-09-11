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
        
        private List<DropSlot> dropSlots;
        public List<Reel> Reels;
        private List<Symbol> symbols;
        
        private SpinResult spinResult;

        public bool IsSpinning { get;  set; }
        
        private void Awake()
        {
            Services.Machine = this;
            spinResult = new SpinResult();
            
            dropSlots=new List<DropSlot>(Count);
            Reels=new List<Reel>(Count);
            symbols=new List<Symbol>();
            var configuration = ReelConfiguration;
            ReelSymbolPlacement.GenerateReelSymbols(configuration,ref symbols);
            Debug.Log(symbols.Count);
            for (var i = 0; i < ReelCount; i++)
            {
                dropSlots.Add(null);
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
                EventBus.OnCoinGiven?.Invoke(payout.Amount);
            }
            
            //todo: animate rewards
        }

        public async UniTask ResolveRocks()
        {
            spinResult.Inputs.Clear();
            foreach (var reel in Reels)
            {
                spinResult.Inputs.Add(reel.CurrentSymbol);
            }
            Services.PayTable.Evaluate(spinResult.Inputs,spinResult.Payouts);
            Debug.Log("ResolveRocks Payouts.Count:"+spinResult.Payouts.Count);
            if(spinResult.Payouts.Count==0)
                return;
            
            foreach (var payout in spinResult.Payouts)
            {
                if(payout.Amount!=0)
                    continue;
                if (payout.Symbols[0].Type == SymbolType.Rock)
                {
                    for (int i = 0; i < payout.Symbols.Count; i++)
                    {
                        var sym=payout.Symbols[i];
                        var ri = payout.Indexes[i];
                        if(sym.Type != SymbolType.Rock)
                            continue;
                        await Reels[ri].ChangeSymbol(sym,SymbolType.Rock2);
                    }   
                }else if (payout.Symbols[0].Type == SymbolType.Rock2)
                {
                    for (int i = 0; i < payout.Symbols.Count; i++)
                    {
                        var sym=payout.Symbols[i];
                        var ri = payout.Indexes[i];
                        if(sym.Type != SymbolType.Rock2)
                            continue;
                        await Reels[ri].ChangeSymbol(sym,SymbolType.GoldOre);
                    } 
                }
            }
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

        public void RegisterDropSlot(DropSlot dropSlot, int index)
        {
            dropSlots[index] = dropSlot;
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