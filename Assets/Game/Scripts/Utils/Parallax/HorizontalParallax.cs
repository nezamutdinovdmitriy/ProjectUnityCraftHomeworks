using UnityEngine;

namespace Game
{
    public class HorizontalParallax : MonoBehaviour
    {
        [SerializeField]
        private float parallaxMultiplier = 0.5f;

        private Transform cameraTransform;
        private Vector3 startPosition;
        private Vector3 startCameraPosition;

        private void Start()
        {
            cameraTransform = Camera.main.transform;
            startPosition = transform.position;
            startCameraPosition = cameraTransform.position;
        }

        private void FixedUpdate()
        {
            // float deltaX = cameraTransform.position.x - startCameraPosition.x;
            //
            // float newX = startPosition.x + deltaX * parallaxMultiplier;
            //
            // transform.position = new Vector3(
            //     newX,
            //     startPosition.y,
            //     transform.position.z
            // );
            
            Vector3 delta = cameraTransform.position - startCameraPosition;
            Vector3 newX = startPosition + delta * parallaxMultiplier;
            this.transform.position = newX;
        }
    }
}