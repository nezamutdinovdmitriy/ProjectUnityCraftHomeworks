using System;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.Sensors
{
    public class FindNearestEnemySensor : MonoBehaviour
    {
        [Header("AI")]
        [SerializeField]
        private Blackboard _blackboard;

        [Space]
        [Header("Sensor Settings")]
        [SerializeField]
        private float _detectRadius;
        [SerializeField] private float _updateInterval;
        [SerializeField] private int _bufferSize;

        private float _time;
        private Collider[] _buffer;

        private void Awake() => _buffer = new Collider[_bufferSize];

        private void Update()
        {
            _time -= Time.deltaTime;

            if (_time > 0)
                return;

            _time = _updateInterval;

            if (_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
                || character.TryGetComponent(out TeamComponent teamComponent) == false)
                throw new InvalidOperationException("Required components are missing!");

            if (TryGetNearestTarget(_buffer, character, teamComponent, out GameObject nearestTarget))
                _blackboard.SetReferenceValue(BlackboardAPI.NearestEnemy, nearestTarget);
            else
                _blackboard.DelValue(BlackboardAPI.NearestEnemy);
        }

        private bool TryGetNearestTarget(Collider[] buffer, GameObject self, TeamComponent selfTeamComponent,
            out GameObject nearestTarget)
        {
            Vector3 selfPosition = self.transform.position;

            int size = Physics.OverlapSphereNonAlloc(selfPosition, _detectRadius, buffer);

            float minSqrDistance = float.MaxValue;
            nearestTarget = null;

            for (int i = 0; i < size; i++)
            {
                Collider collider = buffer[i];

                if (collider.TryGetComponent(out TeamComponent teamComponent) == false
                    || selfTeamComponent.Team == teamComponent.Team
                    || collider.TryGetComponent(out HealthComponent healthComponent) == false
                    || healthComponent.IsDead)
                    continue;

                float sqrDistance = (collider.transform.position - selfPosition).sqrMagnitude;

                if (sqrDistance >= minSqrDistance)
                    continue;

                minSqrDistance = sqrDistance;
                nearestTarget = collider.gameObject;
            }

            return nearestTarget != null;
        }
    }
}