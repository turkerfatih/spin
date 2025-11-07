using UnityEngine;

namespace Game.Core.UI
{
    public class BackButton:Button
    {
        protected override void SubmitAction()
        {
            WindowManager.Instance.Hide();
        }
    }
}