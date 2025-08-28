using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class SpinAction:IGameAction
    {
        public async UniTask ExecuteAsync()
        {
            Services.SlotMachine.IsSpinning = true;
            Services.Sound.StartSpinning();
            await Services.MachineView.Spin();
            Services.Sound.StopSpinning();
            Services.SlotMachine.IsSpinning = false;
        }
    }
}