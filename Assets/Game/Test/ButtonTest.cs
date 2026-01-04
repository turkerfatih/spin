using System;
using Game.Core.Animation;
using UnityEngine;

namespace Game.Test
{
    public class ButtonTest:MonoBehaviour
    {
        [SerializeField] private Transform MoveHandle;
        [SerializeField] private Transform Frame;

        private float position;
        private float velocity;

        public float Frequency=15;
        public float Damping=0.5f;
        
        public float goalPosition;
        public float goalScale;
        
        
        public float downHeight = -10f;
        public float upHeight = 0f;

        public float downScale = 0.75f;
        public float upScale = 1f;

        public bool wasPressed = false;

        private bool reachedGoal = false;

        private void Update()
        {
            float time=Time.deltaTime;
            Springs.CalcDampedSimpleHarmonicMotion(ref position,ref velocity
                ,goalPosition,time,Frequency,Damping);
            OnSpringValue(position);
            if (!reachedGoal)
            {
                if (Springs.Approximately(goalPosition, position))
                {
                    Debug.Log("goal reached");
                    reachedGoal = true;
                }
            }
        }

        private void OnMouseDown()
        {
            SetPressed(true);
        }

        private void OnMouseUpAsButton()
        {
            SetPressed(false);
        }

        public void SetPressed(bool isPressed)
        {
            if (wasPressed != isPressed)
            {
                wasPressed = isPressed;
                if (isPressed)
                {
                    goalPosition = downHeight;
                    goalScale = downScale;
                }
                else
                {
                    goalPosition = upHeight;
                    goalScale = upScale;
                }
                reachedGoal = false;
            }
        }

        public void OnSpringValue(float springValue)
        {
            OnSpringUpdated(springValue);
            OnSpringUpdatedScale(springValue);
        }

        private void OnSpringUpdated(float springValue)
        {
            Vector3 offset = Vector3.up*springValue;
            MoveHandle.localPosition = offset;
        }

        private void OnSpringUpdatedScale(float springValue)
        {
            Vector3 newScale = new Vector3(1f,springValue,1f);
            MoveHandle.localScale = newScale;
        }
    }
}