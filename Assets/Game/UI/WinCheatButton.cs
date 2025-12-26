using Game.Core.UI;

namespace Game.UI
{
    public class WinCheatButton:Button
    {
        protected override void SubmitAction()
        {
            Services.WinLoseCheck.PreWinDisplayAction();
            Services.WinDisplay.Show();
        }
    }
}