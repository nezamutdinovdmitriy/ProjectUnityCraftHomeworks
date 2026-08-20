using UnityEngine;

namespace SampleGame
{
    public sealed class MoveTransformComponent : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;
        
        public void MoveStep(Vector3 direction, float deltaTime)
        {
            this.transform.position += direction * (_moveSpeed * deltaTime);
        }
    }
}