using UnityEngine;

namespace SampleGame.AI
{
    public readonly struct FollowCommandData : ICommandData
    {
        public CommandType CommandType => CommandType.Follow;

        public readonly GameObject Target;

        public FollowCommandData(GameObject target)
        {
            Target = target;
        }

        public bool IsValid => Target != null;
    }
}