using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class MainMenuSetup:MonoBehaviour
    {
        [SerializeField] private Shared SharedPrefab;
        [SerializeField] private MainMenu MainMenuPrefab;
        

        private void Start()
        {
            if (Shared.Instance == null)
            {
                Instantiate(SharedPrefab);
            }
            var mainMenu = Instantiate(MainMenuPrefab);
        }
        
    }
}