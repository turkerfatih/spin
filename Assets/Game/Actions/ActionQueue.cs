using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class ActionQueue
    {
        private readonly Queue<IGameAction> queue = new();
        private bool isRunning;

        public void Add(IGameAction action)
        {
            queue.Enqueue(action);
            if (!isRunning)
            {
                RunAsync().Forget(); 
            }
        }

        private async UniTaskVoid RunAsync()
        {
            isRunning = true;

            while (queue.Count > 0)
            {
                var action = queue.Dequeue();
                await action.ExecuteAsync();
            }

            isRunning = false;
        }
    }
}