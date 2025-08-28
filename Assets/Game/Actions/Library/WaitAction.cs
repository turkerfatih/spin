using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class WaitAction:IGameAction
    {
        private readonly float seconds;
        public WaitAction(float seconds) => this.seconds = seconds;
        public async UniTask ExecuteAsync()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(seconds));
        }
    }
}