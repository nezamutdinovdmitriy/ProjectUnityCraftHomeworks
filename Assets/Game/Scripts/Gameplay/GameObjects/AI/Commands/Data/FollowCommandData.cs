using UnityEngine;

namespace SampleGame.AI
{
    public readonly struct FollowCommandData : ICommandData
    {
        public readonly GameObject Target;

        public FollowCommandData(GameObject target) 
            => Target = target;

        public bool IsValid 
            => Target != null;

        public CommandType Type => CommandType.Follow;
    }
}