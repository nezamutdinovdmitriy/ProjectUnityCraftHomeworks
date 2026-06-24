using UnityEngine;

namespace Game
{
    public sealed class MoveTransformComponent : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 4.5f;

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero) 
                this.transform.Translate((Vector3) direction * _speed * Time.fixedDeltaTime);
        }
    }
}