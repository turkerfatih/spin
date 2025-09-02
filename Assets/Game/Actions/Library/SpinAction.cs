using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;

namespace Game.Actions
{
    public class SpinAction:IGameAction
    {

        private static InfiniteRandomBag<float> delays=new (new[] { 0, 0.15f, 0.25f });
        private readonly List<UniTask> tasks = new List<UniTask>();
        private readonly int single=-1;

        public SpinAction()
        {
            
        }
        public SpinAction(int reelIndex)
        {
            single = reelIndex;
        }


        public async UniTask ExecuteAsync()
        {
            tasks.Clear();
            var machine = Services.Machine;
            machine.IsSpinning = true;
            var count = machine.Count;
            delays.Reset();
            for (int i = 0; i < count; i++)
            {
                if(single>=0 && single!=i)
                    continue;
                tasks.Add(Services.Machine.Reels[i].Spin(delays.GetRandom()));   
            }
            await UniTask.WhenAll(tasks);
            await machine.ResolveSpin();
            await machine.AdvanceSlots();
            machine.IsSpinning = false;
        }
    }
}