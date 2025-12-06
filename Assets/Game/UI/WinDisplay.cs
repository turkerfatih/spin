using System;
using Game.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class WinDisplay:PopupWindow
    {
        [SerializeField] WinButton ContinueButton; 
        private void Awake()
        {
            gameObject.SetActive(false);
            Services.WinDisplay = this;
            ContinueButton.SetPopup(this);
            
        }
        
        public override bool HasCustomClose => true;

        public void Close()
        {
            Hide();
            SceneManager.LoadScene(Scenes.Shop, LoadSceneMode.Single);
        }
    }
}