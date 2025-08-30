using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;

namespace Game.Actions
{
    public class SpinAction:IGameAction
    {
        private static InfiniteRandomBag<float> delays=new (new[] { 0, 0.5f, 1f });
        private readonly List<UniTask> tasks = new List<UniTask>();
        public async UniTask ExecuteAsync()
        {
            tasks.Clear();
            var machine = Services.Machine;
            machine.IsSpinning = true;
            var count = machine.Count;
            delays.Reset();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(Services.Machine.Reels[i].Spin(delays.GetRandom()));   
            }
            await UniTask.WhenAll(tasks);
            await machine.ResolveSpin();
            machine.IsSpinning = false;
        }
    }
}