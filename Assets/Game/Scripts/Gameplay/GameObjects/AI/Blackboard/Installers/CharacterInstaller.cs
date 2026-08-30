using Modules.AI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame.AI
{
    public class CharacterInstaller : IBlackboardInstaller
    {
        [SerializeField]
        private float _stoppingDistance;
        
        [ShowInInspector, HideInEditorMode]
        private Vector3 _targetPosition;
        
        [SerializeField]
        private int _colliderBufferSize = 5;
        
        public void Install(Blackboard blackboard)
        {
            blackboard.AddPrimitiveValue(BlackboardAPI.TargetPosition, _targetPosition);
            blackboard.AddPrimitiveValue(BlackboardAPI.StoppingDistance, _stoppingDistance);
            blackboard.AddReferenceValue(BlackboardAPI.ColliderBuffer, new Collider[_colliderBufferSize]);
        }
    }
}