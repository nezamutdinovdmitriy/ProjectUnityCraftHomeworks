using UnityEngine;

namespace SampleGame.AI
{
    public struct CommandPoint
    {
        public readonly Vector3? Point;
        public readonly GameObject Target;
        
        public CommandPoint(Vector3? point)
        {
            Point = point;
            Target = null;
        }
        
        public CommandPoint(GameObject target)
        {
            Target = target;
            Point = null;
        }
    }
}