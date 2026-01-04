using UnityEngine;

namespace Game.Core.Animation
{
    public class LookAtSpring : MonoBehaviour
    {
        public Transform target;
        public QuaternionSpring rotationSpring = new QuaternionSpring();

        void Update()
        {
            // 1. Determine where we want to look
            Vector3 direction = target.position - transform.position;
            rotationSpring.EndValue = Quaternion.LookRotation(direction);

            // 2. Update the spring
            transform.rotation = rotationSpring.Evaluate(Time.deltaTime);
        }
    }
}