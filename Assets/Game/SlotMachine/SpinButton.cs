using System;
using Game.Actions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class SpinButton:MonoBehaviour
    {
        private void Awake()
        {
            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Add(new EventTrigger.Entry()
            {
                eventID = EventTriggerType.PointerClick,
            });
            trigger.triggers[0].callback.AddListener(Click);
        }
        
        private void Click(BaseEventData eventData)
        {
            Services.Actions.Add(new SpinAction());
            //Services.SlotMachine.Spin();
        }
    }
}