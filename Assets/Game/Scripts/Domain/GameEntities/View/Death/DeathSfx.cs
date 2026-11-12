using Modules.AudioEvents;
using Unity.Entities;

namespace Game.Components
{
    public struct DeathSfx : IComponentData
    {
        public AudioEventKey Value;
    }
}