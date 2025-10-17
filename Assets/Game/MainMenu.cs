using UnityEngine;

namespace Game
{
    public class MainMenu:MonoBehaviour
    {
        public void NewGame()
        {
            Services.GameSetup.StartNewRun();
        }
    }
}