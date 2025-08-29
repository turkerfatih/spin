using System;
using Cysharp.Threading.Tasks;

namespace Game.Core
{
    public interface IReelView
    {
        public UniTask SpinAnimation(Guid symbolId, float delay = 0f);
        public UniTask FreezeAnimation();
    }
}