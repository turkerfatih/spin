using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class SequenceAction:IGameAction
    {
        private readonly List<IGameAction> actions;

        public SequenceAction(params IGameAction[] actions)
        {
            this.actions = new List<IGameAction>(actions);
        }

        public SequenceAction(List<IGameAction> actions)
        {
            this.actions = actions;
        }

        public async UniTask ExecuteAsync()
        {
            foreach (var action in actions)
            {
                await action.ExecuteAsync();
            }
        }
    }
}