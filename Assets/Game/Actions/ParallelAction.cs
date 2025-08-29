using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class ParallelAction:IGameAction
    {
        private readonly List<IGameAction> actions;

        public ParallelAction(params IGameAction[] actions)
        {
            this.actions = new List<IGameAction>(actions);
        }

        public ParallelAction(List<IGameAction> actions)
        {
            this.actions = actions;
        }

        public async UniTask ExecuteAsync()
        {
            var tasks = new List<UniTask>();
            foreach (var action in actions)
            {
                tasks.Add(action.ExecuteAsync());
            }

            await UniTask.WhenAll(tasks);
        }
    }
}