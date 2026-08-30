using System;
using Modules.AI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SampleGame.AI.BehaviourTree.Sensors
{
    public class FindNearestEnemySensor : MonoBehaviour
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;

        [Space] [SerializeField]
        private float _minCooldown = 0.3f;

        [SerializeField]
        private float _maxCooldown = 0.6f;
        
        [SerializeField]
        private float _detectRadius;

        [SerializeField]
        private Transform _self;
        
        private float _cooldown;
        
        private readonly Collider[] _buffer = new Collider[10];
        
        private void FixedUpdate()
        {
            _cooldown -= Time.fixedDeltaTime;

            if (_cooldown <= 0)
            {
                UpdateTarget();
                _cooldown = Random.Range(_minCooldown, _maxCooldown);
            }
        }

        private void UpdateTarget()
        {
            int size = Physics.OverlapSphereNonAlloc(_self.transform.position, _detectRadius, _buffer);

            float minSqrDistance = float.MaxValue;
            
            GameObject nearestTarget = null;

            for (int i = 0; i < size; i++)
            {
                Collider collider = _buffer[i];

                if (collider.TryGetComponent(out TeamComponent teamComponent)
                    && _self.gameObject.TryGetComponent(out TeamComponent selfTeamComponent)
                    && selfTeamComponent.Team != teamComponent.Team)
                {
                    float sqrDistance = (collider.transform.position - _self.transform.position).sqrMagnitude;
                    
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestTarget = collider.gameObject;
                    }
                }
            }

            Debug.Log(nearestTarget);
            
            if (nearestTarget != null)
                _blackboard.SetReferenceValue(BlackboardAPI.Target, nearestTarget);
            else
                _blackboard.DelValue(_targetKey);
        }
    }
}