using UnityEngine;

namespace Game
{
    public class CloudParallax : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float resetPositionX = -20f;
        [SerializeField] private float startPositionX = 20f;

        private void Update()
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            if (transform.position.x <= resetPositionX)
            {
                transform.position = new Vector3(
                    startPositionX,
                    transform.position.y,
                    transform.position.z
                );
            }
        }
    }
}