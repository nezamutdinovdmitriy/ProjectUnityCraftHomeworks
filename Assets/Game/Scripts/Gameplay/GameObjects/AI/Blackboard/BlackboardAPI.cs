using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    [BlackboardAPI]
    public static class BlackboardAPI
    {
        // Common
        public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));
        public static readonly BlackboardValueKey<GameObject> Target = new(nameof(Target));
        public static readonly BlackboardValueKey<Collider[]> ColliderBuffer = new(nameof(ColliderBuffer));
        
        // Commands
        public static readonly BlackboardValueKey<CommandType> CurrentCommandType = new(nameof(CurrentCommandType));
        public static readonly BlackboardValueKey<ICommandData> CurrentCommand = new(nameof(CurrentCommand));
        public static readonly BlackboardValueKey<Queue<ICommandData>> CommandQueue = new(nameof(CommandQueue));
        
        // Patrol
        public static readonly BlackboardValueKey<int> PatrolPointIndex = new(nameof(PatrolPointIndex));
        
        // Movement
        public static readonly BlackboardValueKey<Vector3> TargetPosition = new(nameof(TargetPosition));
        public static readonly BlackboardValueKey<float> StoppingDistance = new(nameof(StoppingDistance));
    }
}