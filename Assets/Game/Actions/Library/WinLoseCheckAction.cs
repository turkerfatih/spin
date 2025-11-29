using Cysharp.Threading.Tasks;

namespace Game.Actions
{
    public class WinLoseCheckAction:IGameAction
    {
        public async UniTask ExecuteAsync()
        {
            if (Services.Round.IsWin())
            {
                Services.CardSelection.Show();
                return;
            }

            if (Services.Round.IsLose())
            {
                Services.GameSetup.LoseRun();
            }
        }
    }
}