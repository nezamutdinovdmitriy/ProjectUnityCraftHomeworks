using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace SampleGame.AI
{
    [BlackboardAPI]
    public static class BlackboardAPI
    {
        public static readonly BlackboardValueKey<GameObject> Character = new(nameof(Character));

        public static readonly BlackboardValueKey<CommandType> CurrentCommandType = new(nameof(CurrentCommandType));
        public static readonly BlackboardValueKey<ICommandData> CurrentCommand = new(nameof(CurrentCommand));
        public static readonly BlackboardValueKey<Queue<ICommandData>> CommandQueue = new(nameof(CommandQueue));
        public static readonly BlackboardValueKey<int> PatrolPointIndex = new(nameof(PatrolPointIndex));
    }
}