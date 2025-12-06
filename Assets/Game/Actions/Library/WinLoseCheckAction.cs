using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class WinLoseCheckAction:IGameAction
    {
        public  UniTask ExecuteAsync()
        {
            if (Services.Round.IsWin())
            {
                Services.WinDisplay.Show();
                return UniTask.CompletedTask;
            }

            if (Services.Round.IsLose())
            {
                Services.GameSetup.LoseRun();
            }

            return UniTask.CompletedTask;
        }
    }
}