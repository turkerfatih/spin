using System;
using Cysharp.Threading.Tasks;

namespace Game.Core
{
    public interface IReelView
    {
        public UniTask SpinAnimation(Guid symbolId, float delay = 0f);
        public UniTask FreezeAnimation();
        
        public UniTask PullAnimation(float delay = 0f);
        public UniTask PushAnimation(float delay = 0f);
        public UniTask AnimateMatch(Guid id,float delay = 0f);
    }
}