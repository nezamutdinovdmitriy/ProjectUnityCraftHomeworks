using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.AI
{
    [AddComponentMenu("AI/BehaviourTree/BehaviourNode «WaitForSeconds»")]
    public sealed class BehaviourNodeWaitForSeconds : BehaviourNode
    {
        [Space]
        [SerializeField]
        private float _duration;

        [HideInEditorMode]
        [ShowInInspector, ReadOnly]
        private float _time;

        protected override void OnStart()
        {
            _time = _duration;
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            _time -= deltaTime;
            return _time <= 0 ? BehaviourResult.Success : BehaviourResult.Running;
        }
    }
}