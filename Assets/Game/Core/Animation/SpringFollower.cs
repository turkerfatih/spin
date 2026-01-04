using UnityEngine;

namespace Game.Core.Animation
{
    public class SpringFollower : MonoBehaviour
    {
        public Transform target;
        public Vector3Spring spring = new Vector3Spring();

        void Start() => spring.Reset(transform.position);

        void Update()
        {
            // 1. Target is moving, so update the goal
            spring.EndValue = target.position;

            // 2. Solve the spring
            transform.position = spring.Evaluate(Time.deltaTime);
            Quaternion rotation = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0);
        }
    }
}