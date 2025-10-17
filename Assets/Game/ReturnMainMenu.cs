using Game.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ReturnMainMenu:Button
    {
        protected override void SubmitAction()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}