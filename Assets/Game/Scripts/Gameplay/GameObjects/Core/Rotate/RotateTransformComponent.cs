using UnityEngine;

namespace SampleGame
{
    public sealed class RotateTransformComponent : MonoBehaviour
    {
        [SerializeField]
        private float _angularSpeed;

        public void RotateTowards(GameObject target, float deltaTime)
        {
            if (!target)
                return;
            
            Vector3 distance = target.transform.position - this.transform.position;
            distance.y = 0;
            
            this.RotateTowards(distance.normalized, deltaTime);
        }

        public void RotateTowards(Vector3 direction, float deltaTime)
        {
            if (direction == Vector3.zero)
                return;

            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion next = Quaternion.RotateTowards(this.transform.rotation, target, _angularSpeed * deltaTime);
            this.transform.rotation = next;
        }
        
        public void LookAt(GameObject target)
        {
            if (!target)
                return;
            
            Vector3 distance = target.transform.position - this.transform.position;
            distance.y = 0;
            
            this.LookAt(distance.normalized);
        }
        
        public void LookAt(Vector3 direction)
        {
            if (direction != Vector3.zero) 
                this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}