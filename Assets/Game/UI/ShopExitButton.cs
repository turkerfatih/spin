using Game.Core.UI;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class ShopExitButton:Button
    {
        protected override void SubmitAction()
        {
            SceneManager.LoadSceneAsync(Scenes.Round, LoadSceneMode.Single);
        }
    }
}