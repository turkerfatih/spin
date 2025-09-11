using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Effects;

using UnityEngine;

namespace Game.Core
{
    public class Reel
    {
        public List<Symbol> Symbols { get;private set; }
        public bool IsFrozen { get; set; }
        public int CurrentSymbolIndex { get;private set; }
        public Symbol CurrentSymbol => Symbols[CurrentSymbolIndex];
        
        public int Index { get; private set; }
        public IReelView View { get; set; }

        private readonly List<ReelEffect> effects = new();

        public void AddEffect(ReelEffect effect)
        {
            effects.Add(effect);
            effect.OnApply(this);
        }

        public void RemoveEffect(ReelEffect effect) => effects.Remove(effect);

        public Reel(List<Symbol> symbols,int index)
        {
            //todo: we need to create new list for symbols sharing guid across reels!?
            Symbols = new List<Symbol>(symbols.Count);
            foreach (var symbol in symbols)
            {
                Symbols.Add(symbol.Clone());
            }

            //Symbols = symbols;
            Index = index;
        }

        public UniTask AnimateSymbol(Guid id,float delay=0)
        {
            return View.AnimateMatch(id,delay);
        }

        private async UniTask SpinWith(Func<UniTask> spinTask)
        {
            for (var i = effects.Count - 1; i >= 0; i--)
            {
                var effect = effects[i];
                effect.OnBeforeSpin(this);
            }
            if(!IsFrozen)
                await spinTask.Invoke();
            //if(IsFrozen)
             //   Debug.Log("Reel ["+Index+"] is frozen");
            for (var i = effects.Count - 1; i >= 0; i--)
            {
                
                var effect = effects[i];
                effect.OnAfterSpin(this);
            }
        }
        public async UniTask Spin(float delay)
        {
            await SpinWith(()=>SpinTask(delay));
        }

        private  UniTask SpinTask(float delay)
        {
            CurrentSymbolIndex=Services.Random.Range(0, Symbols.Count);
            var item = Symbols[CurrentSymbolIndex];
            return View.SpinAnimation(item.Id,delay);
        }

        public async UniTask Pull(float delay)
        {
            await SpinWith(()=>PullTask(delay));
        }
        public async UniTask Push(float delay)
        {
            await SpinWith(()=>PushTask(delay));
        }

        private UniTask PullTask(float delay)
        {
            MoveCurrentSymbol(-1);
            return View.PullAnimation(delay);
        }
        private  UniTask PushTask(float delay)
        {
            MoveCurrentSymbol(1);
            return View.PushAnimation(delay);
        }

        private void MoveCurrentSymbol(int dir)
        {
            CurrentSymbolIndex+=dir;
            if (CurrentSymbolIndex < 0)
                CurrentSymbolIndex = Symbols.Count - 1;
            if (CurrentSymbolIndex >= Symbols.Count)
                CurrentSymbolIndex = 0;
        }

        public async UniTask ChangeSymbol(Symbol from, SymbolType symbolType)
        {
            Debug.Log("change symbol "+from.Type+" to "+symbolType);
            foreach (var symbol in Symbols)
            {
                if(symbol!=from)
                    continue;
                from.ChangeType(symbolType);
                View.UpdateSymbolChange(symbol);
                break;
            }
        }

    }
}