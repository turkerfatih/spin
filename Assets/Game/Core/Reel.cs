using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Effects;

namespace Game.Core
{
    public class Reel
    {
        public List<Symbol> Symbols { get;private set; }
        public bool IsFrozen { get; set; }
        public int CurrentSymbolIndex { get;private set; }
        public Symbol CurrentSymbol => Symbols[CurrentSymbolIndex];

        public IReelView View { get; set; }

        private readonly List<ReelEffect> effects = new();

        public void AddEffect(ReelEffect effect) => effects.Add(effect);

        public void RemoveEffect(ReelEffect effect) => effects.Remove(effect);

        public Reel(List<Symbol> symbols)
        {
            Symbols = symbols;
        }
        
        public async UniTask Spin(float delay)
        {
            foreach (var effect in effects)
            {
                if (!effect.OnBeforeSpin(this))
                {
                    effect.OnAfterSpin(this);
                    return;
                }
            }
            CurrentSymbolIndex=Services.Random.Range(0, Symbols.Count);
            var item = Symbols[CurrentSymbolIndex];
            await View.SpinAnimation(item.Id,delay);
            foreach (var effect in effects)
            {
                effect.OnAfterSpin(this);
            }
        }
        
    }
}