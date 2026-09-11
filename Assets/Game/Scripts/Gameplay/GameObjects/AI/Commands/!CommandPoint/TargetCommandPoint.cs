using UnityEngine;

namespace SampleGame.AI
{
    public readonly struct TargetCommandPoint : ICommandPoint
    {
        public readonly GameObject Target;

        public TargetCommandPoint(GameObject target) 
            => Target = target;

        public Vector3? GetPosition() 
            => Target.transform.position;
    }
}