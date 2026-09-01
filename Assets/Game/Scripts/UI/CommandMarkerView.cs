using UnityEngine;

namespace SampleGame
{
    public sealed class CommandMarkerView : MonoBehaviour
    {
        [SerializeField]
        private float offsetY = 0.15f;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private ParticleSystem _moveMarkerPrefab;

        [SerializeField]
        private ParticleSystem _patrolMarkerPrefab;

        [SerializeField]
        private ParticleSystem _holdPositionPrefab;

        [SerializeField]
        private ParticleSystem _followMarkerPrefab;

        [SerializeField]
        private ParticleSystem _attackMarkerPrefab;

        public void ShowAttackMarker(Vector3 position)
        {
            Vector3 spawnPosition = position + new Vector3(0, offsetY, 0);
            Instantiate(_attackMarkerPrefab, spawnPosition, Quaternion.identity, _container);
        }

        public void ShowAttackMarker(Transform target)
        {
            Vector3 position = target.position + new Vector3(0, offsetY, 0);
            Instantiate(_attackMarkerPrefab, position, Quaternion.identity, target);
        }

        public void ShowFollowMarker(Vector3 position)
        {
            Vector3 spawnPosition = position + new Vector3(0, offsetY, 0);
            Instantiate(_followMarkerPrefab, spawnPosition, Quaternion.identity, _container);
        }

        public void ShowFollowMarker(Transform target)
        {
            Vector3 position = target.position + new Vector3(0, offsetY, 0);
            Instantiate(_followMarkerPrefab, position, Quaternion.identity, _container);
        }

        public void ShowMoveMarker(Vector3 position)
        {
            Vector3 spawnPosition = position + new Vector3(0, offsetY, 0);
            Instantiate(_moveMarkerPrefab, spawnPosition, Quaternion.identity, _container);
        }
        
        public void ShowMoveMarker(Transform target)
        {
            Vector3 position = target.position + new Vector3(0, offsetY, 0);
            Instantiate(_moveMarkerPrefab, position, Quaternion.identity, _container);
        }

        public void ShowPatrolMarker(Vector3 position)
        {
            Vector3 spawnPosition = position + new Vector3(0, offsetY, 0);
            Instantiate(_patrolMarkerPrefab, spawnPosition, Quaternion.identity, _container);
        }
        
        public void ShowPatrolMarker(Transform target)
        {
            Vector3 position = target.position + new Vector3(0, offsetY, 0);
            Instantiate(_patrolMarkerPrefab, position, Quaternion.identity, _container);
        }
        
        public void ShowHoldPositionMarker(Vector3 position)
        {
            Vector3 spawnPosition = position + new Vector3(0, offsetY, 0);
            Instantiate(_holdPositionPrefab, spawnPosition, Quaternion.identity, _container);
        }
    }
}