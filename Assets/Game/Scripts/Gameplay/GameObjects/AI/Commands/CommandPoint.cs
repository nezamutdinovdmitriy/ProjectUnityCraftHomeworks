using UnityEngine;

namespace SampleGame.AI
{
    public struct CommandPoint
    {
        public readonly Vector3? Position;
        public readonly GameObject Target;
        
        public CommandPoint(Vector3? position)
        {
            Position = position;
            Target = null;
        }
        
        public CommandPoint(GameObject target)
        {
            Target = target;
            Position = null;
        }
    }
}