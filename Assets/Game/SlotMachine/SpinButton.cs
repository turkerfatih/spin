using System;
using Game.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class SpinButton:MonoBehaviour
    {
        private void OnMouseUpAsButton()
        {
            Services.Actions.Add(new SpinAction());
        }
    }
}