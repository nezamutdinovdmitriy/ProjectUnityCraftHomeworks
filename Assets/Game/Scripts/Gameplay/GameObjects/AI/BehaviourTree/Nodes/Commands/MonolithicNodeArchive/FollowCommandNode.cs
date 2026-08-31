// using Modules.AI;
// using UnityEngine;
//
// namespace SampleGame.AI
// {
//     public class FollowCommandNode : BehaviourNode
//     {
//         [SerializeField]
//         private Blackboard _blackboard;
//         
//         [SerializeField]
//         private float _stoppingDistance;
//         
//         protected override BehaviourResult OnUpdate(float deltaTime)
//         {
//             if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out ICommandData command) == false
//                 || command is not FollowCommandData followCommandData)
//                 return BehaviourResult.Failure;
//             
//             if (followCommandData.IsValid == false)
//             {
//                 _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, new DefaultCommandData());
//                 return BehaviourResult.Failure;
//             }
//             
//             if (_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) == false
//                 || character.TryGetComponent(out MoveComponent moveComponent) == false)
//                 return BehaviourResult.Failure;
//             
//             Vector3 selfPosition = character.transform.position;
//             Vector3 targetPosition = followCommandData.Target.transform.position;
//             Vector3 vector = targetPosition - selfPosition;
//             vector.y = 0f;
//             
//             float sqrStoppingDistance = _stoppingDistance * _stoppingDistance;
//             bool isReached = vector.sqrMagnitude <= sqrStoppingDistance;
//             
//             if (isReached)
//                 return BehaviourResult.Success;
//             
//             Vector3 direction = vector.normalized;
//             moveComponent.MoveStep(direction, deltaTime);
//             
//             return BehaviourResult.Running;
//         }
//     }
// }