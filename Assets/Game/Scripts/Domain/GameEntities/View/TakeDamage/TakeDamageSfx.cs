using Modules.AudioEvents;
using Unity.Entities;

namespace Game.Components
{
    public struct TakeDamageSfx : IComponentData
    {
        public AudioEventKey Key;
    }
}