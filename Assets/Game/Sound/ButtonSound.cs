using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    private void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick,
            
        });
        
        trigger.triggers[0].callback.AddListener(PlayClick);
        trigger.triggers.Add(new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerEnter,
            
        });
        trigger.triggers[1].callback.AddListener(PlayHover);
    }

    private void PlayClick(BaseEventData eventData)
    {
        Services.Sound.PlayButtonClick();
    }
    private void PlayHover(BaseEventData eventData)
    {
        Services.Sound.PlayButtonHover();
    }
}
