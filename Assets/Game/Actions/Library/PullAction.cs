using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class PullAction:IGameAction
    {
        private readonly int reelIndex;
        private readonly bool isPull;
        public PullAction(int reel,bool push=false)
        {
            isPull=!push;
            reelIndex=reel;
        }
        public async UniTask ExecuteAsync()
        {
            var machine = Services.Machine;
            machine.IsSpinning = true;
            if(isPull)
                await Services.Machine.Reels[reelIndex].Pull(0);
            else
            {
                await Services.Machine.Reels[reelIndex].Push(0);
            }
            await machine.ResolvePayout();
            //await machine.AdvanceSlots();
            await Services.WinLoseCheck.ExecuteAsync();
            machine.IsSpinning = false;
        }
    }
}