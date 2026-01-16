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
            EventBus.OnLightValueChanged+= OnLightChanged;
            EventBus.OnLightRotationChanged += OnLightRotationChanged;
        }

        private void OnDisable()
        {
            EventBus.OnLightValueChanged-= OnLightChanged;
            EventBus.OnLightRotationChanged -= OnLightRotationChanged;
        }

        private void OnLightChanged(float intensity)
        {
            light.intensity = intensity;
        }

        private void OnLightRotationChanged(Vector3 rot)
        {
            light.transform.localRotation = Quaternion.Euler(rot);
        }
    }
}