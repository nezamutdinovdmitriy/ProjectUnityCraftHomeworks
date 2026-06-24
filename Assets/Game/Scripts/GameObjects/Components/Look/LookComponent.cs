using UnityEngine;

namespace Game
{
    public sealed class LookComponent : MonoBehaviour
    {
        public void Look(Transform target)
        {
            Vector2 direction = target.position - this.transform.position;
            this.Look(direction.x);
        }
        
        public void Look(float direction)
        {
            float angle = direction > 0 ? 0 : 180;
            this.transform.eulerAngles = new Vector3(0, angle, 0);
        }
    }
}