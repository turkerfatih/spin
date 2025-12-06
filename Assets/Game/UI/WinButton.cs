using Game.Core.UI;
using UnityEngine;

namespace Game.UI
{
    public class WinButton:Button
    {
        private WinDisplay winDisplay;
        
        protected override void SubmitAction()
        {
            Debug.Log("Win popup close");
            winDisplay.Close();
        }

        public void SetPopup(WinDisplay display)
        {
            winDisplay = display;
        }
    }
}