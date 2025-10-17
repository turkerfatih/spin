using System;
using UnityEngine;

namespace Game
{
    public class SharedLoader:MonoBehaviour
    {
        [SerializeField]
        private Shared prefab = null;

        private void Awake()
        {
            if (Shared.Instance == null)
            {
                Instantiate(prefab);
            }
        }

        public void NewGame()
        {
            Services.GameSetup.StartNewRun();
        }
    }
}