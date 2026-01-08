using System;
using Game.Event;
using UnityEngine;

namespace Game
{
    public class MainMenu:MonoBehaviour
    {
        public void NewGame()
        {
            Services.GameSetup.StartNewRun();
        }

        private void Awake()
        {
            EventBus.OnLightChanged?.Invoke(0.1f);
        }
    }
}