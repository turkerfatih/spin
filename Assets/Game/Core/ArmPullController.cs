using System;
using DG.Tweening;
using Game.Actions;
using UnityEngine;

namespace Game.Core
{
    public class ArmPullController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform arm;

        [SerializeField] private Collider headCollider;

        [Header("Angles")] [SerializeField] private float startX = -50f;
        [SerializeField] private float pulledX = -100f;
        [SerializeField] private float completeThreshold = 0.98f;

        [Header("Spring Settings")] [SerializeField]
        private float springStrength = 120f; // k

        [SerializeField] private float damping = 18f; // d
        [SerializeField] private float dragForce = 3f;



        private float currentX;
        private float velocityX;
        private float springTargetX;

        private bool isDragging;
        private float lastMouseY;

        private void Start()
        {
            currentX = startX;
            springTargetX = startX;
            ApplyRotation();
        }

        private void Update()
        {
            HandleInput();
            SimulateSpring(Time.deltaTime);
            ApplyRotation();
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                TryStartPull();

            if (isDragging && Input.GetMouseButton(0))
                Drag();

            if (isDragging && Input.GetMouseButtonUp(0))
                Release();
        }

        private void TryStartPull()
        {
            Ray ray = Services.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit))
                return;

            if (hit.collider != headCollider)
                return;

            isDragging = true;
            lastMouseY = Input.mousePosition.y;
            velocityX = 0f;
        }

        private void Drag()
        {
            float deltaY = Input.mousePosition.y - lastMouseY;
            lastMouseY = Input.mousePosition.y;

            // Mouse DOWN (deltaY < 0) → pull
            // Mouse UP   (deltaY > 0) → release
            //float force = deltaY * dragForce;
            float force = Mathf.Clamp(deltaY, -30f, 30f) * dragForce;

            velocityX += force;

            // While dragging, don't let the spring fight the hand
            springTargetX = currentX;
        }

        private void Release()
        {
            isDragging = false;

            float progress = Mathf.InverseLerp(startX, pulledX, currentX);

            if (progress >= completeThreshold)
            {
                springTargetX = pulledX;
                if(!Services.Machine.CanSpin())
                    return;
                Services.Actions.Add(new SpinAction());
            }
            else
            {
                springTargetX = startX;
            }
        }

        private void SimulateSpring(float dt)
        {
            float displacement = currentX - springTargetX;

            float springForce = -springStrength * displacement;
            float dampingForce = -damping * velocityX;

            velocityX += (springForce + dampingForce) * dt;
            currentX += velocityX * dt;

            currentX = Mathf.Clamp(currentX, pulledX, startX);
        }

        private void ApplyRotation()
        {
            Vector3 euler = arm.localEulerAngles;
            euler.x = currentX;
            arm.localEulerAngles = euler;
        }
    }
}