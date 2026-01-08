using System;
using Game.Event;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Game.Core.UI
{
    public class GlobalLightChanger:MonoBehaviour
    {
        private Light light;
        private void Awake()
        {
            light = GetComponent<Light>();
        }

        private void OnEnable()
        {
            EventBus.OnLightChanged+= OnLightChanged;
        }

        private void OnDisable()
        {
            EventBus.OnLightChanged-= OnLightChanged;
        }

        private void OnLightChanged(float intensity)
        {
            light.intensity = intensity;
        }
    }
}