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

        private async UniTask SpinWith(UniTask task)
        {
            foreach (var effect in effects)
            {
                if (!effect.OnBeforeSpin(this))
                {
                    effect.OnAfterSpin(this);
                    return;
                }
            }
            await task;
            foreach (var effect in effects)
            {
                effect.OnAfterSpin(this);
            }
        }
        public async UniTask Spin(float delay)
        {
            await SpinWith(SpinTask(delay));
        }

        private  UniTask SpinTask(float delay)
        {
            CurrentSymbolIndex=Services.Random.Range(0, Symbols.Count);
            var item = Symbols[CurrentSymbolIndex];
            return View.SpinAnimation(item.Id,delay);
        }

        public async UniTask Pull(float delay)
        {
            await SpinWith(PullTask(delay));
        }
        public async UniTask Push(float delay)
        {
            await SpinWith(PushTask(delay));
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

    }
}