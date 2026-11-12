using Modules.AudioEvents;
using Unity.Entities;

namespace Game.Components
{
    public struct AbilitySounds : IComponentData
    {
        public AudioEventKey Started;
        public AudioEventKey Executed;
    }
}