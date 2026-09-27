using Modules.AI;
using UnityEngine;

namespace SampleGame.AI.BehaviourTree.Conditions
{
    public class IsSelfAliveCondition : ICondition
    {
        [SerializeField] private Blackboard _blackboard;
        
        public bool Invoke()
        {
            if (_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character)
                && character.TryGetComponent(out HealthComponent health)
                && health.IsAlive)
                return true;

            return false;
        }
    }
}