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
    }
}